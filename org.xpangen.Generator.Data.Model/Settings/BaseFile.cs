// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

namespace org.xpangen.Generator.Data.Model.Settings
{
    /// <summary>
    /// The base files that can be used for editing generator data
    /// </summary>
    public class BaseFile : GenNamedApplicationBase
    {
        public BaseFile()
        {
            SubClasses.Add("Profile");
            Properties.Add("Name");
            Properties.Add("FileName");
            Properties.Add("FilePath");
            Properties.Add("Title");
            Properties.Add("FileExtension");
        }

        public BaseFile(GenData genData) : this()
        {
            GenData = genData;
        }

        /// <summary>
        /// The file name of the base file
        /// </summary>
        public override string Name
        {
            get { return AsString("Name"); }
            set
            {
                if (Name == value) return;
                SetString("Name", value);
                if (!DelayedSave) SaveFields();
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
                if (!DelayedSave) SaveFields();
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
                if (!DelayedSave) SaveFields();
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
                if (!DelayedSave) SaveFields();
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
                if (!DelayedSave) SaveFields();
            }
        }

        public GenNamedApplicationList<Profile> ProfileList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            base.GenObjectSetNotification();
            ProfileList = new GenNamedApplicationList<Profile>(this, 4, 0);
        }

        public Profile AddProfile(string name, string fileName = "", string filePath = "", string title = "")
        {
            var item = new Profile(GenData)
                           {
                               GenObject = ((GenObject)GenObject).CreateGenObject("Profile"),
                               Name = name,
                               FileName = fileName,
                               FilePath = filePath,
                               Title = title
                           };
            ProfileList.Add(item);
            return item;
        }

    }
}
