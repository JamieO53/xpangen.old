using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace Generator.Editor.Model
{
    /// <summary>
    /// Related group of files for editing a file
    /// </summary>
    public class FileGroup : GenApplicationBase
    {
        public FileGroup(GenDataDef genDataDef) : base(genDataDef)
        {
        }

        /// <summary>
        /// The file group name
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
        /// The name of the file being edited
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
        /// The full path of the file being edited
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
        /// The name of the file's definitions data file
        /// </summary>
        public string BaseFileName
        {
            get { return AsString("BaseFileName"); }
            set
            {
                if (BaseFileName == value) return;
                SetString("BaseFileName", value);
                SaveFields();
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
                SaveFields();
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
                SaveFields();
            }
        }

    }
}
