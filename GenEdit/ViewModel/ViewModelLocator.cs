using System;
using System.Diagnostics;
using org.xpangen.Generator.Editor.Helper;
using org.xpangen.Generator.Profile.Parser.CompactProfileParser;

namespace GenEdit.ViewModel
{
    public static class ViewModelLocator
    {
        private static GeData _geData;
        private static GenDataEditorViewModel _genDataEditorViewModel;

        private static GeData GeData
        {
            get { return _geData ?? (_geData = GetDefaultGeData()); }
        }

        private static GeData GetDefaultGeData()
        {
            var geData = new GeData();
            var processName = Process.GetCurrentProcess().ProcessName;
            if (processName.StartsWith("devenv", StringComparison.OrdinalIgnoreCase) ||
                processName.EndsWith(".vshost", StringComparison.OrdinalIgnoreCase))
            {
                geData.Testing = true;
                geData.GenDataStore.SetBase(@"Data\ProgramDefinition.dcb");
                geData.GenDataStore.SetData(@"Data\GeneratorDefinitionModel.dcb");
                geData.Profile = new GenCompactProfileParser(geData.GenData, @"Data\GenProfileModel.prf", "");
            } 
            return geData;
        }
        
        public static GenDataEditorViewModel GenDataEditorViewModel
        {
            get { return _genDataEditorViewModel ?? (_genDataEditorViewModel = new GenDataEditorViewModel {Data = GeData}); }
        }
    }
}
