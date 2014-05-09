using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GenEdit.View
{
    partial class GenLibrary
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenLibrary));
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStripLibrary = new System.Windows.Forms.ToolStrip();
            this.buttonNew = new System.Windows.Forms.ToolStripButton();
            this.buttonSave = new System.Windows.Forms.ToolStripButton();
            this.buttonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.buttonClose = new System.Windows.Forms.ToolStripButton();
            this.buttonGenerate = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxFileGroup = new System.Windows.Forms.ComboBox();
            this.fileGroupUserControl1 = new GenEdit.View.FileGroupUserControl();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxProfile = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxBaseFile = new System.Windows.Forms.ComboBox();
            this.textBoxFilePath = new System.Windows.Forms.TextBox();
            this.textBoxGenerated = new System.Windows.Forms.TextBox();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.saveGeneratedFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStripLibrary.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(303, 20);
            this.panel1.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Library";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 20);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.toolStripLibrary);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.comboBoxFileGroup);
            this.splitContainer1.Panel1.Resize += new System.EventHandler(this.splitContainer1_Panel1_Resize);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.fileGroupUserControl1);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxProfile);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.comboBoxBaseFile);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxFilePath);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxGenerated);
            this.splitContainer1.Panel2.Controls.Add(this.textBoxFileName);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Size = new System.Drawing.Size(303, 535);
            this.splitContainer1.SplitterDistance = 92;
            this.splitContainer1.TabIndex = 3;
            // 
            // toolStripLibrary
            // 
            this.toolStripLibrary.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripLibrary.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonNew,
            this.buttonSave,
            this.buttonSaveAs,
            this.buttonClose,
            this.buttonGenerate});
            this.toolStripLibrary.Location = new System.Drawing.Point(0, 67);
            this.toolStripLibrary.Name = "toolStripLibrary";
            this.toolStripLibrary.Size = new System.Drawing.Size(303, 25);
            this.toolStripLibrary.TabIndex = 2;
            this.toolStripLibrary.Text = "toolStrip1";
            // 
            // buttonNew
            // 
            this.buttonNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonNew.Image = ((System.Drawing.Image)(resources.GetObject("buttonNew.Image")));
            this.buttonNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonNew.Name = "buttonNew";
            this.buttonNew.Size = new System.Drawing.Size(23, 22);
            this.buttonNew.Text = "toolStripButton2";
            this.buttonNew.ToolTipText = "Create a new data file. Fill in the details below, and Save.";
            this.buttonNew.Click += new System.EventHandler(this.buttonNew_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonSave.Image = ((System.Drawing.Image)(resources.GetObject("buttonSave.Image")));
            this.buttonSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(23, 22);
            this.buttonSave.Text = "toolStripButton3";
            this.buttonSave.ToolTipText = "Save the data file";
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonSaveAs
            // 
            this.buttonSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonSaveAs.Image = ((System.Drawing.Image)(resources.GetObject("buttonSaveAs.Image")));
            this.buttonSaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonSaveAs.Name = "buttonSaveAs";
            this.buttonSaveAs.Size = new System.Drawing.Size(23, 22);
            this.buttonSaveAs.Text = "toolStripButton4";
            this.buttonSaveAs.ToolTipText = "Save the data file as...";
            // 
            // buttonClose
            // 
            this.buttonClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonClose.Image = ((System.Drawing.Image)(resources.GetObject("buttonClose.Image")));
            this.buttonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(23, 22);
            this.buttonClose.Text = "Close";
            this.buttonClose.ToolTipText = "Close the data file";
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonGenerate
            // 
            this.buttonGenerate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonGenerate.Image = ((System.Drawing.Image)(resources.GetObject("buttonGenerate.Image")));
            this.buttonGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonGenerate.Name = "buttonGenerate";
            this.buttonGenerate.Size = new System.Drawing.Size(23, 22);
            this.buttonGenerate.Text = "Generate";
            this.buttonGenerate.ToolTipText = "Generate the selected profile";
            this.buttonGenerate.Click += new System.EventHandler(this.buttonGenerate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Data file";
            // 
            // comboBoxFileGroup
            // 
            this.comboBoxFileGroup.FormattingEnabled = true;
            this.comboBoxFileGroup.Location = new System.Drawing.Point(19, 29);
            this.comboBoxFileGroup.Name = "comboBoxFileGroup";
            this.comboBoxFileGroup.Size = new System.Drawing.Size(252, 21);
            this.comboBoxFileGroup.TabIndex = 0;
            this.comboBoxFileGroup.SelectedValueChanged += new System.EventHandler(this.comboBoxFileGroup_SelectedValueChanged);
            // 
            // fileGroupUserControl1
            // 
            this.fileGroupUserControl1.AutoSize = true;
            this.fileGroupUserControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fileGroupUserControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.fileGroupUserControl1.Location = new System.Drawing.Point(0, 219);
            this.fileGroupUserControl1.Name = "fileGroupUserControl1";
            this.fileGroupUserControl1.Profile = null;
            this.fileGroupUserControl1.Size = new System.Drawing.Size(303, 220);
            this.fileGroupUserControl1.TabIndex = 2;
            this.fileGroupUserControl1.ViewModel = null;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 132);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(36, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Profile";
            // 
            // comboBoxProfile
            // 
            this.comboBoxProfile.FormattingEnabled = true;
            this.comboBoxProfile.Location = new System.Drawing.Point(19, 148);
            this.comboBoxProfile.Name = "comboBoxProfile";
            this.comboBoxProfile.Size = new System.Drawing.Size(252, 21);
            this.comboBoxProfile.TabIndex = 0;
            this.comboBoxProfile.SelectedValueChanged += new System.EventHandler(this.comboBoxProfile_SelectedValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 92);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Base file";
            // 
            // comboBoxBaseFile
            // 
            this.comboBoxBaseFile.FormattingEnabled = true;
            this.comboBoxBaseFile.Location = new System.Drawing.Point(19, 108);
            this.comboBoxBaseFile.Name = "comboBoxBaseFile";
            this.comboBoxBaseFile.Size = new System.Drawing.Size(252, 21);
            this.comboBoxBaseFile.TabIndex = 0;
            this.comboBoxBaseFile.SelectedValueChanged += new System.EventHandler(this.comboBoxBaseFile_SelectedValueChanged);
            // 
            // textBoxFilePath
            // 
            this.textBoxFilePath.Location = new System.Drawing.Point(19, 69);
            this.textBoxFilePath.Name = "textBoxFilePath";
            this.textBoxFilePath.Size = new System.Drawing.Size(252, 20);
            this.textBoxFilePath.TabIndex = 1;
            // 
            // textBoxGenerated
            // 
            this.textBoxGenerated.Location = new System.Drawing.Point(19, 189);
            this.textBoxGenerated.Name = "textBoxGenerated";
            this.textBoxGenerated.Size = new System.Drawing.Size(252, 20);
            this.textBoxGenerated.TabIndex = 1;
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.Location = new System.Drawing.Point(19, 29);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.Size = new System.Drawing.Size(252, 20);
            this.textBoxFileName.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "File path";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Generated file";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "File name";
            // 
            // GenLibrary
            // 
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Name = "GenLibrary";
            this.Size = new System.Drawing.Size(303, 555);
            this.Load += new System.EventHandler(this.GenLibrary_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.toolStripLibrary.ResumeLayout(false);
            this.toolStripLibrary.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Panel panel1;
        private Label label1;
        private SplitContainer splitContainer1;
        private Label label2;
        private ComboBox comboBoxFileGroup;
        private TextBox textBoxFilePath;
        private TextBox textBoxFileName;
        private Label label4;
        private Label label3;
        private Label label5;
        private ComboBox comboBoxBaseFile;
        private TextBox textBoxGenerated;
        private Label label6;
        private Label label7;
        private ComboBox comboBoxProfile;
        private ToolStrip toolStripLibrary;
        private ToolStripButton buttonNew;
        private ToolStripButton buttonSave;
        private ToolStripButton buttonSaveAs;
        private ToolStripButton buttonClose;
        private ToolStripButton buttonGenerate;
        private SaveFileDialog saveGeneratedFileDialog;
        private FileGroupUserControl fileGroupUserControl1;
    }
}
