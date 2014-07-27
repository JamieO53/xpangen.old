using System;

namespace org.xpangen.Generator.Data
{
    public class GenApplicationData : GenBase, IGenApplicationData
    {
        public NameList Classes { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; protected set; }
        public NameList Properties { get; protected set; }
        public NameList SubClasses { get; protected set; }
        protected NameList Fields { get; private set; }
        protected TextList Values { get; private set; }
        public bool Changed { get; set; }

        protected GenApplicationData()
        {
            Fields = new NameList();
            Values = new TextList();
        }

        internal static GenApplicationData CreateDefaultInstance()
        {
            var data = new GenApplicationData {Properties = new NameList(), SubClasses = new NameList()};
            return data;
        }

        public virtual void GetFields()
        {
            throw new NotImplementedException();
        }

        public void SetString(string name, string value)
        {
            var i = Fields.IndexOf(name);
            if (i == -1)
            {
                Fields.Add(name);
                Values.Add(value);
                Changed = true;
            }
            else
            {
                for (var j = Values.Count; j <= i; j++)
                    Values.Add("");
                Changed |= Values[i] != value;
                Values[i] = value;
            }

        }

        public string AsString(string name)
        {
            var i = Fields.IndexOf(name);
            return i == -1 ? "" : Values[i];
        }

        protected void ClearFieldValues()
        {
            try
            {
                if (Properties != null)
                    foreach (var t in Properties)
                        SetString(t, "");
            }
            catch (Exception)
            {
                foreach (var field in Fields)
                    SetString(field, "");
            }
        }
    }
}