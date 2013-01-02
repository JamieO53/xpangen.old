using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Minimal
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

    }
}
