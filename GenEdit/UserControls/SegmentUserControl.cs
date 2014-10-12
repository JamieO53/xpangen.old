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
    public partial class SegmentUserControl : UserControlBase
    {
        public EventHandler CardinalitySelected;
        public Code Cardinality { get; set; }
        private Segment _segment;
        private readonly ToolTip _toolTip = new ToolTip();

        public Segment Segment
        {
            get { return _segment; }
            set
            {
                if (_segment != value)
                {
                    if (_segment != null)
                        _segment.PropertyChanged -= ViewModelPropertyChanged;
                    bindingSourceSegment.DataSource = value ?? DefaultDataSource;
                    _segment = value;
                    if (value != null)
                    {
                        value.DelayedSave = false;
                        value.PropertyChanged += ViewModelPropertyChanged;
                        ComboBoxCardinalitySelectedValueChanged(this, EventArgs.Empty);
                    }
                }
            }
        }

        private static IGenDataProfile ViewModel
        {
            get { return ViewModelLocator.GenDataEditorViewModel.Data.Profile; }
        }

        public SegmentUserControl() : base("Segment")
        {
            InitializeComponent();
            BindingSource = bindingSourceSegment;
            comboBoxCardinality.DataSource = ViewModel.GetDataSource(Segment, "Cardinality");
            comboBoxCardinality.SelectedItem = null;
        }

        private void RaiseCardinalitySelected()
        {
            var handler = CardinalitySelected;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        private void ComboBoxCardinalitySelectedValueChanged(object sender, EventArgs e)
        {
            if (Segment != null && comboBoxCardinality.SelectedItem != null)
            {
                var item = (Code) comboBoxCardinality.SelectedItem;
                Segment.Cardinality = item.Name;
                Cardinality = item;
                RaiseCardinalitySelected();
                return;
            }
            Cardinality = null;
            RaiseCardinalitySelected();
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
            _toolTip.Show("The class of the fragment", textBoxClass);
        }

        private void TextBoxClassMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(textBoxClass);
        }

        public void SetCardinalityReadOnly(bool value)
        {
            comboBoxCardinality.Enabled = value;
        }
        
        private void ComboBoxCardinalityMouseEnter(object sender, EventArgs e)
        {
            _toolTip.Show("How the class objects are to be generated", comboBoxCardinality);
        }

        private void ComboBoxCardinalityMouseLeave(object sender, EventArgs e)
        {
            _toolTip.Hide(comboBoxCardinality);
        }
    }
}
