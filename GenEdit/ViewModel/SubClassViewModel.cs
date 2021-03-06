using org.xpangen.Generator.Data;

namespace GenEdit.ViewModel
{
    public class SubClassViewModel : GenDataViewModelBase
    {
        private ObservableCollection<FieldViewModelBase> _fields;

        public SubClassViewModel(ISubClassBase parent, ISubClassBase subClassBase,
                                 GenNamedApplicationBase def, GenDataDefSubClass subClassDef, bool isReadOnly)
        {
            Parent = parent;
            Definition = def;
            SubClassDef = subClassDef;
            IsReadOnly = isReadOnly;
//            SavedContext = savedContext;
            SubClassBase = subClassBase;
            SavedReference = Parent != null ? Parent.Reference : "";
            GenObject = subClassBase.Parent;
        }

        public GenDataDefSubClass SubClassDef { get; private set; }

        public ISubClassBase SubClassBase { get; private set; }

        public ISubClassBase Parent { get; private set; }

        private string SavedReference { get; set; }

        public override string Name
        {
            get { return SubClassDef.SubClass.Name; }
        }

        /// <summary>
        /// Cancels changes to the object data
        /// </summary>
        public override void Cancel()
        {
            Parent.Reference = SavedReference;
            base.Cancel();
        }

        /// <summary>
        /// Saves the object data
        /// </summary>
        public override void Save()
        {
            SavedReference = "";
            base.Save();
        }
        
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

                    var item = new SubClassFieldViewModel(Parent, SubClassDef, SubClassBase, SubClassField.Name,
                                                          true);
                    _fields.Add(item);
                    item = new SubClassFieldViewModel(Parent, SubClassDef, SubClassBase, SubClassField.Reference,
                                                      IsReadOnly);
                    _fields.Add(item);
                }
                return _fields;
            }
        }
    }
}