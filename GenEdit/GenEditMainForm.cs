using System.Windows.Forms;
using GenEdit.ViewModel;

namespace GenEdit
{
    public partial class GenEditMainForm : Form
    {
        public GenEditMainForm()
        {
            InitializeComponent();
            genDataEditor1.GenDataEditorViewModel = ViewModelLocator.GenDataEditorViewModel;
            genProfileEditor1.GenDataEditorViewModel = ViewModelLocator.GenDataEditorViewModel;
            genDataEditor1.OnDataChanged = () => genProfileEditor1.RefreshProfile(genProfileEditor1.GenDataEditorViewModel.Data.GenData);
            genDataEditor1.OnDataChanged += () => genLibrary1.DataChanged();
            genLibrary1.GenDataEditorViewModel = ViewModelLocator.GenDataEditorViewModel;
            genLibrary1.OnDataLoaded = () => genDataEditor1.LoadData();
            genLibrary1.OnDataLoaded += () => genProfileEditor1.LoadData();
        }

    }
}
