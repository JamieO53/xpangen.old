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
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxProfile = new System.Windows.Forms.ComboBox();
            this.bindingSourceFileGroup = new System.Windows.Forms.BindingSource(this.components);
            this.bindingSourceProfile = new System.Windows.Forms.BindingSource(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxBaseFile = new System.Windows.Forms.ComboBox();
            this.bindingSourceBaseFile = new System.Windows.Forms.BindingSource(this.components);
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.textBoxGenerated = new System.Windows.Forms.TextBox();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFileGroup)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceProfile)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBaseFile)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 141);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 7;
            this.label7.Text = "Profile";
            // 
            // comboBoxProfile
            // 
            this.comboBoxProfile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxProfile.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bindingSourceFileGroup, "Profile", true));
            this.comboBoxProfile.DataSource = this.bindingSourceProfile;
            this.comboBoxProfile.DisplayMember = "Name";
            this.comboBoxProfile.FormattingEnabled = true;
            this.comboBoxProfile.Location = new System.Drawing.Point(6, 157);
            this.comboBoxProfile.Name = "comboBoxProfile";
            this.comboBoxProfile.Size = new System.Drawing.Size(250, 21);
            this.comboBoxProfile.TabIndex = 6;
            this.comboBoxProfile.ValueMember = "Name";
            this.comboBoxProfile.SelectedValueChanged += new System.EventHandler(this.comboBoxProfile_SelectedValueChanged);
            // 
            // bindingSourceFileGroup
            // 
            this.bindingSourceFileGroup.DataSource = typeof(org.xpangen.Generator.Data.Model.Settings.FileGroup);
            // 
            // bindingSourceProfile
            // 
            this.bindingSourceProfile.DataSource = typeof(org.xpangen.Generator.Data.Model.Settings.Profile);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Base file";
            // 
            // comboBoxBaseFile
            // 
            this.comboBoxBaseFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxBaseFile.DataBindings.Add(new System.Windows.Forms.Binding("SelectedValue", this.bindingSourceFileGroup, "BaseFileName", true));
            this.comboBoxBaseFile.DataSource = this.bindingSourceBaseFile;
            this.comboBoxBaseFile.DisplayMember = "Name";
            this.comboBoxBaseFile.FormattingEnabled = true;
            this.comboBoxBaseFile.Location = new System.Drawing.Point(6, 117);
            this.comboBoxBaseFile.Name = "comboBoxBaseFile";
            this.comboBoxBaseFile.Size = new System.Drawing.Size(250, 21);
            this.comboBoxBaseFile.TabIndex = 2;
            this.comboBoxBaseFile.ValueMember = "Name";
            this.comboBoxBaseFile.SelectedValueChanged += new System.EventHandler(this.comboBoxBaseFile_SelectedValueChanged);
            // 
            // bindingSourceBaseFile
            // 
            this.bindingSourceBaseFile.DataSource = typeof(org.xpangen.Generator.Data.Model.Settings.BaseFile);
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "File path";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 181);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 3;
            this.label6.Text = "Generated file";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "File name";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(259, 20);
            this.panel1.TabIndex = 12;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "File Group";
            // 
            // FileGroupUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.comboBoxProfile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBoxBaseFile);
            this.Controls.Add(this.textBoxFilePath);
            this.Controls.Add(this.textBoxGenerated);
            this.Controls.Add(this.textBoxFileName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Name = "FileGroupUserControl";
            this.Size = new System.Drawing.Size(259, 225);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFileGroup)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceProfile)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceBaseFile)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxProfile;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxBaseFile;
        private System.Windows.Forms.TextBox textBoxFilePath;
        private System.Windows.Forms.TextBox textBoxGenerated;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource bindingSourceFileGroup;
        private System.Windows.Forms.BindingSource bindingSourceBaseFile;
        private System.Windows.Forms.BindingSource bindingSourceProfile;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}
