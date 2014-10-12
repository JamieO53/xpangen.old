namespace GenEdit.UserControls
{
    partial class ConditionUserControl
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
            this.labelClass1 = new System.Windows.Forms.Label();
            this.textBoxClass1 = new System.Windows.Forms.TextBox();
            this.labelProperty1 = new System.Windows.Forms.Label();
            this.textBoxProperty1 = new System.Windows.Forms.TextBox();
            this.labelComparison = new System.Windows.Forms.Label();
            this.comboBoxComparison = new System.Windows.Forms.ComboBox();
            this.labelClass2 = new System.Windows.Forms.Label();
            this.textBoxClass2 = new System.Windows.Forms.TextBox();
            this.labelProperty2 = new System.Windows.Forms.Label();
            this.textBoxProperty2 = new System.Windows.Forms.TextBox();
            this.labelLit = new System.Windows.Forms.Label();
            this.textBoxLit = new System.Windows.Forms.TextBox();
            this.labelUseLit = new System.Windows.Forms.Label();
            this.textBoxUseLit = new System.Windows.Forms.TextBox();
            this.bindingSourceCondition = new System.Windows.Forms.BindingSource(this.components);
            this.panelUcHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCondition)).BeginInit();
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
            this.labelUcHeader.Size = new System.Drawing.Size(250, 17);
            this.labelUcHeader.TabIndex = 0;
            this.labelUcHeader.Text = "Condition";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(3, 23);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(250, 13);
            this.labelName.TabIndex = 2;
            this.labelName.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceCondition, "Name", true));
            this.textBoxName.Location = new System.Drawing.Point(6, 39);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(250, 20);
            this.textBoxName.TabIndex = 3;
            this.textBoxName.MouseEnter += new System.EventHandler(this.TextBoxNameMouseEnter);
            this.textBoxName.MouseLeave += new System.EventHandler(this.TextBoxNameMouseLeave);
            // 
            // labelClass1
            // 
            this.labelClass1.AutoSize = true;
            this.labelClass1.Location = new System.Drawing.Point(3, 62);
            this.labelClass1.Name = "labelClass1";
            this.labelClass1.Size = new System.Drawing.Size(250, 13);
            this.labelClass1.TabIndex = 4;
            this.labelClass1.Text = "Class 1";
            // 
            // textBoxClass1
            // 
            this.textBoxClass1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClass1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceCondition, "Class1", true));
            this.textBoxClass1.Location = new System.Drawing.Point(6, 78);
            this.textBoxClass1.Name = "textBoxClass1";
            this.textBoxClass1.Size = new System.Drawing.Size(250, 20);
            this.textBoxClass1.TabIndex = 5;
            this.textBoxClass1.MouseEnter += new System.EventHandler(this.TextBoxClass1MouseEnter);
            this.textBoxClass1.MouseLeave += new System.EventHandler(this.TextBoxClass1MouseLeave);
            // 
            // labelProperty1
            // 
            this.labelProperty1.AutoSize = true;
            this.labelProperty1.Location = new System.Drawing.Point(3, 101);
            this.labelProperty1.Name = "labelProperty1";
            this.labelProperty1.Size = new System.Drawing.Size(250, 13);
            this.labelProperty1.TabIndex = 6;
            this.labelProperty1.Text = "Property 1";
            // 
            // textBoxProperty1
            // 
            this.textBoxProperty1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProperty1.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceCondition, "Property1", true));
            this.textBoxProperty1.Location = new System.Drawing.Point(6, 117);
            this.textBoxProperty1.Name = "textBoxProperty1";
            this.textBoxProperty1.Size = new System.Drawing.Size(250, 20);
            this.textBoxProperty1.TabIndex = 7;
            this.textBoxProperty1.MouseEnter += new System.EventHandler(this.TextBoxProperty1MouseEnter);
            this.textBoxProperty1.MouseLeave += new System.EventHandler(this.TextBoxProperty1MouseLeave);
            // 
            // labelComparison
            // 
            this.labelComparison.AutoSize = true;
            this.labelComparison.Location = new System.Drawing.Point(3, 140);
            this.labelComparison.Name = "labelComparison";
            this.labelComparison.Size = new System.Drawing.Size(250, 13);
            this.labelComparison.TabIndex = 8;
            this.labelComparison.Text = "Comparison";
            // 
            // comboBoxComparison
            // 
            this.comboBoxComparison.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxComparison.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bindingSourceCondition, "Comparison", true));
            this.comboBoxComparison.DisplayMember = "Name";
            this.comboBoxComparison.FormattingEnabled = true;
            this.comboBoxComparison.Location = new System.Drawing.Point(6, 156);
            this.comboBoxComparison.Name = "comboBoxComparison";
            this.comboBoxComparison.Size = new System.Drawing.Size(250, 21);
            this.comboBoxComparison.TabIndex = 9;
            this.comboBoxComparison.ValueMember = "Name";
            this.comboBoxComparison.SelectedValueChanged += new System.EventHandler(this.ComboBoxComparisonSelectedValueChanged);
            this.comboBoxComparison.MouseEnter += new System.EventHandler(this.ComboBoxComparisonMouseEnter);
            this.comboBoxComparison.MouseLeave += new System.EventHandler(this.ComboBoxComparisonMouseLeave);
            // 
            // labelClass2
            // 
            this.labelClass2.AutoSize = true;
            this.labelClass2.Location = new System.Drawing.Point(3, 180);
            this.labelClass2.Name = "labelClass2";
            this.labelClass2.Size = new System.Drawing.Size(250, 13);
            this.labelClass2.TabIndex = 10;
            this.labelClass2.Text = "Class 2";
            // 
            // textBoxClass2
            // 
            this.textBoxClass2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClass2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceCondition, "Class2", true));
            this.textBoxClass2.Location = new System.Drawing.Point(6, 196);
            this.textBoxClass2.Name = "textBoxClass2";
            this.textBoxClass2.Size = new System.Drawing.Size(250, 20);
            this.textBoxClass2.TabIndex = 11;
            this.textBoxClass2.MouseEnter += new System.EventHandler(this.TextBoxClass2MouseEnter);
            this.textBoxClass2.MouseLeave += new System.EventHandler(this.TextBoxClass2MouseLeave);
            // 
            // labelProperty2
            // 
            this.labelProperty2.AutoSize = true;
            this.labelProperty2.Location = new System.Drawing.Point(3, 219);
            this.labelProperty2.Name = "labelProperty2";
            this.labelProperty2.Size = new System.Drawing.Size(250, 13);
            this.labelProperty2.TabIndex = 12;
            this.labelProperty2.Text = "Property 2";
            // 
            // textBoxProperty2
            // 
            this.textBoxProperty2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProperty2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceCondition, "Property2", true));
            this.textBoxProperty2.Location = new System.Drawing.Point(6, 235);
            this.textBoxProperty2.Name = "textBoxProperty2";
            this.textBoxProperty2.Size = new System.Drawing.Size(250, 20);
            this.textBoxProperty2.TabIndex = 13;
            this.textBoxProperty2.MouseEnter += new System.EventHandler(this.TextBoxProperty2MouseEnter);
            this.textBoxProperty2.MouseLeave += new System.EventHandler(this.TextBoxProperty2MouseLeave);
            // 
            // labelLit
            // 
            this.labelLit.AutoSize = true;
            this.labelLit.Location = new System.Drawing.Point(3, 258);
            this.labelLit.Name = "labelLit";
            this.labelLit.Size = new System.Drawing.Size(250, 13);
            this.labelLit.TabIndex = 14;
            this.labelLit.Text = "Lit";
            // 
            // textBoxLit
            // 
            this.textBoxLit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxLit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceCondition, "Lit", true));
            this.textBoxLit.Location = new System.Drawing.Point(6, 274);
            this.textBoxLit.Name = "textBoxLit";
            this.textBoxLit.Size = new System.Drawing.Size(250, 20);
            this.textBoxLit.TabIndex = 15;
            this.textBoxLit.MouseEnter += new System.EventHandler(this.TextBoxLitMouseEnter);
            this.textBoxLit.MouseLeave += new System.EventHandler(this.TextBoxLitMouseLeave);
            // 
            // labelUseLit
            // 
            this.labelUseLit.AutoSize = true;
            this.labelUseLit.Location = new System.Drawing.Point(3, 297);
            this.labelUseLit.Name = "labelUseLit";
            this.labelUseLit.Size = new System.Drawing.Size(250, 13);
            this.labelUseLit.TabIndex = 16;
            this.labelUseLit.Text = "Use lit";
            // 
            // textBoxUseLit
            // 
            this.textBoxUseLit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxUseLit.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceCondition, "UseLit", true));
            this.textBoxUseLit.Location = new System.Drawing.Point(6, 313);
            this.textBoxUseLit.Name = "textBoxUseLit";
            this.textBoxUseLit.Size = new System.Drawing.Size(250, 20);
            this.textBoxUseLit.TabIndex = 17;
            this.textBoxUseLit.MouseEnter += new System.EventHandler(this.TextBoxUseLitMouseEnter);
            this.textBoxUseLit.MouseLeave += new System.EventHandler(this.TextBoxUseLitMouseLeave);
            //
            // bindingSourceCondition
            // 
            this.bindingSourceCondition.DataSource = typeof(org.xpangen.Generator.Profile.Profile.Condition);
            // 
            // ConditionUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelUcHeader);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelClass1);
            this.Controls.Add(this.textBoxClass1);
            this.Controls.Add(this.labelProperty1);
            this.Controls.Add(this.textBoxProperty1);
            this.Controls.Add(this.labelComparison);
            this.Controls.Add(this.comboBoxComparison);
            this.Controls.Add(this.labelClass2);
            this.Controls.Add(this.textBoxClass2);
            this.Controls.Add(this.labelProperty2);
            this.Controls.Add(this.textBoxProperty2);
            this.Controls.Add(this.labelLit);
            this.Controls.Add(this.textBoxLit);
            this.Controls.Add(this.labelUseLit);
            this.Controls.Add(this.textBoxUseLit);
            this.Name = "ConditionUserControl";
            this.Size = new System.Drawing.Size(259, 341);
            this.panelUcHeader.ResumeLayout(false);
            this.panelUcHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceCondition)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelUcHeader;
        private System.Windows.Forms.Label labelUcHeader;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelClass1;
        private System.Windows.Forms.TextBox textBoxClass1;
        private System.Windows.Forms.Label labelProperty1;
        private System.Windows.Forms.TextBox textBoxProperty1;
        private System.Windows.Forms.Label labelComparison;
        private System.Windows.Forms.ComboBox comboBoxComparison;
        private System.Windows.Forms.Label labelClass2;
        private System.Windows.Forms.TextBox textBoxClass2;
        private System.Windows.Forms.Label labelProperty2;
        private System.Windows.Forms.TextBox textBoxProperty2;
        private System.Windows.Forms.Label labelLit;
        private System.Windows.Forms.TextBox textBoxLit;
        private System.Windows.Forms.Label labelUseLit;
        private System.Windows.Forms.TextBox textBoxUseLit;
        private System.Windows.Forms.BindingSource bindingSourceCondition;
    }
}
