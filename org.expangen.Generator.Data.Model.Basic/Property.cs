using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Basic
{
    /// <summary>
    /// 
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

    }
}
