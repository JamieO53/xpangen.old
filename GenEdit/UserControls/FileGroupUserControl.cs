// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.ComponentModel;
using GenEdit.ViewModel;
using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data.Model.Settings;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.UserControls
{
    public partial class FileGroupUserControl : UserControlBase
    {
        public EventHandler ProfileSelected;

        public Profile Profile { get; set; }
        private FileGroup _fileGroup;

        public FileGroup FileGroup
        {
            private get { return _fileGroup; }
            set
            {
                if (_fileGroup != value)
                {
                    bindingSourceFileGroup.DataSource = value ?? DefaultDataSource;
                    _fileGroup = value;
                    if (value != null)
                    {
                        value.PropertyChanged += ViewModelPropertyChanged;
                        ComboBoxBaseFileSelectedValueChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        private void ViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Split('.')[0] == "FileGroup")
                bindingSourceFileGroup.ResetBindings(false);
        }

        private static IGenDataSettings ViewModel
        {
            get { return ViewModelLocator.GenDataEditorViewModel.Data.Settings; }
        }

        public FileGroupUserControl()
        {
            InitializeComponent();
            DefaultDataSource = bindingSourceFileGroup.DataSource;
            comboBoxBaseFile.DataSource = ViewModel.GetDataSource(FileGroup, "BaseFile");
            comboBoxBaseFile.SelectedItem = null;
        }

        private object DefaultDataSource { get; set; }

        private void ComboBoxBaseFileSelectedValueChanged(object sender, EventArgs e)
        {
            if (FileGroup != null && comboBoxBaseFile.SelectedItem != null)
            {
                var profile = FileGroup.Profile;
                var profileList =
                    (GenNamedApplicationList<Profile>) ViewModel.GetDataSource(comboBoxBaseFile.SelectedItem, "Profile");
                comboBoxProfile.DataSource = profileList;
                comboBoxProfile.SelectedItem = profileList.Find(profile);
                FileGroup.BaseFileName = ((BaseFile) comboBoxBaseFile.SelectedItem).Name;
                ComboBoxProfileSelectedValueChanged(sender, e);
            }
            else comboBoxProfile.DataSource = null;
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