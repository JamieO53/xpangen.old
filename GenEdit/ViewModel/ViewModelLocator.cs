﻿// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.ComponentModel;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.ViewModel
{
    public static class ViewModelLocator
    {
        private static GeData _geData;
        private static GenDataEditorViewModel _genDataEditorViewModel;

        private static bool IsInDesignMode {
            get {
                var isInDesignMode = LicenseManager.UsageMode == LicenseUsageMode.Designtime; // || Debugger.IsAttached;

                //if (!isInDesignMode) {
                //    using (var process = Process.GetCurrentProcess()) {
                //        return process.ProcessName.ToLowerInvariant().Contains("devenv") ||
                //            process.ProcessName.ToLowerInvariant().Contains(".vshost");
                //    }
                //}

                return isInDesignMode;
            }
        }

        private static GeData GeData
        {
            get { return _geData ?? (_geData = GeData.GetDefaultGeData(IsInDesignMode)); }
        }

        public static GenDataEditorViewModel GenDataEditorViewModel
        {
            get { return _genDataEditorViewModel ?? (_genDataEditorViewModel = new GenDataEditorViewModel {Data = GeData}); }
        }
    }
}
