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
    public class FileGroup : GenNamedApplicationBase
    {
        public FileGroup(GenData genData) : base(genData)
        {
        }

        /// <summary>
        /// The file group name
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
        /// The name of the file being edited
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
        /// The full path of the file being edited
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
        /// The name of the file's definitions data file
        /// </summary>
        public string BaseFileName
        {
            get { return AsString("BaseFileName"); }
            set
            {
                if (BaseFileName == value) return;
                SetString("BaseFileName", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The file path of the generated file
        /// </summary>
        public string Generated
        {
            get { return AsString("Generated"); }
            set
            {
                if (Generated == value) return;
                SetString("Generated", value);
                if (!DelayedSave) SaveFields();
            }
        }

        /// <summary>
        /// The file path of the profile used to generate the file's output
        /// </summary>
        public string Profile
        {
            get { return AsString("Profile"); }
            set
            {
                if (Profile == value) return;
                SetString("Profile", value);
                if (!DelayedSave) SaveFields();
            }
        }

        public bool Changed { get; set; }
    }
}
