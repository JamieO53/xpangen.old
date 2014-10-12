// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.ComponentModel;
using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Profile.Profile;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.UserControls
{
    public partial class ProfileUserControl : UserControlBase
    {
        private Profile _profile;
        private readonly ToolTip _toolTip = new ToolTip();

        public Profile Profile
        {
            get { return _profile; }
            set
            {
                if (_profile != value)
                {
                    if (_profile != null)
                        _profile.PropertyChanged -= ViewModelPropertyChanged;
                    bindingSourceProfile.DataSource = value ?? DefaultDataSource;
                    _profile = value;
                    if (value != null)
                    {
                        value.DelayedSave = false;
                        value.PropertyChanged += ViewModelPropertyChanged;
                    }
                }
            }
        }

        private static IGenDataProfile ViewModel
        {
            get { return ViewModelLocator.GenDataEditorViewModel.Data.Profile; }
        }

        public ProfileUserControl() : base("Profile")
        {
            InitializeComponent();
            BindingSource = bindingSourceProfile;
        }


        public void SetNameReadOnly(bool value)
        {
            textBoxName.ReadOnly = value;
        }
        
        private void TextBoxNameMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The name of the profile fragment", textBoxName);
        }

        private void TextBoxNameMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxName);
        }
    }
}
