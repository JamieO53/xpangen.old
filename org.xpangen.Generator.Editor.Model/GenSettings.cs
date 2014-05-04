// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace org.xpangen.Generator.Editor.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class GenSettings : GenNamedApplicationBase
    {
        public GenSettings()
        {
        }

        public GenSettings(GenData genData) : this()
        {
			GenData = genData;
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
            FileGroupList = new GenNamedApplicationList<FileGroup>(this);
            BaseFileList = new GenNamedApplicationList<BaseFile>(this);
        }

        public FileGroup AddFileGroup(string Name, string FileName, string FilePath, string BaseFileName, string Generated, string Profile)
        {
            var item = new FileGroup(GenData)
                           {
                               GenObject = GenData.CreateObject("GenSettings", "FileGroup"),
                               Name = Name,
                               FileName = FileName,
                               FilePath = FilePath,
                               BaseFileName = BaseFileName,
                               Generated = Generated,
                               Profile = Profile
                           };
            FileGroupList.Add(item);
            return item;
        }


        public BaseFile AddBaseFile(string Name, string FileName, string FilePath, string Title, string FileExtension)
        {
            var item = new BaseFile(GenData)
                           {
                               GenObject = GenData.CreateObject("GenSettings", "BaseFile"),
                               Name = Name,
                               FileName = FileName,
                               FilePath = FilePath,
                               Title = Title,
                               FileExtension = FileExtension
                           };
            BaseFileList.Add(item);
            return item;
        }
    }
}
