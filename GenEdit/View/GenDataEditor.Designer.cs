namespace GenEdit.View
{
    partial class GenDataEditor
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.MoveToTopButton = new System.Windows.Forms.ToolStripButton();
            this.MoveUpButton = new System.Windows.Forms.ToolStripButton();
            this.MoveDownButton = new System.Windows.Forms.ToolStripButton();
            this.MoveToBottomButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AddItemButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveItemButton = new System.Windows.Forms.ToolStripButton();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.SaveItemChangesButton = new System.Windows.Forms.ToolStripButton();
            this.CancelItemChangesButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.DataEditorStatusLabel = new System.Windows.Forms.ToolStripLabel();
            this.GenDataDataGrid = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GenDataDataGrid)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer1.Panel2.Controls.Add(this.GenDataDataGrid);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(614, 283);
            this.splitContainer1.SplitterDistance = 204;
            this.splitContainer1.TabIndex = 0;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.MoveToTopButton,
            this.MoveUpButton,
            this.MoveDownButton,
            this.MoveToBottomButton,
            this.toolStripSeparator1,
            this.AddItemButton,
            this.RemoveItemButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 258);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(204, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // MoveToTopButton
            // 
            this.MoveToTopButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MoveToTopButton.Image = global::GenEdit.Properties.Resources.MoveToTop;
            this.MoveToTopButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveToTopButton.Name = "MoveToTopButton";
            this.MoveToTopButton.Size = new System.Drawing.Size(23, 22);
            this.MoveToTopButton.Text = "To top";
            this.MoveToTopButton.ToolTipText = "Move the item to the top of its list";
            // 
            // MoveUpButton
            // 
            this.MoveUpButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MoveUpButton.Image = global::GenEdit.Properties.Resources.MoveUp;
            this.MoveUpButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveUpButton.Name = "MoveUpButton";
            this.MoveUpButton.Size = new System.Drawing.Size(23, 22);
            this.MoveUpButton.Text = "Move up";
            this.MoveUpButton.ToolTipText = "Move the item up one place in its list";
            // 
            // MoveDownButton
            // 
            this.MoveDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MoveDownButton.Image = global::GenEdit.Properties.Resources.MoveDown;
            this.MoveDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveDownButton.Name = "MoveDownButton";
            this.MoveDownButton.Size = new System.Drawing.Size(23, 22);
            this.MoveDownButton.Text = "Move down";
            this.MoveDownButton.ToolTipText = "Move the item down one place in its list";
            // 
            // MoveToBottomButton
            // 
            this.MoveToBottomButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.MoveToBottomButton.Image = global::GenEdit.Properties.Resources.MoveToBottom;
            this.MoveToBottomButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.MoveToBottomButton.Name = "MoveToBottomButton";
            this.MoveToBottomButton.Size = new System.Drawing.Size(23, 22);
            this.MoveToBottomButton.Text = "Move to bottom";
            this.MoveToBottomButton.ToolTipText = "Move the item to the bottom of its list";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // AddItemButton
            // 
            this.AddItemButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.AddItemButton.Image = global::GenEdit.Properties.Resources.list_add_4;
            this.AddItemButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.AddItemButton.Name = "AddItemButton";
            this.AddItemButton.Size = new System.Drawing.Size(23, 22);
            this.AddItemButton.Text = "AddI item";
            this.AddItemButton.ToolTipText = "Add an item to the bottom of the current list";
            // 
            // RemoveItemButton
            // 
            this.RemoveItemButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.RemoveItemButton.Image = global::GenEdit.Properties.Resources.list_remove_4;
            this.RemoveItemButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.RemoveItemButton.Name = "RemoveItemButton";
            this.RemoveItemButton.Size = new System.Drawing.Size(23, 22);
            this.RemoveItemButton.Text = "Remove item";
            this.RemoveItemButton.ToolTipText = "Remove the selected item from its list";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 20);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(204, 263);
            this.treeView1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(204, 20);
            this.panel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Data navigator";
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveItemChangesButton,
            this.CancelItemChangesButton,
            this.toolStripSeparator2,
            this.DataEditorStatusLabel});
            this.toolStrip2.Location = new System.Drawing.Point(0, 258);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(406, 25);
            this.toolStrip2.TabIndex = 3;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // SaveItemChangesButton
            // 
            this.SaveItemChangesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.SaveItemChangesButton.Image = global::GenEdit.Properties.Resources.dialog_ok_apply_2;
            this.SaveItemChangesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.SaveItemChangesButton.Name = "SaveItemChangesButton";
            this.SaveItemChangesButton.Size = new System.Drawing.Size(23, 22);
            this.SaveItemChangesButton.Text = "Save changes";
            this.SaveItemChangesButton.ToolTipText = "Save the changes to the current item";
            // 
            // CancelItemChangesButton
            // 
            this.CancelItemChangesButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.CancelItemChangesButton.Image = global::GenEdit.Properties.Resources.dialog_cancel_2;
            this.CancelItemChangesButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.CancelItemChangesButton.Name = "CancelItemChangesButton";
            this.CancelItemChangesButton.Size = new System.Drawing.Size(23, 22);
            this.CancelItemChangesButton.Text = "Cancel changes";
            this.CancelItemChangesButton.ToolTipText = "Cancel any changes made to the selected item";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // DataEditorStatusLabel
            // 
            this.DataEditorStatusLabel.Name = "DataEditorStatusLabel";
            this.DataEditorStatusLabel.Size = new System.Drawing.Size(0, 22);
            // 
            // GenDataDataGrid
            // 
            this.GenDataDataGrid.AllowUserToAddRows = false;
            this.GenDataDataGrid.AllowUserToDeleteRows = false;
            this.GenDataDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GenDataDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GenDataDataGrid.Location = new System.Drawing.Point(0, 20);
            this.GenDataDataGrid.Name = "GenDataDataGrid";
            this.GenDataDataGrid.Size = new System.Drawing.Size(406, 263);
            this.GenDataDataGrid.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(406, 20);
            this.panel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Data editor";
            // 
            // GenDataEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "GenDataEditor";
            this.Size = new System.Drawing.Size(614, 283);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GenDataDataGrid)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridView GenDataDataGrid;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton MoveToTopButton;
        private System.Windows.Forms.ToolStripButton MoveUpButton;
        private System.Windows.Forms.ToolStripButton MoveDownButton;
        private System.Windows.Forms.ToolStripButton MoveToBottomButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton AddItemButton;
        private System.Windows.Forms.ToolStripButton RemoveItemButton;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton SaveItemChangesButton;
        private System.Windows.Forms.ToolStripButton CancelItemChangesButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel DataEditorStatusLabel;

    }
}
