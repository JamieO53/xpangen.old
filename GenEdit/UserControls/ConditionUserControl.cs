// // This Source Code Form is subject to the terms of the Mozilla Public
// // License, v. 2.0. If a copy of the MPL was not distributed with this
// //  file, You can obtain one at http://mozilla.org/MPL/2.0/.

using System;
using System.ComponentModel;
using System.Windows.Forms;
using GenEdit.ViewModel;
using org.xpangen.Generator.Data;
using org.xpangen.Generator.Data.Model.Codes;
using org.xpangen.Generator.Profile.Profile;
using org.xpangen.Generator.Editor.Helper;

namespace GenEdit.UserControls
{
    public partial class ConditionUserControl : UserControlBase
    {
        public EventHandler ComparisonSelected;
        public Code Comparison { get; set; }
        private Condition _condition;
        private readonly ToolTip _toolTip = new ToolTip();

        public Condition Condition
        {
            get { return _condition; }
            set
            {
                if (_condition != value)
                {
                    if (_condition != null)
                        _condition.PropertyChanged -= ViewModelPropertyChanged;
                    bindingSourceCondition.DataSource = value ?? DefaultDataSource;
                    _condition = value;
                    if (value != null)
                    {
                        value.DelayedSave = false;
                        value.PropertyChanged += ViewModelPropertyChanged;
                        ComboBoxComparisonSelectedValueChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        private static IGenDataProfile ViewModel
        {
            get { return ViewModelLocator.GenDataEditorViewModel.Data.Profile; }
        }

        public ConditionUserControl() : base("Condition")
        {
            InitializeComponent();
            BindingSource = bindingSourceCondition;
            comboBoxComparison.DataSource = ViewModel.GetDataSource(Condition, "Comparison");
            comboBoxComparison.SelectedItem = null;
        }

        private void RaiseComparisonSelected()
        {
            var handler = ComparisonSelected;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void ComboBoxComparisonSelectedValueChanged(object sender, EventArgs e)
        {
            if (Condition != null && comboBoxComparison.SelectedItem != null)
            {
                var item = (Code) comboBoxComparison.SelectedItem;
                Condition.Comparison = item.Name;
                Comparison = item;
                RaiseComparisonSelected();
                return;
            }
            Comparison = null;
            RaiseComparisonSelected();
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

        public void SetClass1ReadOnly(bool value)
        {
            textBoxClass1.ReadOnly = value;
        }
        
        private void TextBoxClass1MouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The class of the object being compared", textBoxClass1);
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
            _toolTip.Show("The property whose value is being compared", textBoxProperty1);
        }

        private void TextBoxProperty1MouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxProperty1);
        }

        public void SetComparisonReadOnly(bool value)
        {
            comboBoxComparison.Enabled = value;
        }
        
        private void ComboBoxComparisonMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The comparison to be used", comboBoxComparison);
        }

        private void ComboBoxComparisonMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(comboBoxComparison);
        }

        public void SetClass2ReadOnly(bool value)
        {
            textBoxClass2.ReadOnly = value;
        }
        
        private void TextBoxClass2MouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The class of the object being compared to", textBoxClass2);
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
            _toolTip.Show("The property whose value is being compared to", textBoxProperty2);
        }

        private void TextBoxProperty2MouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxProperty2);
        }

        public void SetLitReadOnly(bool value)
        {
            textBoxLit.ReadOnly = value;
        }
        
        private void TextBoxLitMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("The literal value being compared to", textBoxLit);
        }

        private void TextBoxLitMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxLit);
        }

        public void SetUseLitReadOnly(bool value)
        {
            textBoxUseLit.ReadOnly = value;
        }
        
        private void TextBoxUseLitMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("Is the literal value to be used?", textBoxUseLit);
        }

        private void TextBoxUseLitMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxUseLit);
        }
    }
}
