using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit
{
    public partial class GenEditMainForm : Form
    {
        public GeData GeData { get; private set; }
        public GenEditMainForm()
        {
            InitializeComponent();
            GeData = ViewModelLocator.GeData;
            genDataEditor1.GenDataEditorViewModel = new GenDataEditorViewModel {Data = GeData};
            genProfileEditor1.GenDataEditorViewModel = new GenDataEditorViewModel {Data = GeData};
        }

        private void GenEditMainForm_Load(object sender, System.EventArgs e)
        {

        }
    }
}
