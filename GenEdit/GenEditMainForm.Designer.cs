namespace GenEdit
{
    partial class GenEditMainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenEditMainForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.genDataEditor1 = new GenEdit.View.GenDataEditor();
            this.genProfileEditor1 = new GenEdit.View.GenProfileEditor();
            this.genLibrary1 = new GenEdit.View.GenLibrary();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.genDataEditor1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.genProfileEditor1);
            this.splitContainer1.Size = new System.Drawing.Size(720, 614);
            this.splitContainer1.SplitterDistance = 156;
            this.splitContainer1.TabIndex = 0;
            // 
            // genDataEditor1
            // 
            this.genDataEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.genDataEditor1.GenDataEditorViewModel = null;
            this.genDataEditor1.Location = new System.Drawing.Point(0, 0);
            this.genDataEditor1.Name = "genDataEditor1";
            this.genDataEditor1.Size = new System.Drawing.Size(720, 156);
            this.genDataEditor1.TabIndex = 0;
            // 
            // genProfileEditor1
            // 
            this.genProfileEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.genProfileEditor1.GenDataEditorViewModel = null;
            this.genProfileEditor1.Location = new System.Drawing.Point(0, 0);
            this.genProfileEditor1.Name = "genProfileEditor1";
            this.genProfileEditor1.Size = new System.Drawing.Size(720, 454);
            this.genProfileEditor1.TabIndex = 0;
            // 
            // genLibrary1
            // 
            this.genLibrary1.Dock = System.Windows.Forms.DockStyle.Right;
            this.genLibrary1.GenDataEditorViewModel = null;
            this.genLibrary1.Location = new System.Drawing.Point(720, 0);
            this.genLibrary1.Name = "genLibrary1";
            this.genLibrary1.Size = new System.Drawing.Size(210, 614);
            this.genLibrary1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.splitContainer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(720, 614);
            this.panel1.TabIndex = 2;
            // 
            // GenEditMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(930, 614);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.genLibrary1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GenEditMainForm";
            this.Text = "Generator editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GenEditMainForm_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private View.GenDataEditor genDataEditor1;
        private View.GenProfileEditor genProfileEditor1;
        private View.GenLibrary genLibrary1;
        private System.Windows.Forms.Panel panel1;

    }
}

