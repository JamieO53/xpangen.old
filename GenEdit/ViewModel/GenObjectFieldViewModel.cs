// This Source Code Form is subject to the terms of the Mozilla Public
// License, v. 2.0. If a copy of the MPL was not distributed with this
//  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.Collections.Generic;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Definition;

namespace GenEdit.ViewModel
{
    public class GenObjectFieldViewModel: BindableObject
    {
        private readonly string _name;
        private GenAttributes GenAttributes { get; set; }
        private int PropertyId { get; set; }
        private Property Property { get; set; }

        /// <summary>
        /// The name of the field being edited.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// A hint describing the purpose of the field.
        /// </summary>
        public string Hint { get; private set; }

        /// <summary>
        /// The type of data being edited.
        /// </summary>
        public string DataType { get; private set; }

        /// <summary>
        /// The default value for this field when a new object is created.
        /// </summary>
        public string Default { get; private set; }

        /// <summary>
        /// An optional list of values
        /// </summary>
        public List<GenComboItem> ComboValues { get; private set; }
        
        /// <summary>
        /// The value of the field
        /// </summary>
        public string Value
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
        public GenObjectFieldViewModel(GenAttributes genAttributes, int propertyId, Property property, bool isNew)
        {
            IgnorePropertyValidation = true;
            GenAttributes = genAttributes;
            PropertyId = propertyId;
            Property = property;
            _name = GenAttributes.Definition.Properties[propertyId];
            Name = _name;
            if (property == null) return;
            
            Name = property.Name;
            Hint = property.Title;
            DataType = property.DataType;
            Default = property.Default;
            ComboValues = SetComboValues();
            if (isNew)
                GenAttributes.SetString(_name, Default); // Don't raise the Property Raised event
        }

        private List<GenComboItem> SetComboValues()
        {
            List<GenComboItem> combo;
            if (DataType.Equals("boolean", StringComparison.InvariantCultureIgnoreCase))
            {
                combo = new List<GenComboItem>
                            {
                                new GenComboItem("Yes", "True"),
                                new GenComboItem("No", "")
                            };
            }
            else if (Name.Equals("datatype", StringComparison.InvariantCultureIgnoreCase))
            {
                combo = new List<GenComboItem>
                            {
                                new GenComboItem("String", "String"),
                                new GenComboItem("Integer", "Integer"),
                                new GenComboItem("Boolean", "Boolean"),
                                new GenComboItem("Identifier", "Identifier")
                            };
            }
            else
            {
                combo = null;
            }
            return combo;
        }
    }

    public class GenComboItem
    {
        public GenComboItem(string displayValue, string dataValue)
        {
            DataValue = dataValue;
            DisplayValue = displayValue;
        }

        /// <summary>
        /// The value to be shown in the combo list
        /// </summary>
        public string DisplayValue { get; private set; }

        /// <summary>
        /// The value to be saved in the field value
        /// </summary>
        public string DataValue { get; private set; }
    }
}