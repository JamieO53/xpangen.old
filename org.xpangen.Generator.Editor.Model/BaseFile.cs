// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace Generator.Editor.Model
{
    /// <summary>
    /// The base files that can be used for editing generator data
    /// </summary>
    public class BaseFile : GenApplicationBase
    {
        public BaseFile(GenDataDef genDataDef) : base(genDataDef)
        {
        }

        /// <summary>
        /// The file name of the base file
        /// </summary>
        public string Name
        {
            get { return AsString("Name"); }
            set
            {
                if (Name == value) return;
                SetString("Name", value);
                SaveFields();
            }
        }

        /// <summary>
        /// The base file name
        /// </summary>
        public string FileName
        {
            get { return AsString("FileName"); }
            set
            {
                if (FileName == value) return;
                SetString("FileName", value);
                SaveFields();
            }
        }

        /// <summary>
        /// The full path of the base file
        /// </summary>
        public string FilePath
        {
            get { return AsString("FilePath"); }
            set
            {
                if (FilePath == value) return;
                SetString("FilePath", value);
                SaveFields();
            }
        }

        /// <summary>
        /// Description of the files created using this base file
        /// </summary>
        public string Title
        {
            get { return AsString("Title"); }
            set
            {
                if (Title == value) return;
                SetString("Title", value);
                SaveFields();
            }
        }

        /// <summary>
        /// The default extension of files using this base file
        /// </summary>
        public string FileExtension
        {
            get { return AsString("FileExtension"); }
            set
            {
                if (FileExtension == value) return;
                SetString("FileExtension", value);
                SaveFields();
            }
        }

        public GenApplicationList<Profile> ProfileList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            ProfileList = new GenApplicationList<Profile>();
            var list = new GenObjectList(GenObject.SubClass[0]);
            list.First();
            while (!list.Eol)
            {
                ProfileList.Add(new Profile(GenDataDef) {GenObject = list.GenObject});
                list.Next();
            }

        }
    }
}
