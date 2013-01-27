using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit
{
    public partial class GenEditMainForm : Form
    {
        public GenEditMainForm()
        {
            InitializeComponent();
            genDataEditor1.GenDataEditorViewModel = ViewModelLocator.GenDataEditorViewModel;
            genProfileEditor1.GenDataEditorViewModel = ViewModelLocator.GenDataEditorViewModel;
        }

    }
}
