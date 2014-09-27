// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Windows.Forms;
using org.xpangen.Generator.Data;

namespace GenEdit.ViewModel
{
    public class SubClassFieldViewModel : FieldViewModelBase
    {
        /// <summary>
        /// The Definition of the subclass.
        /// </summary>
        public GenDataDefSubClass GenDataDefSubClass { get; private set; }
        /// <summary>
        /// The field being updated.
        /// </summary>
        public SubClassField SubClassField { get; set; }
        /// <summary>
        /// The subclass being updated
        /// </summary>
        public ISubClassBase SubClass { get; private set; }
        /// <summary>
        /// The value of the field
        /// </summary>

        public ISubClassBase Parent { get; private set; }
        public override string Value
        {
            get{
                switch (SubClassField)
                {
                    case SubClassField.Name:
                        return GenDataDefSubClass == null ? "" : GenDataDefSubClass.SubClass.Name;
                    case SubClassField.Reference:
                        return Parent != null ? Parent.Reference : "";
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            set
            {
                switch (SubClassField)
                {
                    case SubClassField.Name:
                        GenDataDefSubClass.SubClass.Name = value;
                        break;
                    case SubClassField.Reference:
                        if (Parent != null)
                        {
                            if (!ViewModelLocator.GenDataEditorViewModel.Data.CheckIfDataExists(value))
                                MessageBox.Show("The reference file does not exist: " + value, "Reference Data");
                            else
                                Parent.Reference = value;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public SubClassFieldViewModel(ISubClassBase parent, GenDataDefSubClass genDataDefSubClass, ISubClassBase subClass,
                                      SubClassField subClassField, bool isReadOnly)
        {
            IgnorePropertyValidation = true;
            Parent = parent;
            GenDataDefSubClass = genDataDefSubClass;
            SubClassField = subClassField;
            SubClass = subClass;
            switch (subClassField)
            {
                case SubClassField.Name:
                    Name = "Name";
                    Hint = genDataDefSubClass == null || genDataDefSubClass.Reference == ""
                               ? "The name of the subclass"
                               : "The name of the top level class in the referenced file";
                    DataType = "Identifier";
                    IsReadOnly = true;
                    break;
                case SubClassField.Reference:
                    Name = "Reference";
                    Hint = genDataDefSubClass == null || genDataDefSubClass.Reference == ""
                               ? "Must be blank"
                               : "The name of the referenced file";
                    DataType = "String";
                    IsReadOnly = isReadOnly || genDataDefSubClass == null || genDataDefSubClass.Reference == "";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("subClassField");
            }
        }
    }

    public enum SubClassField
    {
        Name,
        Reference
    }
}
