using System;
using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Editor.Model;

namespace GenEdit.View
{
    public partial class FileGroupUserControl : UserControl
    {
        public delegate void ProfileSelected();

        public ProfileSelected OnProfileSelected;

        public Profile Profile { get; set; }
        private FileGroup _viewModel;
        public FileGroup ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (_viewModel != value)
                {
                    bindingSourceFileGroup.DataSource = value;
                    _viewModel = value;
                }
            }
        }

        public FileGroupUserControl()
        {
            InitializeComponent();
            var settings = ViewModelLocator.GenDataEditorViewModel.Data.Settings;
            bindingSourceBaseFile.DataSource = settings.GetBaseFiles();
        }

        private void comboBoxBaseFile_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBoxBaseFile.SelectedItem != null)
                bindingSourceProfile.DataSource = ((BaseFile) comboBoxBaseFile.SelectedItem).ProfileList;
            else bindingSourceProfile.DataSource = null;
        }
        private void RaiseProfileSelected()
        {
            if (OnProfileSelected != null)
                OnProfileSelected();
        }

        private void comboBoxProfile_SelectedValueChanged(object sender, EventArgs e)
        {
            Profile = (Profile) comboBoxProfile.SelectedItem;
            RaiseProfileSelected();
        }

    }
}
