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
        public GenSettings(GenData genData)
            : base(genData)
        {
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
            FileGroupList = new GenNamedApplicationList<FileGroup>();
            var classId = GenDataDef.Classes.IndexOf("FileGroup");
            var classIdx = GenDataDef.IndexOfSubClass(ClassId, classId);
            if (classIdx != -1)
            {
                var list = new GenObjectList(GenObject.SubClass[classIdx], GenObject.GenDataBase,
                                             GenData.Context[ClassId]);
                list.First();
                while (!list.Eol)
                {
                    FileGroupList.Add(new FileGroup(GenData) { GenObject = list.GenObject });
                    list.Next();
                }
            }
            BaseFileList = new GenNamedApplicationList<BaseFile>();
            classId = GenDataDef.Classes.IndexOf("BaseFile");
            classIdx = GenDataDef.IndexOfSubClass(ClassId, classId);
            if (classIdx != -1)
            {
                var list = new GenObjectList(GenObject.SubClass[classIdx], GenObject.GenDataBase,
                                             GenData.Context[ClassId]);
                list.First();
                while (!list.Eol)
                {
                    BaseFileList.Add(new BaseFile(GenData) { GenObject = list.GenObject });
                    list.Next();
                }
            }
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
