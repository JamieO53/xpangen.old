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
            this.labelFileName = new System.Windows.Forms.Label();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.labelFilePath = new System.Windows.Forms.Label();
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.labelBaseFile = new System.Windows.Forms.Label();
            this.comboBoxBaseFile = new System.Windows.Forms.ComboBox();
            this.labelProfile = new System.Windows.Forms.Label();
            this.comboBoxProfile = new System.Windows.Forms.ComboBox();
            this.labelGenerated = new System.Windows.Forms.Label();
            this.textBoxGenerated = new System.Windows.Forms.TextBox();
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
            this.panelUcHeader.TabIndex = 12;
            // 
            // labelUcHeader
            // 
            this.labelUcHeader.AutoSize = true;
            this.labelUcHeader.Dock = System.Windows.Forms.DockStyle.Left;
            this.labelUcHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUcHeader.Location = new System.Drawing.Point(0, 0);
            this.labelUcHeader.Name = "labelUcHeader";
            this.labelUcHeader.Size = new System.Drawing.Size(74, 17);
            this.labelUcHeader.TabIndex = 0;
            this.labelUcHeader.Text = "File Group";
            // 
            // labelFileName
            // 
            this.labelFileName.AutoSize = true;
            this.labelFileName.Location = new System.Drawing.Point(3, 23);
            this.labelFileName.Name = "labelFileName";
            this.labelFileName.Size = new System.Drawing.Size(52, 13);
            this.labelFileName.TabIndex = 5;
            this.labelFileName.Text = "File name";
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFileName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFileGroup, "FileName", true));
            this.textBoxFileName.Location = new System.Drawing.Point(6, 39);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(250, 20);
            this.textBoxFileName.TabIndex = 8;
            // 
            // labelFilePath
            // 
            this.labelFilePath.AutoSize = true;
            this.labelFilePath.Location = new System.Drawing.Point(3, 62);
            this.labelFilePath.Name = "labelFilePath";
            this.labelFilePath.Size = new System.Drawing.Size(47, 13);
            this.labelFilePath.TabIndex = 4;
            this.labelFilePath.Text = "File path";
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFilePath.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFileGroup, "FilePath", true));
            this.textBoxFilePath.Location = new System.Drawing.Point(6, 78);
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(250, 20);
            this.textBoxFilePath.TabIndex = 10;
            // 
            // labelBaseFile
            // 
            this.labelBaseFile.AutoSize = true;
            this.labelBaseFile.Location = new System.Drawing.Point(3, 101);
            this.labelBaseFile.Name = "labelBaseFile";
            this.labelBaseFile.Size = new System.Drawing.Size(47, 13);
            this.labelBaseFile.TabIndex = 9;
            this.labelBaseFile.Text = "Base file";
            // 
            // comboBoxBaseFile
            // 
            this.comboBoxBaseFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBaseFile.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bindingSourceFileGroup, "BaseFileName", true));
            this.comboBoxBaseFile.DisplayMember = "Name";
            this.comboBoxBaseFile.FormattingEnabled = true;
            this.comboBoxBaseFile.Location = new System.Drawing.Point(6, 117);
            this.comboBoxBaseFile.Name = "comboBoxBaseFile";
            this.comboBoxBaseFile.Size = new System.Drawing.Size(250, 21);
            this.comboBoxBaseFile.TabIndex = 2;
            this.comboBoxBaseFile.ValueMember = "Name";
            this.comboBoxBaseFile.SelectedValueChanged += new System.EventHandler(this.ComboBoxBaseFileSelectedValueChanged);
            // 
            // labelProfile
            // 
            this.labelProfile.AutoSize = true;
            this.labelProfile.Location = new System.Drawing.Point(3, 141);
            this.labelProfile.Name = "labelProfile";
            this.labelProfile.Size = new System.Drawing.Size(36, 13);
            this.labelProfile.TabIndex = 7;
            this.labelProfile.Text = "Profile";
            // 
            // comboBoxProfile
            // 
            this.comboBoxProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxProfile.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bindingSourceFileGroup, "Profile", true));
            this.comboBoxProfile.DisplayMember = "Name";
            this.comboBoxProfile.FormattingEnabled = true;
            this.comboBoxProfile.Location = new System.Drawing.Point(6, 157);
            this.comboBoxProfile.Name = "comboBoxProfile";
            this.comboBoxProfile.Size = new System.Drawing.Size(250, 21);
            this.comboBoxProfile.TabIndex = 6;
            this.comboBoxProfile.ValueMember = "Name";
            this.comboBoxProfile.SelectedValueChanged += new System.EventHandler(this.ComboBoxProfileSelectedValueChanged);
            // 
            // labelGenerated
            // 
            this.labelGenerated.AutoSize = true;
            this.labelGenerated.Location = new System.Drawing.Point(3, 181);
            this.labelGenerated.Name = "labelGenerated";
            this.labelGenerated.Size = new System.Drawing.Size(73, 13);
            this.labelGenerated.TabIndex = 3;
            this.labelGenerated.Text = "Generated file";
            // 
            // textBoxGenerated
            // 
            this.textBoxGenerated.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxGenerated.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFileGroup, "Generated", true));
            this.textBoxGenerated.Location = new System.Drawing.Point(6, 197);
            this.textBoxGenerated.Name = "textBoxGenerated";
            this.textBoxGenerated.Size = new System.Drawing.Size(250, 20);
            this.textBoxGenerated.TabIndex = 11;
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
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.labelFilePath);
            this.Controls.Add(this.textBoxFilePath);
            this.Controls.Add(this.labelBaseFile);
            this.Controls.Add(this.comboBoxBaseFile);
            this.Controls.Add(this.labelProfile);
            this.Controls.Add(this.comboBoxProfile);
            this.Controls.Add(this.labelGenerated);
            this.Controls.Add(this.textBoxGenerated);
            this.Name = "FileGroupUserControl";
            this.Size = new System.Drawing.Size(259, 225);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFileGroup)).EndInit();
            this.panelUcHeader.ResumeLayout(false);
            this.panelUcHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelUcHeader;
        private System.Windows.Forms.Label labelUcHeader;
        private System.Windows.Forms.Label labelFileName;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label labelFilePath;
        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.Label labelBaseFile;
        private System.Windows.Forms.ComboBox comboBoxBaseFile;
        private System.Windows.Forms.Label labelProfile;
        private System.Windows.Forms.ComboBox comboBoxProfile;
        private System.Windows.Forms.Label labelGenerated;
        private System.Windows.Forms.TextBox textBoxGenerated;
        private System.Windows.Forms.BindingSource bindingSourceFileGroup;
    }
}
