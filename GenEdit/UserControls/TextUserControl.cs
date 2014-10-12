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
    public partial class TextUserControl : UserControlBase
    {
        private Text _text;
        private readonly ToolTip _toolTip = new ToolTip();

        public Text Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    if (_text != null)
                        _text.PropertyChanged -= ViewModelPropertyChanged;
                    bindingSourceText.DataSource = value ?? DefaultDataSource;
                    _text = value;
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

        public TextUserControl() : base("Text")
        {
            InitializeComponent();
            BindingSource = bindingSourceText;
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

        public void SetTextValueReadOnly(bool value)
        {
            textBoxTextValue.ReadOnly = value;
        }
        
        private void TextBoxTextValueMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The plain text being generated", textBoxTextValue);
        }

        private void TextBoxTextValueMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxTextValue);
        }
    }
}
