// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.ObjectModel;
using System.ComponentModel;
using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;

namespace GenEdit.ViewModel
{
    public class GenObjectViewModel: BindableObject
    {
        private ObservableCollection<GenObjectFieldViewModel> _fields;
        /// <summary>
        /// The list of fields belonging to the <see cref="GenObject"/> being edited.
        /// </summary>
        public ObservableCollection<GenObjectFieldViewModel> Fields
        {
            get
            {
                var classId = GenAttributes.GenObject.ClassId;
                if (_fields == null)
                {
                    _fields = new ObservableCollection<GenObjectFieldViewModel>();
                    var n = GenAttributes.GenDataDef.Classes[classId].Properties.Count;
                    var @class = Definition as Class;
                    var subClass = Definition as SubClass;
                    var property = Definition as Property;
                    for (var i = 0; i < n; i++)
                    {
                        GenObjectFieldViewModel item = null;
                        if (@class != null)
                            item = new GenObjectFieldViewModel(GenAttributes, i, @class.PropertyList[i]);
                        else if (subClass != null)
                            item = new GenObjectFieldViewModel(GenAttributes, i, null);
                        else if (property != null)
                            item = new GenObjectFieldViewModel(GenAttributes, i, property);

                        if (item == null) continue;
                        item.PropertyChanged += FieldPropertyChanged; 
                        _fields.Add(item);
                    }
                }
                return _fields;
            }
        }

        /// <summary>
        /// Initializes a new instance of the GenObjectViewModel class.
        /// </summary>
        /// <param name="genObject">The <see cref="GenObject"/> being edited.</param>
        /// <param name="definition">The class definition of the object being edited.</param>
        /// <param name="savedContext">The context of the object being edited</param>
        public GenObjectViewModel(GenObject genObject, GenApplicationBase definition, GenSavedContext savedContext)
        {
            SavedContext = savedContext;
            IgnorePropertyValidation = true;
            Definition = definition;
            GenAttributes = new GenAttributes(genObject.GenDataBase.GenDataDef) { GenObject = savedContext.GenObject };
            Changed = false;
        }

        private GenApplicationBase Definition { get; set; }

        public GenAttributes GenAttributes { get; private set; }

        /// <summary>
        /// One or more field values have changed
        /// </summary>
        public bool Changed { get; set; }
        
        /// <summary>
        /// The heading value of the tree node
        /// </summary>
        public string Name { get { return GenAttributes.AsString("Name"); } }

        /// <summary>
        /// A hint describing the use of the node data
        /// </summary>
        public string Hint { get { return GenAttributes.AsString("Title"); } }

        public GenSavedContext SavedContext { get; private set; }

        /// <summary>
        /// Saves the object data
        /// </summary>
        public void Save()
        {
            GenAttributes.SaveFields();
            Changed = false;
        }

        /// <summary>
        /// Cancels changes to the object data
        /// </summary>
        public void Cancel()
        {
            GenAttributes.GetFields();
            Changed = false;
        }
        
        /// <summary>
        /// Establish the context of the current object.
        /// </summary>
        public void EstablishContext()
        {
            SavedContext.EstablishContext();
        }

        private void FieldPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Changed = true;
            RaisePropertyChanged(e.PropertyName);
        }

    }

    public class ObservableCollection<T>: Collection<T>
    {
    }
}