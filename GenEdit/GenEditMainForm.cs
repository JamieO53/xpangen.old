using System;
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
            genDataEditor1.OnFocusChanged = () => genProfileEditor1.RefreshProfile(genProfileEditor1.GenDataEditorViewModel.Data.GenData);
            genLibrary1.GenDataEditorViewModel = ViewModelLocator.GenDataEditorViewModel;
            genLibrary1.OnDataLoaded = () => genDataEditor1.LoadData();
            genLibrary1.OnDataLoaded += () => genProfileEditor1.LoadData();
            genLibrary1.OnProfileChanged = () => genProfileEditor1.RefreshProfile(genProfileEditor1.GenDataEditorViewModel.Data.GenData);
        }

        private void GenEditMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var vm = ViewModelLocator.GenDataEditorViewModel;
            genDataEditor1.SaveEditorChanges();
            if (!vm.Data.Changed) return;

            var dr = MessageBox.Show("The data has changed. Save before closing?", "GenEdit closing",
                                     MessageBoxButtons.YesNoCancel);
            switch (dr)
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    genLibrary1.SaveOrCreateFile();
                    break;
                case DialogResult.No:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
