// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System.Collections.ObjectModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Definition;

namespace GenEdit.ViewModel
{
    public class GenObjectViewModel: GenDataViewModelBase
    {
        private ObservableCollection<FieldViewModelBase> _fields;

        /// <summary>
        /// The list of fields belonging to the <see cref="GenObject"/> being edited.
        /// </summary>
        public override ObservableCollection<FieldViewModelBase> Fields
        {
            get
            {
                if (_fields == null)
                {
                    _fields = new ObservableCollection<FieldViewModelBase>();
                    var n = GenAttributes.Properties.Count;
                    var def = Definition as Class;

                    for (var i = 0; i < n; i++)
                    {
                        GenObjectFieldViewModel item;
                        if (def != null)
                            item = new GenObjectFieldViewModel(GenAttributes, i, def.PropertyList[i], IsNew,
                                                               IsReadOnly);
                        else
                            item = new GenObjectFieldViewModel(GenAttributes, i, null, IsNew, true);

                        item.PropertyChanged += FieldPropertyChanged;
                        _fields.Add(item);
                    }
                }
                return _fields;
            }
        }

        public GenAttributes GenAttributes { get; private set; }

        public override string Name { get { return GenAttributes.AsString("Name"); } }
        
        /// <summary>
        /// Saves the object data
        /// </summary>
        public override void Save()
        {
            GenAttributes.SaveFields();
            base.Save();
        }

        /// <summary>
        /// Cancels changes to the object data
        /// </summary>
        public override void Cancel()
        {
            GenAttributes.GetFields();
            base.Cancel();
        }

        /// <summary>
        /// Initializes a new instance of the GenObjectViewModel class.
        /// </summary>
        /// <param name="genObject">The <see cref="GenObject"/> being edited.</param>
        /// <param name="definition">The class definition of the object being edited.</param>
        /// <param name="isReadOnly">Is this data readonly?</param>
        public GenObjectViewModel(GenObject genObject, GenNamedApplicationBase definition,
                                  bool isReadOnly)
        {
            IsReadOnly = isReadOnly;
            IgnorePropertyValidation = true;
            Definition = definition;
            GenAttributes = new GenAttributes(genObject.GenDataBase.GenDataDef, genObject.ClassId) {GenObject = genObject};
            GenObject = genObject;
            Changed = false;
            IsNew = false;
        }
    }

    public class ObservableCollection<T>: Collection<T>
    {
    }
}