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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenDataEditor));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.DataNavigatorTreeView = new System.Windows.Forms.TreeView();
            this.DataNavigatorImageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.MoveToTopButton = new System.Windows.Forms.ToolStripButton();
            this.MoveUpButton = new System.Windows.Forms.ToolStripButton();
            this.MoveDownButton = new System.Windows.Forms.ToolStripButton();
            this.MoveToBottomButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.AddItemButton = new System.Windows.Forms.ToolStripButton();
            this.RemoveItemButton = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.GenDataDataGrid = new System.Windows.Forms.DataGridView();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.SaveItemChangesButton = new System.Windows.Forms.ToolStripButton();
            this.CancelItemChangesButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.DataEditorStatusLabel = new System.Windows.Forms.ToolStripLabel();
            this.DataEditorHintLabel = new System.Windows.Forms.ToolStripLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GenDataDataGrid)).BeginInit();
            this.toolStrip2.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.panel3);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel4);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Size = new System.Drawing.Size(614, 283);
            this.splitContainer1.SplitterDistance = 204;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.DataNavigatorTreeView);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 20);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(204, 238);
            this.panel3.TabIndex = 4;
            // 
            // DataNavigatorTreeView
            // 
            this.DataNavigatorTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataNavigatorTreeView.ImageIndex = 0;
            this.DataNavigatorTreeView.ImageList = this.DataNavigatorImageList;
            this.DataNavigatorTreeView.Location = new System.Drawing.Point(0, 0);
            this.DataNavigatorTreeView.Name = "DataNavigatorTreeView";
            this.DataNavigatorTreeView.SelectedImageIndex = 0;
            this.DataNavigatorTreeView.Size = new System.Drawing.Size(204, 238);
            this.DataNavigatorTreeView.TabIndex = 2;
            this.DataNavigatorTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.DataNavigatorTreeView_BeforeSelect);
            this.DataNavigatorTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.DataNavigatorTreeView_AfterSelect);
            // 
            // DataNavigatorImageList
            // 
            this.DataNavigatorImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("DataNavigatorImageList.ImageStream")));
            this.DataNavigatorImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.DataNavigatorImageList.Images.SetKeyName(0, "DataBrowserSelection.png");
            this.DataNavigatorImageList.Images.SetKeyName(1, "DataBrowserClass.png");
            this.DataNavigatorImageList.Images.SetKeyName(2, "DataBrowserSubClass.png");
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
            this.MoveToTopButton.Click += new System.EventHandler(this.MoveToTopButton_Click);
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
            this.MoveUpButton.Click += new System.EventHandler(this.MoveUpButton_Click);
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
            this.MoveDownButton.Click += new System.EventHandler(this.MoveDownButton_Click);
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
            this.MoveToBottomButton.Click += new System.EventHandler(this.MoveToBottomButton_Click);
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
            this.AddItemButton.Click += new System.EventHandler(this.AddItemButton_Click);
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
            this.RemoveItemButton.Click += new System.EventHandler(this.RemoveItemButton_Click);
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
            // panel4
            // 
            this.panel4.Controls.Add(this.GenDataDataGrid);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 20);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(406, 238);
            this.panel4.TabIndex = 3;
            // 
            // GenDataDataGrid
            // 
            this.GenDataDataGrid.AllowUserToAddRows = false;
            this.GenDataDataGrid.AllowUserToDeleteRows = false;
            this.GenDataDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GenDataDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GenDataDataGrid.Location = new System.Drawing.Point(0, 0);
            this.GenDataDataGrid.Name = "GenDataDataGrid";
            this.GenDataDataGrid.Size = new System.Drawing.Size(406, 238);
            this.GenDataDataGrid.TabIndex = 2;
            this.GenDataDataGrid.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.GenDataDataGrid_ColumnAdded);
            this.GenDataDataGrid.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.GenDataDataGrid_DataBindingComplete);
            this.GenDataDataGrid.SelectionChanged += new System.EventHandler(this.GenDataDataGrid_SelectionChanged);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.SaveItemChangesButton,
            this.CancelItemChangesButton,
            this.toolStripSeparator2,
            this.DataEditorStatusLabel,
            this.DataEditorHintLabel});
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
            this.SaveItemChangesButton.Click += new System.EventHandler(this.SaveItemChangesButton_Click);
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
            this.CancelItemChangesButton.Click += new System.EventHandler(this.CancelItemChangesButton_Click);
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
            // DataEditorHintLabel
            // 
            this.DataEditorHintLabel.Name = "DataEditorHintLabel";
            this.DataEditorHintLabel.Size = new System.Drawing.Size(0, 22);
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
            this.Load += new System.EventHandler(this.GenDataEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GenDataDataGrid)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
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
        private System.Windows.Forms.TreeView DataNavigatorTreeView;
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
        private System.Windows.Forms.ImageList DataNavigatorImageList;
        private System.Windows.Forms.ToolStripLabel DataEditorHintLabel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;

    }
}
