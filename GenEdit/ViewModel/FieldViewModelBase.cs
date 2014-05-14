using System.Collections.Generic;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.ViewModel
{
    public abstract class FieldViewModelBase : BindableObject
    {
        protected string _name;
        protected readonly GenDataEditorViewModel genDataEditorViewModel = ViewModelLocator.GenDataEditorViewModel;

        /// <summary>
        /// The name of the field being edited.
        /// </summary>
        public string Name { get; protected set; }

        /// <summary>
        /// A hint describing the purpose of the field.
        /// </summary>
        public string Hint { get; protected set; }

        /// <summary>
        /// The type of data being edited.
        /// </summary>
        public string DataType { get; protected set; }

        /// <summary>
        /// The default value for this field when a new object is created.
        /// </summary>
        public string Default { get; protected set; }

        /// <summary>
        /// An optional list of values
        /// </summary>
        public List<GeComboItem> ComboValues { get; protected set; }

        /// <summary>
        /// The value of the field
        /// </summary>
        public abstract string Value { get; set; }

        /// <summary>
        /// Is this data readonly?
        /// </summary>
        public bool IsReadOnly { get; protected set; }
    }
}