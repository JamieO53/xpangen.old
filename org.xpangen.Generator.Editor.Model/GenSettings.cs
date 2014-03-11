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
        public GenSettings(GenData genData) : base(genData)
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
                var list = new GenObjectList(GenObject.SubClass[classIdx], GenObject.GenDataBase);
                list.First();
                while (!list.Eol)
                {
                    FileGroupList.Add(new FileGroup(GenData) {GenObject = list.GenObject});
                    list.Next();
                }
            }

            BaseFileList = new GenNamedApplicationList<BaseFile>();
            classId = GenDataDef.Classes.IndexOf("BaseFile");
            classIdx = GenDataDef.IndexOfSubClass(ClassId, classId);
            if (classIdx != -1)
            {
                var list = new GenObjectList(GenObject.SubClass[classIdx], GenObject.GenDataBase);
                list.First();
                while (!list.Eol)
                {
                    BaseFileList.Add(new BaseFile(GenData) {GenObject = list.GenObject});
                    list.Next();
                }
            }

        }
    }
}
