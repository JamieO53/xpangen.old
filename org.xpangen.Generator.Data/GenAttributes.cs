// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Globalization;

namespace org.xpangen.Generator.Data
{
    public class GenAttributes : BindableObject
    {
        private IGenObject _genObject;
        private NameList _classes;
        private NameList Fields { get; set; }
        private TextList Values { get; set; }
        public bool Changed { get; private set; }
        public virtual IGenObject GenObject
        {
            get { return _genObject; }
            set 
            {
                _genObject = value;
                
                if (value != null)
                {
                    if (Properties.Count == 0)
                    {
                        ClassId = GenObject.ClassId;
                        if (GenObject.GenDataBase.GenDataDef != null)
                        {
                            var classDef = GenObject.GenDataBase.GenDataDef.GetClassDef(ClassId);
                            foreach (var property in classDef.Properties)
                                Properties.Add(property);
                            foreach (var subClass in classDef.SubClasses)
                                SubClasses.Add((subClass.SubClass.Name));
                        }
                    }
                    value.GenDataBase.PropertyChanged += OnPropertyChanged;
                }

                GetFields();
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        public void GetFields()
        {
            if (GenObject != null)
            {
                var props = Properties;
                Fields.Clear();
                Values.Clear();
                for (var i = 0; i < props.Count; i++)
                    SetString(props[i], i < GenObject.Attributes.Count ? GenObject.Attributes[i] : "");
            }
            else
            {
                try
                {
                    var props = Properties;
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

        public NameList Classes
        {
            get { return _classes ?? new NameList(); }
            set { _classes = value; }
        }

        protected NameList SubClasses { get; private set; }
        public NameList Properties { get; private set; }
        public int ClassId { get; private set; }

        protected GenAttributes()
        {
            IgnorePropertyValidation = true;
            Fields = new NameList();
            Values = new TextList();
            Properties = new NameList();
            SubClasses = new NameList();
        }

        public GenAttributes(GenDataDef genDataDef, int classId) : this()
        {
            ClassId = classId;
            foreach (var c in genDataDef.Classes)
                Classes.Add(c.Name);
            foreach (var property in genDataDef.GetClassProperties(ClassId))
                Properties.Add(property);
            foreach (var sc in genDataDef.GetClassSubClasses(ClassId))
                SubClasses.Add(sc.SubClass.Name);
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
            Contract.Assert(GenObject != null, "Attempting to save to a null generator object");
            var props = GenObject.Properties;
            var className = GenObject.ClassName;
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
