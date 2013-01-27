using org.xpangen.Generator.Data;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.ViewModel
{
    public class GenDataEditorViewModel: BindableObject
    {
        private GenObjectViewModel _selectedNode;
        public GeData Data { get; set; }
        public GenObjectViewModel SelectedNode
        {
            get
            {
                return _selectedNode;
            }
            set
            {
                if (value != null)
                    value.EstablishContext();
                _selectedNode = value;
            }
        }

    }
}