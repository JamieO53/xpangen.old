// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Data.Model.Settings
{
    /// <summary>
    /// Container for generator settings
    /// </summary>
    public class GenSettings : GenNamedApplicationBase
    {
        public GenSettings()
        {
        }

        public GenSettings(GenData genData)
        {
            GenData = genData;
            Properties.Add("HomeDir");
        }

        /// <summary>
        /// The default location of the base files
        /// </summary>
        public string HomeDir
        {
            get { return AsString("HomeDir"); }
            set
            {
                if (HomeDir == value) return;
                SetString("HomeDir", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<FileGroup> FileGroupList { get; private set; }
        public GenNamedApplicationList<BaseFile> BaseFileList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            FileGroupList = new GenNamedApplicationList<FileGroup>(this);
            base.GenObjectSetNotification();
            BaseFileList = new GenNamedApplicationList<BaseFile>(this);
        }

        public FileGroup AddFileGroup(string name, string fileName = "", string filePath = "", string baseFileName = "", string profile = "", string generatedFile = "")
        {
            var item = new FileGroup(GenData)
                           {
                               GenObject = GenData.CreateObject("GenSettings", "FileGroup"),
                               Name = name,
                               FileName = fileName,
                               FilePath = filePath,
                               BaseFileName = baseFileName,
                               Profile = profile,
                               GeneratedFile = generatedFile
                           };
            FileGroupList.Add(item);
            return item;
        }


        public BaseFile AddBaseFile(string name, string fileName = "", string filePath = "", string title = "", string fileExtension = "")
        {
            var item = new BaseFile(GenData)
                           {
                               GenObject = GenData.CreateObject("GenSettings", "BaseFile"),
                               Name = name,
                               FileName = fileName,
                               FilePath = filePath,
                               Title = title,
                               FileExtension = fileExtension
                           };
            BaseFileList.Add(item);
            return item;
        }

    }
}
