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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.genDataEditor1 = new GenEdit.View.GenDataEditor();
            this.genProfileEditor1 = new GenEdit.View.GenProfileEditor();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
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
            this.splitContainer1.Size = new System.Drawing.Size(617, 503);
            this.splitContainer1.SplitterDistance = 205;
            this.splitContainer1.TabIndex = 0;
            // 
            // genDataEditor1
            // 
            this.genDataEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.genDataEditor1.Location = new System.Drawing.Point(0, 0);
            this.genDataEditor1.Name = "genDataEditor1";
            this.genDataEditor1.Size = new System.Drawing.Size(617, 205);
            this.genDataEditor1.TabIndex = 0;
            // 
            // genProfileEditor1
            // 
            this.genProfileEditor1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.genProfileEditor1.Location = new System.Drawing.Point(0, 0);
            this.genProfileEditor1.Name = "genProfileEditor1";
            this.genProfileEditor1.Size = new System.Drawing.Size(617, 294);
            this.genProfileEditor1.TabIndex = 0;
            // 
            // GenEditMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 503);
            this.Controls.Add(this.splitContainer1);
            this.Name = "GenEditMainForm";
            this.Text = "Generator editor";
            this.Load += new System.EventHandler(this.GenEditMainForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private View.GenDataEditor genDataEditor1;
        private View.GenProfileEditor genProfileEditor1;

    }
}

