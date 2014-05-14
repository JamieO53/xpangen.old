using System;
using System.ComponentModel;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data.Model.Settings;

namespace GenEdit.UserControls
{
    public partial class FileGroupUserControl : UserControlBase
    {
        public EventHandler ProfileSelected;

        public Profile Profile { get; set; }
        private FileGroup _viewModel;
        public FileGroup ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (_viewModel != value)
                {
                    bindingSourceFileGroup.DataSource = value ?? DefaultDataSource;
                    _viewModel = value;
                    if (value != null)
                        value.PropertyChanged += ViewModelPropertyChanged;
                }
            }
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Split('.')[0] == "FileGroup")
                bindingSourceFileGroup.ResetBindings(false);
        }

        public FileGroupUserControl()
        {
            InitializeComponent();
            DefaultDataSource = bindingSourceFileGroup.DataSource;
            var settings = ViewModelLocator.GenDataEditorViewModel.Data.Settings;
            bindingSourceBaseFile.DataSource = settings.GetBaseFiles();
            comboBoxBaseFile.SelectedItem = null;
        }

        private object DefaultDataSource { get; set; }

        private void ComboBoxBaseFileSelectedValueChanged(object sender, EventArgs e)
        {
            if (ViewModel != null && comboBoxBaseFile.SelectedItem != null)
            {
                var profile = ViewModel.Profile;
                var profileList = ((BaseFile) comboBoxBaseFile.SelectedItem).ProfileList;
                bindingSourceProfile.DataSource = profileList;
                comboBoxProfile.SelectedItem = profileList.Find(profile);
                ViewModel.BaseFileName = ((BaseFile) comboBoxBaseFile.SelectedItem).Name;
            }
            else bindingSourceProfile.DataSource = null;
        }

        private void RaiseProfileSelected()
        {
            var handler = ProfileSelected;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void ComboBoxProfileSelectedValueChanged(object sender, EventArgs e)
        {
            Profile = (Profile) comboBoxProfile.SelectedItem;
            RaiseProfileSelected();
        }
    }
}
