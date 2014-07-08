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
            base.GenObject = genData.Root;
		}

        public static GenDataDef GetDefinition()
        {
            var f = new GenDataDef();
            f.Definition = "GeneratorEditor";
            f.AddSubClass("", "GenSettings");
            f.AddSubClass("GenSettings", "FileGroup");
            f.AddSubClass("GenSettings", "BaseFile");
            f.AddSubClass("BaseFile", "Profile");
            f.Classes[1].InstanceProperties.Add("HomeDir");
            f.Classes[2].InstanceProperties.Add("Name");
            f.Classes[2].InstanceProperties.Add("FileName");
            f.Classes[2].InstanceProperties.Add("FilePath");
            f.Classes[2].InstanceProperties.Add("BaseFileName");
            f.Classes[2].InstanceProperties.Add("Profile");
            f.Classes[2].InstanceProperties.Add("GeneratedFile");
            f.Classes[3].InstanceProperties.Add("Name");
            f.Classes[3].InstanceProperties.Add("FileName");
            f.Classes[3].InstanceProperties.Add("FilePath");
            f.Classes[3].InstanceProperties.Add("Title");
            f.Classes[3].InstanceProperties.Add("FileExtension");
            f.Classes[4].InstanceProperties.Add("Name");
            f.Classes[4].InstanceProperties.Add("FileName");
            f.Classes[4].InstanceProperties.Add("FilePath");
            f.Classes[4].InstanceProperties.Add("Title");
            return f;
        }

        public GenNamedApplicationList<GenSettings> GenSettingsList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            GenSettingsList = new GenNamedApplicationList<GenSettings>(this);
        }

        public GenSettings AddGenSettings(string homeDir = "")
        {
            var item = new GenSettings(GenData)
                           {
                               GenObject = GenData.CreateObject("", "GenSettings"),
                               HomeDir = homeDir
                           };
            GenSettingsList.Add(item);
            return item;
        }
    }
}
