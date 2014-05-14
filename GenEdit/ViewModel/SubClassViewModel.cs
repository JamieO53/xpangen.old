using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data;

namespace GenEdit.ViewModel
{
    public class SubClassViewModel : GenDataViewModelBase
    {
        private ObservableCollection<FieldViewModelBase> _fields;

        public SubClassViewModel(IGenObjectListBase parent, IGenObjectListBase genObjectListBase,
                                 GenNamedApplicationBase def, GenDataDefSubClass subClassDef,
                                 GenSavedContext savedContext, bool isReadOnly)
        {
            Parent = parent;
            Definition = def;
            SubClassDef = subClassDef;
            IsReadOnly = isReadOnly;
            SavedContext = savedContext;
            GenObjectListBase = genObjectListBase;
            SavedReference = Parent != null ? Parent.Reference : "";
        }

        public GenDataDefSubClass SubClassDef { get; private set; }

        public IGenObjectListBase GenObjectListBase { get; private set; }

        public IGenObjectListBase Parent { get; private set; }

        private string SavedReference { get; set; }

        public override string Name
        {
            get { return SubClassDef.SubClass.Name; }
        }

        public override void Cancel()
        {
            Parent.Reference = SavedReference;
            base.Cancel();
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

                    var item = new SubClassFieldViewModel(Parent, SubClassDef, GenObjectListBase, SubClassField.Name,
                                                          true);
                    _fields.Add(item);
                    item = new SubClassFieldViewModel(Parent, SubClassDef, GenObjectListBase, SubClassField.Reference,
                                                      IsReadOnly);
                    _fields.Add(item);
                }
                return _fields;
            }
        }
    }
}