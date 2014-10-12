namespace GenEdit.UserControls
{
    partial class SegmentUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelUcHeader = new System.Windows.Forms.Panel();
            this.labelUcHeader = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.bindingSourceSegment = new System.Windows.Forms.BindingSource(this.components);
            this.labelClass = new System.Windows.Forms.Label();
            this.textBoxClass = new System.Windows.Forms.TextBox();
            this.labelCardinality = new System.Windows.Forms.Label();
            this.comboBoxCardinality = new System.Windows.Forms.ComboBox();
            this.panelUcHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSegment)).BeginInit();
            this.SuspendLayout();
            // 
            // panelUcHeader
            // 
            this.panelUcHeader.Controls.Add(this.labelUcHeader);
            this.panelUcHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelUcHeader.Location = new System.Drawing.Point(0, 0);
            this.panelUcHeader.Name = "panelUcHeader";
            this.panelUcHeader.Size = new System.Drawing.Size(259, 20);
            this.panelUcHeader.TabIndex = 1;
            // 
            // labelUcHeader
            // 
            this.labelUcHeader.AutoSize = true;
            this.labelUcHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelUcHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUcHeader.Location = new System.Drawing.Point(0, 0);
            this.labelUcHeader.Name = "labelUcHeader";
            this.labelUcHeader.Size = new System.Drawing.Size(64, 17);
            this.labelUcHeader.TabIndex = 0;
            this.labelUcHeader.Text = "Segment";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 23);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceSegment, "Name", true));
            this.textBoxName.Location = new System.Drawing.Point(6, 39);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(250, 20);
            this.textBoxName.TabIndex = 3;
            this.textBoxName.MouseEnter += new System.EventHandler(this.TextBoxNameMouseEnter);
            this.textBoxName.MouseLeave += new System.EventHandler(this.TextBoxNameMouseLeave);
            // 
            // bindingSourceSegment
            // 
            this.bindingSourceSegment.DataSource = typeof(org.xpangen.Generator.Profile.Profile.Segment);
            // 
            // labelClass
            // 
            this.labelClass.AutoSize = true;
            this.labelClass.Location = new System.Drawing.Point(3, 62);
            this.labelClass.Name = "labelClass";
            this.labelClass.Size = new System.Drawing.Size(32, 13);
            this.labelClass.TabIndex = 4;
            this.labelClass.Text = "Class";
            // 
            // textBoxClass
            // 
            this.textBoxClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClass.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceSegment, "Class", true));
            this.textBoxClass.Location = new System.Drawing.Point(6, 78);
            this.textBoxClass.Name = "textBoxClass";
            this.textBoxClass.Size = new System.Drawing.Size(250, 20);
            this.textBoxClass.TabIndex = 5;
            this.textBoxClass.MouseEnter += new System.EventHandler(this.TextBoxClassMouseEnter);
            this.textBoxClass.MouseLeave += new System.EventHandler(this.TextBoxClassMouseLeave);
            // 
            // labelCardinality
            // 
            this.labelCardinality.AutoSize = true;
            this.labelCardinality.Location = new System.Drawing.Point(3, 101);
            this.labelCardinality.Name = "labelCardinality";
            this.labelCardinality.Size = new System.Drawing.Size(55, 13);
            this.labelCardinality.TabIndex = 6;
            this.labelCardinality.Text = "Cardinality";
            // 
            // comboBoxCardinality
            // 
            this.comboBoxCardinality.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxCardinality.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bindingSourceSegment, "Cardinality", true));
            this.comboBoxCardinality.DataSource = this.bindingSourceSegment;
            this.comboBoxCardinality.DisplayMember = "Name";
            this.comboBoxCardinality.FormattingEnabled = true;
            this.comboBoxCardinality.Location = new System.Drawing.Point(6, 117);
            this.comboBoxCardinality.Name = "comboBoxCardinality";
            this.comboBoxCardinality.Size = new System.Drawing.Size(250, 21);
            this.comboBoxCardinality.TabIndex = 7;
            this.comboBoxCardinality.ValueMember = "Name";
            this.comboBoxCardinality.SelectedValueChanged += new System.EventHandler(this.ComboBoxCardinalitySelectedValueChanged);
            this.comboBoxCardinality.MouseEnter += new System.EventHandler(this.ComboBoxCardinalityMouseEnter);
            this.comboBoxCardinality.MouseLeave += new System.EventHandler(this.ComboBoxCardinalityMouseLeave);
            // 
            // SegmentUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelUcHeader);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelClass);
            this.Controls.Add(this.textBoxClass);
            this.Controls.Add(this.labelCardinality);
            this.Controls.Add(this.comboBoxCardinality);
            this.Name = "SegmentUserControl";
            this.Size = new System.Drawing.Size(259, 146);
            this.panelUcHeader.ResumeLayout(false);
            this.panelUcHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSegment)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelUcHeader;
        private System.Windows.Forms.Label labelUcHeader;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelClass;
        private System.Windows.Forms.TextBox textBoxClass;
        private System.Windows.Forms.Label labelCardinality;
        private System.Windows.Forms.ComboBox comboBoxCardinality;
        private System.Windows.Forms.BindingSource bindingSourceSegment;
    }
}
