// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Globalization;

namespace org.xpangen.Generator.Data
{
    public class GenAttributes
    {
        private GenObject _genObject;
        private NameList Fields { get; set; }
        private TextList Values { get; set; }
        public bool Changed { get; private set; }
        public virtual GenObject GenObject
        {
            get { return _genObject; }
            set 
            {
                _genObject = value;
                
                if (value != null)
                {
                    ClassId = value.ClassId;
                    GenDataDef = value.GenDataBase.GenDataDef;
                }

                GetFields();
            }
        }

        public void GetFields()
        {
            if (GenObject != null)
            {
                var props = GenObject.Definition.Properties;
                for (var i = 0; i < props.Count; i++)
                    SetString(props[i], i < GenObject.Attributes.Count ? GenObject.Attributes[i] : "");
            }
            else
            {
                try
                {
                    var props = Definition.Properties;
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
            Changed = false;
        }

        public GenDataDefClass Definition
        {
            get { return GenDataDef.Classes[ClassId]; }
        }

        public GenDataDef GenDataDef { get; protected set; }

        public int ClassId { get; private set; }

        protected GenAttributes()
        {
            Fields = new NameList();
            Values = new TextList();
        }

        public GenAttributes(GenDataDef genDataDef) : this()
        {
            GenDataDef = genDataDef;
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

        public void SaveFields()
        {
            var props = GenObject.Definition.Properties;
            var className = GenObject.Definition.Name;
            var n = props.Count;
            var changed = false;
            var changedProps = new bool[n];
            for (var i = 0; i < n; i++)
            {
                var oldValue = GenObject.Attributes.Count <= i ? "" : GenObject.Attributes[i];
                var propertyName = props[i];
                var newValue = AsString(propertyName);
                changedProps[i] = oldValue != newValue;
                
                if (!changedProps[i]) continue;
                
                changed = true;
                if (i >= GenObject.Attributes.Count)
                    GenObject.Attributes.Add(newValue);
                else
                    GenObject.Attributes[i] = newValue;
            }

            if (!changed) return;
            
            GenObject.GenDataBase.Changed = true;
            for (var i = 0; i < n; i++)
                if (changedProps[i])
                    GenObject.GenDataBase.RaiseDataChanged(className, props[i]);
            Changed = false;
        }
    }
}
