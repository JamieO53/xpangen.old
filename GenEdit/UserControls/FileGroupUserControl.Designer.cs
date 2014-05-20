namespace GenEdit.UserControls
{
    partial class FileGroupUserControl
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
            this.labelFileName = new System.Windows.Forms.Label();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.labelFilePath = new System.Windows.Forms.Label();
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.labelBaseFileName = new System.Windows.Forms.Label();
            this.comboBoxBaseFileName = new System.Windows.Forms.ComboBox();
            this.labelProfile = new System.Windows.Forms.Label();
            this.comboBoxProfile = new System.Windows.Forms.ComboBox();
            this.labelGeneratedFile = new System.Windows.Forms.Label();
            this.textBoxGeneratedFile = new System.Windows.Forms.TextBox();
            this.bindingSourceFileGroup = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFileGroup)).BeginInit();
            this.panelUcHeader.SuspendLayout();
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
            this.labelUcHeader.Text = "File group";
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
            this.textBoxName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFileGroup, "Name", true));
            this.textBoxName.Location = new System.Drawing.Point(6, 39);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(250, 20);
            this.textBoxName.TabIndex = 3;
            this.textBoxName.MouseEnter += new System.EventHandler(this.TextBoxNameMouseEnter);
            this.textBoxName.MouseLeave += new System.EventHandler(this.TextBoxNameMouseLeave);
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(3, 62);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(250, 13);
            this.labelFileName.TabIndex = 4;
            this.labelFileName.Text = "File name";
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFileName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFileGroup, "FileName", true));
            this.textBoxFileName.Location = new System.Drawing.Point(6, 78);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(250, 20);
            this.textBoxFileName.TabIndex = 5;
            this.textBoxFileName.MouseEnter += new System.EventHandler(this.TextBoxFileNameMouseEnter);
            this.textBoxFileName.MouseLeave += new System.EventHandler(this.TextBoxFileNameMouseLeave);
            // 
            // labelFilePath
            // 
            this.labelFilePath.AutoSize = true;
            this.labelFilePath.Location = new System.Drawing.Point(3, 101);
            this.labelFilePath.Name = "labelFilePath";
            this.labelFilePath.Size = new System.Drawing.Size(250, 13);
            this.labelFilePath.TabIndex = 6;
            this.labelFilePath.Text = "File path";
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilePath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFileGroup, "FilePath", true));
            this.textBoxFilePath.Location = new System.Drawing.Point(6, 117);
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(250, 20);
            this.textBoxFilePath.TabIndex = 7;
            this.textBoxFilePath.MouseEnter += new System.EventHandler(this.TextBoxFilePathMouseEnter);
            this.textBoxFilePath.MouseLeave += new System.EventHandler(this.TextBoxFilePathMouseLeave);
            // 
            // labelBaseFileName
            // 
            this.labelBaseFileName.AutoSize = true;
            this.labelBaseFileName.Location = new System.Drawing.Point(3, 140);
            this.labelBaseFileName.Name = "labelBaseFileName";
            this.labelBaseFileName.Size = new System.Drawing.Size(250, 13);
            this.labelBaseFileName.TabIndex = 8;
            this.labelBaseFileName.Text = "Base file name";
            // 
            // comboBoxBaseFileName
            // 
            this.comboBoxBaseFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBaseFileName.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bindingSourceFileGroup, "BaseFileName", true));
            this.comboBoxBaseFileName.DisplayMember = "Name";
            this.comboBoxBaseFileName.FormattingEnabled = true;
            this.comboBoxBaseFileName.Location = new System.Drawing.Point(6, 156);
            this.comboBoxBaseFileName.Name = "comboBoxBaseFileName";
            this.comboBoxBaseFileName.Size = new System.Drawing.Size(250, 21);
            this.comboBoxBaseFileName.TabIndex = 9;
            this.comboBoxBaseFileName.ValueMember = "Name";
            this.comboBoxBaseFileName.SelectedValueChanged += new System.EventHandler(this.ComboBoxBaseFileNameSelectedValueChanged);
            this.comboBoxBaseFileName.MouseEnter += new System.EventHandler(this.ComboBoxBaseFileNameMouseEnter);
            this.comboBoxBaseFileName.MouseLeave += new System.EventHandler(this.ComboBoxBaseFileNameMouseLeave);
            // 
            // labelProfile
            // 
            this.labelProfile.AutoSize = true;
            this.labelProfile.Location = new System.Drawing.Point(3, 180);
            this.labelProfile.Name = "labelProfile";
            this.labelProfile.Size = new System.Drawing.Size(250, 13);
            this.labelProfile.TabIndex = 10;
            this.labelProfile.Text = "Profile";
            // 
            // comboBoxProfile
            // 
            this.comboBoxProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxProfile.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bindingSourceFileGroup, "Profile", true));
            this.comboBoxProfile.DisplayMember = "Name";
            this.comboBoxProfile.FormattingEnabled = true;
            this.comboBoxProfile.Location = new System.Drawing.Point(6, 196);
            this.comboBoxProfile.Name = "comboBoxProfile";
            this.comboBoxProfile.Size = new System.Drawing.Size(250, 21);
            this.comboBoxProfile.TabIndex = 11;
            this.comboBoxProfile.ValueMember = "Name";
            this.comboBoxProfile.SelectedValueChanged += new System.EventHandler(this.ComboBoxProfileSelectedValueChanged);
            this.comboBoxProfile.MouseEnter += new System.EventHandler(this.ComboBoxProfileMouseEnter);
            this.comboBoxProfile.MouseLeave += new System.EventHandler(this.ComboBoxProfileMouseLeave);
            // 
            // labelGeneratedFile
            // 
            this.labelGeneratedFile.AutoSize = true;
            this.labelGeneratedFile.Location = new System.Drawing.Point(3, 220);
            this.labelGeneratedFile.Name = "labelGeneratedFile";
            this.labelGeneratedFile.Size = new System.Drawing.Size(250, 13);
            this.labelGeneratedFile.TabIndex = 12;
            this.labelGeneratedFile.Text = "Generated file";
            // 
            // textBoxGeneratedFile
            // 
            this.textBoxGeneratedFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGeneratedFile.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFileGroup, "GeneratedFile", true));
            this.textBoxGeneratedFile.Location = new System.Drawing.Point(6, 236);
            this.textBoxGeneratedFile.Name = "textBoxGeneratedFile";
            this.textBoxGeneratedFile.Size = new System.Drawing.Size(250, 20);
            this.textBoxGeneratedFile.TabIndex = 13;
            this.textBoxGeneratedFile.MouseEnter += new System.EventHandler(this.TextBoxGeneratedFileMouseEnter);
            this.textBoxGeneratedFile.MouseLeave += new System.EventHandler(this.TextBoxGeneratedFileMouseLeave);
            //
            // bindingSourceFileGroup
            // 
            this.bindingSourceFileGroup.DataSource = typeof(org.xpangen.Generator.Data.Model.Settings.FileGroup);
            // 
            // FileGroupUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelUcHeader);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelFileName);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.labelFilePath);
            this.Controls.Add(this.textBoxFilePath);
            this.Controls.Add(this.labelBaseFileName);
            this.Controls.Add(this.comboBoxBaseFileName);
            this.Controls.Add(this.labelProfile);
            this.Controls.Add(this.comboBoxProfile);
            this.Controls.Add(this.labelGeneratedFile);
            this.Controls.Add(this.textBoxGeneratedFile);
            this.Name = "FileGroupUserControl";
            this.Size = new System.Drawing.Size(259, 264);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFileGroup)).EndInit();
            this.panelUcHeader.ResumeLayout(false);
            this.panelUcHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelUcHeader;
        private System.Windows.Forms.Label labelUcHeader;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label labelFilePath;
        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.Label labelBaseFileName;
        private System.Windows.Forms.ComboBox comboBoxBaseFileName;
        private System.Windows.Forms.Label labelProfile;
        private System.Windows.Forms.ComboBox comboBoxProfile;
        private System.Windows.Forms.Label labelGeneratedFile;
        private System.Windows.Forms.TextBox textBoxGeneratedFile;
        private System.Windows.Forms.BindingSource bindingSourceFileGroup;
    }
}
