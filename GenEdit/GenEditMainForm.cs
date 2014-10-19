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
            genDataEditor1.OnDataChanged =
                () =>
                    genProfileEditor1.RefreshProfile(genProfileEditor1.GenDataEditorViewModel.Data.GenDataBase,
                        genProfileEditor1.GenDataEditorViewModel.Data.GenObject);
            genDataEditor1.OnDataChanged += () => genLibrary1.DataChanged();
            genDataEditor1.OnFocusChanged =
                () =>
                    genProfileEditor1.RefreshProfile(genProfileEditor1.GenDataEditorViewModel.Data.GenDataBase,
                        genProfileEditor1.GenDataEditorViewModel.Data.GenObject);
            genLibrary1.GenDataEditorViewModel = ViewModelLocator.GenDataEditorViewModel;
            genLibrary1.OnDataLoaded = () => genDataEditor1.LoadData();
            //genLibrary1.OnDataLoaded += () => genProfileEditor1.LoadData();
            genLibrary1.OnProfileChanged = () => genProfileEditor1.LoadData();
        }

        private void GenEditMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            var vm = ViewModelLocator.GenDataEditorViewModel;
            genDataEditor1.SaveEditorChanges();
            if (!vm.Data.Changed)
            {
                genLibrary1.OnDataLoaded = DoNothing;
                genLibrary1.OnProfileChanged = DoNothing;
                return;
            }

            var dr = MessageBox.Show("The data has changed. Save before closing?", "GenEdit closing",
                                     MessageBoxButtons.YesNoCancel);
            switch (dr)
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                case DialogResult.Yes:
                    genLibrary1.OnDataLoaded = DoNothing;
                    genLibrary1.OnProfileChanged = DoNothing;
                    genLibrary1.SaveOrCreateFile();
                    break;
                case DialogResult.No:
                    genLibrary1.OnDataLoaded = DoNothing;
                    genLibrary1.OnProfileChanged = DoNothing;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void DoNothing(){}
    }
}
