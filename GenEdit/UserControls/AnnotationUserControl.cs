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
    public partial class AnnotationUserControl : UserControlBase
    {
        private Annotation _annotation;
        private readonly ToolTip _toolTip = new ToolTip();

        public Annotation Annotation
        {
            get { return _annotation; }
            set
            {
                if (_annotation != value)
                {
                    if (_annotation != null)
                        _annotation.PropertyChanged -= ViewModelPropertyChanged;
                    bindingSourceAnnotation.DataSource = value ?? DefaultDataSource;
                    _annotation = value;
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

        public AnnotationUserControl() : base("Annotation")
        {
            InitializeComponent();
            BindingSource = bindingSourceAnnotation;
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
    }
}
