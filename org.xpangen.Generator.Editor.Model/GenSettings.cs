// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace Generator.Editor.Model
{
    /// <summary>
    /// Container for generator settings
    /// </summary>
    public class GenSettings : GenApplicationBase
    {
        public GenSettings(GenDataDef genDataDef) : base(genDataDef)
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
                SaveFields();
            }
        }

        public GenApplicationList<FileGroup> FileGroupList { get; private set; }
        public GenApplicationList<BaseFile> BaseFileList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            FileGroupList = new GenApplicationList<FileGroup>();
            var list = new GenObjectList(GenObject.SubClass[0]);
            list.First();
            while (!list.Eol)
            {
                FileGroupList.Add(new FileGroup(GenDataDef) {GenObject = list.GenObject});
                list.Next();
            }

            BaseFileList = new GenApplicationList<BaseFile>();
            list.GenObjectListBase = GenObject.SubClass[0];

            list.First();
            while (!list.Eol)
            {
                BaseFileList.Add(new BaseFile(GenDataDef) {GenObject = list.GenObject});
                list.Next();
            }

        }
    }
}
