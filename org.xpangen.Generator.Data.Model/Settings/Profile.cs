// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Settings
{
    /// <summary>
    /// Profile compatible with a Base File
    /// </summary>
    public class Profile : GenNamedApplicationBase
    {
        public Profile()
        {
        }

        public Profile(GenData genData)
        {
			GenData = genData;
        }

        /// <summary>
        /// The profile name
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
        /// Profile file name
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
        /// Full path of profile
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
        /// Description of profile
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


    }
}
