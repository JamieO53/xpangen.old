// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace GenEdit.ViewModel
{
    public static class ViewModelLocator
    {
        private static GeData _geData;
        private static GenDataEditorViewModel _genDataEditorViewModel;

        public static bool IsInDesignMode {
            get {
                var isInDesignMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime || Debugger.IsAttached;

                if (!isInDesignMode) {
                    using (var process = Process.GetCurrentProcess()) {
                        return process.ProcessName.ToLowerInvariant().Contains("devenv") ||
                            process.ProcessName.ToLowerInvariant().Contains(".vshost");
                    }
                }

                return true;
            }
        }

        private static GeData GeData
        {
            get { return _geData ?? (_geData = GetDefaultGeData()); }
        }

        private static GeData GetDefaultGeData()
        {
            var geData = new GeData();
            //var processName = Process.GetCurrentProcess().ProcessName;
            //if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            //    MessageBox.Show(processName, "Process name", MessageBoxButtons.OK);
            //Clipboard.SetText(processName);
            //if (IsInDesignMode)
            {
                geData.Testing = true;
                if (File.Exists(@"Data\ProgramDefinition.dcb"))
                {
                    geData.GenDataStore.SetBase(@"Data\ProgramDefinition.dcb");
                    geData.GenDataStore.SetData(@"Data\GeneratorDefinitionModel.dcb");
                    geData.Profile = new GenCompactProfileParser(geData.GenData, @"Data\GenProfileModel.prf", "");
                }
            } 
            return geData;
        }
        
        public static GenDataEditorViewModel GenDataEditorViewModel
        {
            get { return _genDataEditorViewModel ?? (_genDataEditorViewModel = new GenDataEditorViewModel {Data = GeData}); }
        }
    }
}
