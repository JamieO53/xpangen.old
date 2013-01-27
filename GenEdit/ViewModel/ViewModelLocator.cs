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
            if (Process.GetCurrentProcess().ProcessName.StartsWith("devenv", StringComparison.Ordinal))
            {
                geData.Testing = true;
                geData.GenDataStore.SetBase(@"Data\ProgramDefinition.dcb");
                geData.GenDataStore.SetData(@"Data\GeneratorDefinitionModel.dcb");
                geData.Profile = new GenCompactProfileParser(GeData.GenData, @"Data\GenProfileModel.prf", "");
            } 
            return geData;
        }
        
        public static GenDataEditorViewModel GenDataEditorViewModel
        {
            get { return _genDataEditorViewModel ?? (_genDataEditorViewModel = new GenDataEditorViewModel {Data = GeData}); }
        }
    }
}
