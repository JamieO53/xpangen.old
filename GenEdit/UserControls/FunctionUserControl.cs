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
    public partial class FunctionUserControl : UserControlBase
    {
        private Function _function;
        private readonly ToolTip _toolTip = new ToolTip();

        public Function Function
        {
            get { return _function; }
            set
            {
                if (_function != value)
                {
                    if (_function != null)
                        _function.PropertyChanged -= ViewModelPropertyChanged;
                    bindingSourceFunction.DataSource = value ?? DefaultDataSource;
                    _function = value;
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

        public FunctionUserControl() : base("Function")
        {
            InitializeComponent();
            BindingSource = bindingSourceFunction;
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

        public void SetFunctionNameReadOnly(bool value)
        {
            textBoxFunctionName.ReadOnly = value;
        }
        
        private void TextBoxFunctionNameMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The name of the function", textBoxFunctionName);
        }

        private void TextBoxFunctionNameMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxFunctionName);
        }
    }
}
