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
    public partial class LookupUserControl : UserControlBase
    {
        private Lookup _lookup;
        private readonly ToolTip _toolTip = new ToolTip();

        public Lookup Lookup
        {
            get { return _lookup; }
            set
            {
                if (_lookup != value)
                {
                    if (_lookup != null)
                        _lookup.PropertyChanged -= ViewModelPropertyChanged;
                    bindingSourceLookup.DataSource = value ?? DefaultDataSource;
                    _lookup = value;
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

        public LookupUserControl() : base("Lookup")
        {
            InitializeComponent();
            BindingSource = bindingSourceLookup;
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

        public void SetNoMatchReadOnly(bool value)
        {
            textBoxNoMatch.ReadOnly = value;
        }
        
        private void TextBoxNoMatchMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("Is the body expanded if the lookup fails?", textBoxNoMatch);
        }

        private void TextBoxNoMatchMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxNoMatch);
        }

        public void SetClass1ReadOnly(bool value)
        {
            textBoxClass1.ReadOnly = value;
        }
        
        private void TextBoxClass1MouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The class of the object being sought", textBoxClass1);
        }

        private void TextBoxClass1MouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxClass1);
        }

        public void SetProperty1ReadOnly(bool value)
        {
            textBoxProperty1.ReadOnly = value;
        }
        
        private void TextBoxProperty1MouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The property on the object being sought to match", textBoxProperty1);
        }

        private void TextBoxProperty1MouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxProperty1);
        }

        public void SetClass2ReadOnly(bool value)
        {
            textBoxClass2.ReadOnly = value;
        }
        
        private void TextBoxClass2MouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The class of the object with the search value", textBoxClass2);
        }

        private void TextBoxClass2MouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxClass2);
        }

        public void SetProperty2ReadOnly(bool value)
        {
            textBoxProperty2.ReadOnly = value;
        }
        
        private void TextBoxProperty2MouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The property on the object with the search value", textBoxProperty2);
        }

        private void TextBoxProperty2MouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxProperty2);
        }
    }
}
