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
    public partial class PlaceholderUserControl : UserControlBase
    {
        private Placeholder _placeholder;
        private readonly ToolTip _toolTip = new ToolTip();

        public Placeholder Placeholder
        {
            get { return _placeholder; }
            set
            {
                if (_placeholder != value)
                {
                    if (_placeholder != null)
                        _placeholder.PropertyChanged -= ViewModelPropertyChanged;
                    bindingSourcePlaceholder.DataSource = value ?? DefaultDataSource;
                    _placeholder = value;
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

        public PlaceholderUserControl() : base("Placeholder")
        {
            InitializeComponent();
            BindingSource = bindingSourcePlaceholder;
        }


        public void SetNameReadOnly(bool value)
        {
            textBoxName.ReadOnly = value;
        }
        
        private void TextBoxNameMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("Generated name of the fragment", textBoxName);
        }

        private void TextBoxNameMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxName);
        }

        public void SetClassReadOnly(bool value)
        {
            textBoxClass.ReadOnly = value;
        }
        
        private void TextBoxClassMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The class containing the value to substitute in place", textBoxClass);
        }

        private void TextBoxClassMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxClass);
        }

        public void SetPropertyReadOnly(bool value)
        {
            textBoxProperty.ReadOnly = value;
        }
        
        private void TextBoxPropertyMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The property whose value is to be substituted in place", textBoxProperty);
        }

        private void TextBoxPropertyMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxProperty);
        }
    }
}
