// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace org.xpangen.Generator.Data
{
    public class GenApplicationBase : BindableObject
    {
        private Dictionary<string, IList> _lists;
        private IGenObject _genObject;
        private NameList _classes;
        private IGenApplicationData _definition;
        private GenApplicationBase _parent;
        public string ClassName { get; set; }

        public bool Changed
        {
            get { return Definition.Changed; }
            private set { Definition.Changed = value; }
        }

        private IGenApplicationData Definition
        {
            get
            {
                if (_definition == null)
                {
                    _definition = GenApplicationData.CreateDefaultInstance();
                    if (Parent != null) _definition.Classes = Parent.Classes;
                }
                return _definition;
            }
            set
            {
                if (value.Classes == null && Parent != null) value.Classes = Parent.Classes;
                _definition = value;
            }
        }

        public IGenObject GenObject
        {
            get { return _genObject; }
            set
            {
                if (_genObject == value) return;
                var oldValue = _genObject;
                _genObject = value;

                if (value != null)
                {
                    CheckData();
                    ClassName = Definition.ClassName;
                    if (oldValue != null) oldValue.GenDataBase.PropertyChanged -= OnPropertyChanged;
                    value.GenDataBase.PropertyChanged += OnPropertyChanged;
                }

                GetFields();
                GenObjectSetNotification();
            }
        }

        private void CheckData()
        {
            var def = Definition;
            if (GenObject is GenObject)
                Definition = new GenApplicationGenObject((GenObject) GenObject);
            else Definition = new GenApplicationSubClass((GenSubClass) GenObject);
            if (def != null)
            {
                if (Definition.Properties.Count == 0)
                {
                    foreach (var property in def.Properties)
                        Definition.Properties.Add(property);
                }
                if (Definition.SubClasses.Count == 0)
                {
                    foreach (var subClass in def.SubClasses)
                        Definition.SubClasses.Add(subClass);
                }
            }
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            RaisePropertyChanged(e.PropertyName);
        }

        private void GetFields()
        {
            Definition.GetFields();
            Changed = false;
        }

        public NameList Classes
        {
            get { return _classes = _classes ?? new NameList(); }
            set { _classes = value; }
        }

        public NameList SubClasses
        {
            get { return Definition.SubClasses; }
        }

        protected NameList Properties
        {
            get { return Definition.Properties; }
        }

        public int ClassId
        {
            get { return Definition.ClassId; }
            set { Definition.ClassId = value; }
        }


        protected GenApplicationBase()
        {
            IgnorePropertyValidation = true;
            DelayedSave = false;
        }

        protected virtual void GenObjectSetNotification()
        {
            
        }

        public GenData GenData { get; set; }

        public bool DelayedSave { protected get; set; }
        
        public Dictionary<string, IList> Lists 
        {
            get { return _lists ?? (_lists = new Dictionary<string, IList>()); }
        }

        public GenApplicationBase Parent
        {
            get { return _parent; }
            set
            {
                if (Classes == null || Classes.Count == 0)
                    Classes = value.Classes;
                _parent = value;
            }
        }

        protected string AsString(string name)
        {
            return Definition.AsString(name);
        }

        protected void SetString(string name, string value)
        {
            Definition.SetString(name, value);
        }

        public void SaveFields()
        {
            if (GenObject == null) throw new GeneratorException("Attempting to save to a null generator object", GenErrorType.Assertion);
            var props = Properties;
            var className = ClassName;
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
