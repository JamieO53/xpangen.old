using System;
using System.ComponentModel;
using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace GenEdit.ViewModel
{
    public abstract class GenDataViewModelBase : BindableObject
    {
        /// <summary>
        /// The list of fields belonging to the <see cref="GenObject"/> being edited.
        /// </summary>
        public abstract ObservableCollection<FieldViewModelBase> Fields { get; }

        protected GenNamedApplicationBase Definition { get; set; }
        public bool IsReadOnly { get; protected set; }

        /// <summary>
        /// One or more field values have changed
        /// </summary>
        public bool Changed { get; set; }

        /// <summary>
        /// The heading value of the tree node
        /// </summary>
        public string Name { get { throw new NotImplementedException(); } }

        /// <summary>
        /// A hint describing the use of the node data
        /// </summary>
        public string Hint { get { throw new NotImplementedException(); } }

        public GenSavedContext SavedContext { get; protected set; }
        public bool IsNew { get; set; }

        /// <summary>
        /// Saves the object data
        /// </summary>
        public virtual void Save()
        {
            Changed = false;
            IsNew = false;
        }

        /// <summary>
        /// Cancels changes to the object data
        /// </summary>
        public virtual void Cancel()
        {
            Changed = false;
        }

        /// <summary>
        /// Establish the context of the current object.
        /// </summary>
        public void EstablishContext()
        {
            if (SavedContext != null) SavedContext.EstablishContext();
        }

        protected void FieldPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Changed = true;
            RaisePropertyChanged(e.PropertyName);
        }
    }
}