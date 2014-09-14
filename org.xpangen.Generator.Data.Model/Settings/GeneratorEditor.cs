// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Settings
{
    /// <summary>
    /// Classes allowing access to generator data for the editor
    /// </summary>
    public class GeneratorEditor : GenApplicationBase
    {
        public GeneratorEditor(): this(new GenData(GetDefinition()))
        {
        }

        public GeneratorEditor(GenData genData)
        {
            GenData = genData;
            Classes.Add("GenSettings");
            Classes.Add("FileGroup");
            Classes.Add("BaseFile");
            Classes.Add("Profile");
            SubClasses.Add("GenSettings");
            base.GenObject = genData.Root;
        }

        public static GenDataDef GetDefinition()
        {
            var f = new GenDataDef();
            f.DefinitionName = "GeneratorEditor";
            f.AddSubClass("", "GenSettings");
            f.AddSubClass("GenSettings", "FileGroup");
            f.AddSubClass("GenSettings", "BaseFile");
            f.AddSubClass("BaseFile", "Profile");
            f.Classes[1].AddInstanceProperty("HomeDir");
            f.Classes[2].AddInstanceProperty("Name");
            f.Classes[2].AddInstanceProperty("FileName");
            f.Classes[2].AddInstanceProperty("FilePath");
            f.Classes[2].AddInstanceProperty("BaseFileName");
            f.Classes[2].AddInstanceProperty("Profile");
            f.Classes[2].AddInstanceProperty("GeneratedFile");
            f.Classes[3].AddInstanceProperty("Name");
            f.Classes[3].AddInstanceProperty("FileName");
            f.Classes[3].AddInstanceProperty("FilePath");
            f.Classes[3].AddInstanceProperty("Title");
            f.Classes[3].AddInstanceProperty("FileExtension");
            f.Classes[4].AddInstanceProperty("Name");
            f.Classes[4].AddInstanceProperty("FileName");
            f.Classes[4].AddInstanceProperty("FilePath");
            f.Classes[4].AddInstanceProperty("Title");
            return f;
        }

        public GenNamedApplicationList<GenSettings> GenSettingsList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            GenSettingsList = new GenNamedApplicationList<GenSettings>(this, 1, 0);
        }

        public GenSettings AddGenSettings(string homeDir = "")
        {
            var item = new GenSettings(GenData)
                           {
                               GenObject = GenData.Root.CreateGenObject("GenSettings"),
                               HomeDir = homeDir
                           };
            GenSettingsList.Add(item);
            return item;
        }
    }
}
