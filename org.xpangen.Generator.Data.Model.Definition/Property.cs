using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Definition
{
    /// <summary>
    /// Property definition
    /// </summary>
    public class Property : GenApplicationBase
    {
        public Property(GenDataDef genDataDef) : base(genDataDef)
        {
        }

        /// <summary>
        /// Property name: must be well formed
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
        /// Property description: used as a hint when editing
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
        /// Data type of property (used for editing) - String, integer or boolean
        /// </summary>
        public string DataType
        {
            get { return AsString("DataType"); }
            set
            {
                if (DataType == value) return;
                SetString("DataType", value);
                SaveFields();
            }
        }

        /// <summary>
        /// Default value of the property when a new object is created (used for editing)
        /// </summary>
        public string Default
        {
            get { return AsString("Default"); }
            set
            {
                if (Default == value) return;
                SetString("Default", value);
                SaveFields();
            }
        }

    }
}
