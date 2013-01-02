using org.xpangen.Generator.Application;

namespace org.xpangen.Generator.Data.Model.Basic
{
    /// <summary>
    /// Class Definition
    /// </summary>
    public class Class : GenApplicationBase
    {
        public Class(GenDataDef genDataDef) : base(genDataDef)
        {
        }

        /// <summary>
        /// Object name: must be well formed
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
        /// Object description: used as a hint when editing
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

        public GenApplicationList<SubClass> SubClassList { get; private set; }
        public GenApplicationList<Property> PropertyList { get; private set; }

        protected override void GenObjectSetNotification()
        {
            SubClassList = new GenApplicationList<SubClass>();
            var list = GenObject.SubClass[0];
            list.First();
            while (!list.Eol)
            {
                SubClassList.Add(new SubClass(GenDataDef) {GenObject = list.Context});
                list.Next();
            }

            PropertyList = new GenApplicationList<Property>();
            list = GenObject.SubClass[0];
            list.First();
            while (!list.Eol)
            {
                PropertyList.Add(new Property(GenDataDef) {GenObject = list.Context});
                list.Next();
            }

        }
    }
}
