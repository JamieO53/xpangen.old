// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.ViewModel
{
    public class GenObjectFieldViewModel: FieldViewModelBase
    {
        private GenAttributes GenAttributes { get; set; }

        /// <summary>
        /// The value of the field
        /// </summary>
        public override string Value
        {
            get { return GenAttributes.AsString(_name); }
            set
            {
                if (value == Value) return;
                GenAttributes.SetString(_name, value);
                RaisePropertyChanged("Value");
            }
        }

        /// <summary>
        /// Initializes a new instance of the GenObjectFieldViewModel class.
        /// </summary>
        /// <param name="genAttributes">The Generator Attributes object for the object owning this value.</param>
        /// <param name="propertyId">The ID of the property being edited.</param>
        /// <param name="property">The property definition of the field being edited.</param>
        /// <param name="isNew">Is this field in a new object?</param>
        /// <param name="isReadOnly">Is this field readonly?</param>
        public GenObjectFieldViewModel(GenAttributes genAttributes, int propertyId, Property property, bool isNew, bool isReadOnly)
        {
            IgnorePropertyValidation = true;
            GenAttributes = genAttributes;
            _name = GenAttributes.Definition.Properties[propertyId];
            Name = _name;
            if (property != null)
            {
                Name = property.Name;
                Hint = property.Title;
                DataType = property.DataType;
                Default = property.Default;
                ComboValues = SetComboValues();
                IsReadOnly = isReadOnly;
                if (isNew)
                    GenAttributes.SetString(_name, Default); // Don't raise the Property Raised event
            }
            else
            {
                Name = _name;
                Hint = "";
                DataType = "String";
                Default = "";
                ComboValues = null;
                IsReadOnly = true;
                if (isNew)
                    GenAttributes.SetString(_name, Default); // Don't raise the Property Raised event
            }
        }

        private List<GeComboItem> SetComboValues()
        {
            return DataType.Equals("boolean", StringComparison.InvariantCultureIgnoreCase)
                       ? _genDataEditorViewModel.Data.GetCodesCombo("YesNo")
                       : (Name.Equals("datatype", StringComparison.InvariantCultureIgnoreCase)
                              ? _genDataEditorViewModel.Data.GetCodesCombo("DataType")
                              : null);
        }
    }
}