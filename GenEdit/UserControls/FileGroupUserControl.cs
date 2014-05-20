// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.ComponentModel;
using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Application;
using org.xpangen.Generator.Data.Model.Settings;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.UserControls
{
    public partial class FileGroupUserControl : UserControlBase
    {
        public EventHandler BaseFileNameSelected;
        public BaseFile BaseFile { get; set; }
        public EventHandler ProfileSelected;
        public Profile Profile { get; set; }
        private FileGroup _fileGroup;
        private readonly ToolTip _toolTip = new ToolTip();

        public FileGroup FileGroup
        {
            get { return _fileGroup; }
            set
            {
                if (_fileGroup != value)
                {
                    if (_fileGroup != null)
                        _fileGroup.PropertyChanged -= ViewModelPropertyChanged;
                    bindingSourceFileGroup.DataSource = value ?? DefaultDataSource;
                    _fileGroup = value;
                    if (value != null)
                    {
                        value.DelayedSave = false;
                        value.PropertyChanged += ViewModelPropertyChanged;
                        ComboBoxBaseFileNameSelectedValueChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        private static IGenDataSettings ViewModel
        {
            get { return ViewModelLocator.GenDataEditorViewModel.Data.Settings; }
        }

        public FileGroupUserControl() : base("FileGroup")
        {
            InitializeComponent();
            BindingSource = bindingSourceFileGroup;
            comboBoxBaseFileName.DataSource = ViewModel.GetDataSource(FileGroup, "BaseFile");
            comboBoxBaseFileName.SelectedItem = null;
        }

        private void RaiseBaseFileNameSelected()
        {
            var handler = BaseFileNameSelected;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void ComboBoxBaseFileNameSelectedValueChanged(object sender, EventArgs e)
        {
            if (FileGroup != null && comboBoxBaseFileName.SelectedItem != null)
            {
                var item = (BaseFile) comboBoxBaseFileName.SelectedItem;
                FileGroup.BaseFileName = item.Name;
                BaseFile = item;
                var profile = FileGroup.Profile;
                var profileList =
                    (GenNamedApplicationList<Profile>) ViewModel.GetDataSource(item, "Profile");
                comboBoxProfile.DataSource = profileList;
                comboBoxProfile.SelectedItem = profileList.Find(profile);
                RaiseBaseFileNameSelected();
                return;
            }
            comboBoxProfile.DataSource = null;
            BaseFile = null;
            RaiseBaseFileNameSelected();
        }
        private void RaiseProfileSelected()
        {
            var handler = ProfileSelected;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void ComboBoxProfileSelectedValueChanged(object sender, EventArgs e)
        {
            if (FileGroup != null && comboBoxProfile.SelectedItem != null)
            {
                var item = (Profile) comboBoxProfile.SelectedItem;
                FileGroup.Profile = item.Name;
                Profile = item;
                RaiseProfileSelected();
                return;
            }
            Profile = null;
            RaiseProfileSelected();
        }

        public void SetNameReadOnly(bool value)
        {
            textBoxName.ReadOnly = value;
        }
        
        private void TextBoxNameMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The file group name", textBoxName);
        }

        private void TextBoxNameMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxName);
        }

        public void SetFileNameReadOnly(bool value)
        {
            textBoxFileName.ReadOnly = value;
        }
        
        private void TextBoxFileNameMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The name of the file being edited", textBoxFileName);
        }

        private void TextBoxFileNameMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxFileName);
        }

        public void SetFilePathReadOnly(bool value)
        {
            textBoxFilePath.ReadOnly = value;
        }
        
        private void TextBoxFilePathMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The full path of the file being edited", textBoxFilePath);
        }

        private void TextBoxFilePathMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxFilePath);
        }

        public void SetBaseFileNameReadOnly(bool value)
        {
            comboBoxBaseFileName.Enabled = value;
        }
        
        private void ComboBoxBaseFileNameMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The name of the file's definitions data file", comboBoxBaseFileName);
        }

        private void ComboBoxBaseFileNameMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(comboBoxBaseFileName);
        }

        public void SetProfileReadOnly(bool value)
        {
            comboBoxProfile.Enabled = value;
        }
        
        private void ComboBoxProfileMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The file path of the profile used to generate the file's output", comboBoxProfile);
        }

        private void ComboBoxProfileMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(comboBoxProfile);
        }

        public void SetGeneratedFileReadOnly(bool value)
        {
            textBoxGeneratedFile.ReadOnly = value;
        }
        
        private void TextBoxGeneratedFileMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The file path of the generated file", textBoxGeneratedFile);
        }

        private void TextBoxGeneratedFileMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxGeneratedFile);
        }
    }
}
