﻿using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using GenEdit.UserControls;

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
            this.saveGeneratedFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStripLibrary = new System.Windows.Forms.ToolStrip();
            this.buttonNew = new System.Windows.Forms.ToolStripButton();
            this.buttonSave = new System.Windows.Forms.ToolStripButton();
            this.buttonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.buttonClose = new System.Windows.Forms.ToolStripButton();
            this.buttonGenerate = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxFileGroup = new System.Windows.Forms.ComboBox();
            this.fileGroupUserControl1 = new GenEdit.UserControls.FileGroupUserControl();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStripLibrary.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel1);
            this.panel2.Controls.Add(this.toolStripLibrary);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.comboBoxFileGroup);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(303, 92);
            this.panel2.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(303, 20);
            this.panel1.TabIndex = 6;
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
            this.toolStripLibrary.TabIndex = 5;
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
            this.label2.Location = new System.Drawing.Point(6, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Data file";
            // 
            // comboBoxFileGroup
            // 
            this.comboBoxFileGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFileGroup.FormattingEnabled = true;
            this.comboBoxFileGroup.Location = new System.Drawing.Point(6, 39);
            this.comboBoxFileGroup.Name = "comboBoxFileGroup";
            this.comboBoxFileGroup.Size = new System.Drawing.Size(294, 21);
            this.comboBoxFileGroup.TabIndex = 3;
            this.comboBoxFileGroup.SelectedValueChanged += new System.EventHandler(this.comboBoxFileGroup_SelectedValueChanged);
            // 
            // fileGroupUserControl1
            // 
            this.fileGroupUserControl1.AutoSize = true;
            this.fileGroupUserControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.fileGroupUserControl1.BaseFile = null;
            this.fileGroupUserControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.fileGroupUserControl1.FileGroup = null;
            this.fileGroupUserControl1.Location = new System.Drawing.Point(0, 92);
            this.fileGroupUserControl1.Name = "fileGroupUserControl1";
            this.fileGroupUserControl1.Profile = null;
            this.fileGroupUserControl1.Size = new System.Drawing.Size(303, 259);
            this.fileGroupUserControl1.TabIndex = 2;
            // 
            // GenLibrary
            // 
            this.Controls.Add(this.fileGroupUserControl1);
            this.Controls.Add(this.panel2);
            this.Name = "GenLibrary";
            this.Size = new System.Drawing.Size(303, 555);
            this.Load += new System.EventHandler(this.GenLibrary_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStripLibrary.ResumeLayout(false);
            this.toolStripLibrary.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SaveFileDialog saveGeneratedFileDialog;
        private FileGroupUserControl fileGroupUserControl1;
        private Panel panel2;
        private Panel panel1;
        private Label label1;
        private ToolStrip toolStripLibrary;
        private ToolStripButton buttonNew;
        private ToolStripButton buttonSave;
        private ToolStripButton buttonSaveAs;
        private ToolStripButton buttonClose;
        private ToolStripButton buttonGenerate;
        private Label label2;
        private ComboBox comboBoxFileGroup;
    }
}
