using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace Generator.Editor.Model
{
    /// <summary>
    /// Profile compatible with a Base File
    /// </summary>
    public class Profile : GenApplicationBase
    {
        public Profile(GenDataDef genDataDef) : base(genDataDef)
        {
        }

        /// <summary>
        /// The profile name
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
        /// Profile file name
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
        /// Full path of profile
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
        /// Description of profile
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

    }
}
