namespace GenEdit.UserControls
{
    partial class PlaceholderUserControl
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
            this.labelClass = new System.Windows.Forms.Label();
            this.textBoxClass = new System.Windows.Forms.TextBox();
            this.labelProperty = new System.Windows.Forms.Label();
            this.textBoxProperty = new System.Windows.Forms.TextBox();
            this.bindingSourcePlaceholder = new System.Windows.Forms.BindingSource(this.components);
            this.panelUcHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePlaceholder)).BeginInit();
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
            this.labelUcHeader.Text = "Placeholder";
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
            this.textBoxName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourcePlaceholder, "Name", true));
            this.textBoxName.Location = new System.Drawing.Point(6, 39);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(250, 20);
            this.textBoxName.TabIndex = 3;
            this.textBoxName.MouseEnter += new System.EventHandler(this.TextBoxNameMouseEnter);
            this.textBoxName.MouseLeave += new System.EventHandler(this.TextBoxNameMouseLeave);
            // 
            // labelClass
            // 
            this.labelClass.AutoSize = true;
            this.labelClass.Location = new System.Drawing.Point(3, 62);
            this.labelClass.Name = "labelClass";
            this.labelClass.Size = new System.Drawing.Size(250, 13);
            this.labelClass.TabIndex = 4;
            this.labelClass.Text = "Class";
            // 
            // textBoxClass
            // 
            this.textBoxClass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxClass.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourcePlaceholder, "Class", true));
            this.textBoxClass.Location = new System.Drawing.Point(6, 78);
            this.textBoxClass.Name = "textBoxClass";
            this.textBoxClass.Size = new System.Drawing.Size(250, 20);
            this.textBoxClass.TabIndex = 5;
            this.textBoxClass.MouseEnter += new System.EventHandler(this.TextBoxClassMouseEnter);
            this.textBoxClass.MouseLeave += new System.EventHandler(this.TextBoxClassMouseLeave);
            // 
            // labelProperty
            // 
            this.labelProperty.AutoSize = true;
            this.labelProperty.Location = new System.Drawing.Point(3, 101);
            this.labelProperty.Name = "labelProperty";
            this.labelProperty.Size = new System.Drawing.Size(250, 13);
            this.labelProperty.TabIndex = 6;
            this.labelProperty.Text = "Property";
            // 
            // textBoxProperty
            // 
            this.textBoxProperty.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxProperty.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourcePlaceholder, "Property", true));
            this.textBoxProperty.Location = new System.Drawing.Point(6, 117);
            this.textBoxProperty.Name = "textBoxProperty";
            this.textBoxProperty.Size = new System.Drawing.Size(250, 20);
            this.textBoxProperty.TabIndex = 7;
            this.textBoxProperty.MouseEnter += new System.EventHandler(this.TextBoxPropertyMouseEnter);
            this.textBoxProperty.MouseLeave += new System.EventHandler(this.TextBoxPropertyMouseLeave);
            //
            // bindingSourcePlaceholder
            // 
            this.bindingSourcePlaceholder.DataSource = typeof(org.xpangen.Generator.Profile.Profile.Placeholder);
            // 
            // PlaceholderUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelUcHeader);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelClass);
            this.Controls.Add(this.textBoxClass);
            this.Controls.Add(this.labelProperty);
            this.Controls.Add(this.textBoxProperty);
            this.Name = "PlaceholderUserControl";
            this.Size = new System.Drawing.Size(259, 145);
            this.panelUcHeader.ResumeLayout(false);
            this.panelUcHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourcePlaceholder)).EndInit();
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
        private System.Windows.Forms.Label labelProperty;
        private System.Windows.Forms.TextBox textBoxProperty;
        private System.Windows.Forms.BindingSource bindingSourcePlaceholder;
    }
}
