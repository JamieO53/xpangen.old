using System;
using System.Globalization;

namespace org.xpangen.Generator.Data
{
    public class GenAttributes
    {
        private GenObject _genObject;
        private NameList Fields { get; set; }
        private TextList Values { get; set; }
        public virtual GenObject GenObject
        {
            get { return _genObject; }
            set 
            {
                _genObject = value;
                
                if (value != null)
                {
                    ClassId = value.ClassId;
                    GenDataDef = value.GenData.GenDataDef;
                }

                GetFields();
            }
        }

        private void GetFields()
        {
            if (GenObject != null)
            {
                var classId = GenObject.ClassId;
                var props = GenObject.GenData.GenDataDef.Properties[classId];
                for (var i = 0; i < props.Count; i++)
                    SetString(props[i], i < GenObject.Attributes.Count ? GenObject.Attributes[i] : "");
            }
            else
            {
                var classId = ClassId;
                try
                {
                    var props = GenDataDef.Properties[classId];
                    if (props != null)
                        foreach (var t in props)
                            SetString(t, "");
                }
                catch (Exception)
                {
                    foreach (var field in Fields)
                        SetString(field, "");
                }
            }
        }

        protected GenDataDef GenDataDef { get; private set; }

        protected int ClassId { get; private set; }

        public GenAttributes(GenDataDef genDataDef)
        {
            GenDataDef = genDataDef;
            Fields = new NameList();
            Values = new TextList();
        }

        public string AsString(string name)
        {
            var i = Fields.IndexOf(name);
            return i == -1 ? "" : Values[i];
        }

        public void SetNumber(string name, int value)
        {
            SetString(name, value.ToString(CultureInfo.InvariantCulture));
        }

        public void SetString(string name, string value)
        {
            var i = Fields.IndexOf(name);
            if (i == -1)
            {
                Fields.Add(name);
                Values.Add(value);
            }
            else
            {
                for (var j = Values.Count; j <= i; j++)
                    Values.Add("");
                Values[i] = value;
            }
        }

        public void SaveFields()
        {
            var classId = GenObject.ClassId;
            var props = GenObject.GenData.GenDataDef.Properties[classId];
            var className = GenObject.GenData.GenDataDef.Classes[classId];
            var n = props.Count;
            var changed = false;
            for (var i = 0; i < n; i++)
            {
                var oldValue = GenObject.Attributes.Count <= i ? "" : GenObject.Attributes[i];
                var propertyName = props[i];
                var newValue = AsString(propertyName);
                if (oldValue != newValue)
                {
                    changed = true;
                    GenObject.GenData.RaiseDataChanged(className, propertyName);
                }
            }

            if (changed)
            {
                GenObject.Attributes.Clear();
                for (var i = 0; i < n; i++)
                    GenObject.Attributes.Add(AsString(props[i]));
                GenObject.GenData.Changed = true;
            }
        }
    }
}
