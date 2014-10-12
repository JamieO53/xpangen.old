namespace GenEdit.View
{
    partial class GenProfileEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenProfileEditor));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ProfileNavigatorTreeView = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.ProfileTextBox = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.ProfileExpansionTextBox = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.ProfileNavigatorImageList = new System.Windows.Forms.ImageList(this.components);
            this.textUserControl = new GenEdit.UserControls.TextUserControl();
            this.placeholderUserControl = new GenEdit.UserControls.PlaceholderUserControl();
            this.profileUserControl = new GenEdit.UserControls.ProfileUserControl();
            this.segmentUserControl = new GenEdit.UserControls.SegmentUserControl();
            this.blockUserControl = new GenEdit.UserControls.BlockUserControl();
            this.lookupUserControl = new GenEdit.UserControls.LookupUserControl();
            this.conditionUserControl = new GenEdit.UserControls.ConditionUserControl();
            this.functionUserControl = new GenEdit.UserControls.FunctionUserControl();
            this.textBlockUserControl = new GenEdit.UserControls.TextBlockUserControl();
            this.annotationUserControl = new GenEdit.UserControls.AnnotationUserControl();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
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
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(670, 472);
            this.splitContainer1.SplitterDistance = 223;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ProfileNavigatorTreeView);
            this.splitContainer2.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.textUserControl);
            this.splitContainer2.Panel2.Controls.Add(this.placeholderUserControl);
            this.splitContainer2.Panel2.Controls.Add(this.profileUserControl);
            this.splitContainer2.Panel2.Controls.Add(this.segmentUserControl);
            this.splitContainer2.Panel2.Controls.Add(this.blockUserControl);
            this.splitContainer2.Panel2.Controls.Add(this.lookupUserControl);
            this.splitContainer2.Panel2.Controls.Add(this.conditionUserControl);
            this.splitContainer2.Panel2.Controls.Add(this.functionUserControl);
            this.splitContainer2.Panel2.Controls.Add(this.textBlockUserControl);
            this.splitContainer2.Panel2.Controls.Add(this.annotationUserControl);
            this.splitContainer2.Size = new System.Drawing.Size(223, 472);
            this.splitContainer2.SplitterDistance = 225;
            this.splitContainer2.TabIndex = 0;
            // 
            // ProfileNavigatorTreeView
            // 
            this.ProfileNavigatorTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProfileNavigatorTreeView.Location = new System.Drawing.Point(0, 20);
            this.ProfileNavigatorTreeView.Name = "ProfileNavigatorTreeView";
            this.ProfileNavigatorTreeView.Size = new System.Drawing.Size(223, 205);
            this.ProfileNavigatorTreeView.TabIndex = 1;
            this.ProfileNavigatorTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(223, 20);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Left;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Profile navigator";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(223, 20);
            this.panel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Left;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(129, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Fragment definition";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            this.splitContainer3.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.ProfileTextBox);
            this.splitContainer3.Panel1.Controls.Add(this.panel3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.ProfileExpansionTextBox);
            this.splitContainer3.Panel2.Controls.Add(this.panel4);
            this.splitContainer3.Size = new System.Drawing.Size(443, 472);
            this.splitContainer3.SplitterDistance = 225;
            this.splitContainer3.TabIndex = 1;
            // 
            // ProfileTextBox
            // 
            this.ProfileTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProfileTextBox.Location = new System.Drawing.Point(0, 20);
            this.ProfileTextBox.Multiline = true;
            this.ProfileTextBox.Name = "ProfileTextBox";
            this.ProfileTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ProfileTextBox.Size = new System.Drawing.Size(443, 205);
            this.ProfileTextBox.TabIndex = 2;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(443, 20);
            this.panel3.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Left;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "Profile text";
            // 
            // ProfileExpansionTextBox
            // 
            this.ProfileExpansionTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProfileExpansionTextBox.Location = new System.Drawing.Point(0, 20);
            this.ProfileExpansionTextBox.Multiline = true;
            this.ProfileExpansionTextBox.Name = "ProfileExpansionTextBox";
            this.ProfileExpansionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.ProfileExpansionTextBox.Size = new System.Drawing.Size(443, 223);
            this.ProfileExpansionTextBox.TabIndex = 3;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label4);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(443, 20);
            this.panel4.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Left;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(145, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Generated profile text";
            // 
            // ProfileNavigatorImageList
            // 
            this.ProfileNavigatorImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ProfileNavigatorImageList.ImageStream")));
            this.ProfileNavigatorImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.ProfileNavigatorImageList.Images.SetKeyName(0, "DataBrowserSelection.png");
            this.ProfileNavigatorImageList.Images.SetKeyName(1, "ProfileBrowserText.png");
            this.ProfileNavigatorImageList.Images.SetKeyName(2, "ProfileBrowserPlaceholder.png");
            this.ProfileNavigatorImageList.Images.SetKeyName(3, "ProfileBrowserSegment.png");
            this.ProfileNavigatorImageList.Images.SetKeyName(4, "ProfileBrowserSegment.png");
            this.ProfileNavigatorImageList.Images.SetKeyName(5, "ProfileBrowserSegment.png");
            this.ProfileNavigatorImageList.Images.SetKeyName(6, "ProfileBrowserLookup.png");
            this.ProfileNavigatorImageList.Images.SetKeyName(7, "ProfileBrowserCondition.png");
            this.ProfileNavigatorImageList.Images.SetKeyName(8, "ProfileBrowserFunction.png");
            // 
            // textUserControl
            //
            this.textUserControl.Name = "textUserControl";
            this.textUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textUserControl.Location = new System.Drawing.Point(0, 20);
            this.textUserControl.Text = null;
            this.textUserControl.Size = new System.Drawing.Size(223, 223);
            this.textUserControl.TabIndex = 2;
            this.textUserControl.Visible = false;
            // 
            // placeholderUserControl
            //
            this.placeholderUserControl.Name = "placeholderUserControl";
            this.placeholderUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.placeholderUserControl.Location = new System.Drawing.Point(0, 20);
            this.placeholderUserControl.Placeholder = null;
            this.placeholderUserControl.Size = new System.Drawing.Size(223, 223);
            this.placeholderUserControl.TabIndex = 2;
            this.placeholderUserControl.Visible = false;
            // 
            // profileUserControl
            //
            this.profileUserControl.Name = "profileUserControl";
            this.profileUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.profileUserControl.Location = new System.Drawing.Point(0, 20);
            this.profileUserControl.Profile = null;
            this.profileUserControl.Size = new System.Drawing.Size(223, 223);
            this.profileUserControl.TabIndex = 2;
            this.profileUserControl.Visible = false;
            // 
            // segmentUserControl
            //
            this.segmentUserControl.Cardinality = null;
            this.segmentUserControl.Name = "segmentUserControl";
            this.segmentUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.segmentUserControl.Location = new System.Drawing.Point(0, 20);
            this.segmentUserControl.Segment = null;
            this.segmentUserControl.Size = new System.Drawing.Size(223, 223);
            this.segmentUserControl.TabIndex = 2;
            this.segmentUserControl.Visible = false;
            // 
            // blockUserControl
            //
            this.blockUserControl.Name = "blockUserControl";
            this.blockUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.blockUserControl.Location = new System.Drawing.Point(0, 20);
            this.blockUserControl.Block = null;
            this.blockUserControl.Size = new System.Drawing.Size(223, 223);
            this.blockUserControl.TabIndex = 2;
            this.blockUserControl.Visible = false;
            // 
            // lookupUserControl
            //
            this.lookupUserControl.Name = "lookupUserControl";
            this.lookupUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lookupUserControl.Location = new System.Drawing.Point(0, 20);
            this.lookupUserControl.Lookup = null;
            this.lookupUserControl.Size = new System.Drawing.Size(223, 223);
            this.lookupUserControl.TabIndex = 2;
            this.lookupUserControl.Visible = false;
            // 
            // conditionUserControl
            //
            this.conditionUserControl.Comparison = null;
            this.conditionUserControl.Name = "conditionUserControl";
            this.conditionUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.conditionUserControl.Location = new System.Drawing.Point(0, 20);
            this.conditionUserControl.Condition = null;
            this.conditionUserControl.Size = new System.Drawing.Size(223, 223);
            this.conditionUserControl.TabIndex = 2;
            this.conditionUserControl.Visible = false;
            // 
            // functionUserControl
            //
            this.functionUserControl.Name = "functionUserControl";
            this.functionUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.functionUserControl.Location = new System.Drawing.Point(0, 20);
            this.functionUserControl.Function = null;
            this.functionUserControl.Size = new System.Drawing.Size(223, 223);
            this.functionUserControl.TabIndex = 2;
            this.functionUserControl.Visible = false;
            // 
            // textBlockUserControl
            //
            this.textBlockUserControl.Name = "textBlockUserControl";
            this.textBlockUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBlockUserControl.Location = new System.Drawing.Point(0, 20);
            this.textBlockUserControl.TextBlock = null;
            this.textBlockUserControl.Size = new System.Drawing.Size(223, 223);
            this.textBlockUserControl.TabIndex = 2;
            this.textBlockUserControl.Visible = false;
            // 
            // annotationUserControl
            //
            this.annotationUserControl.Name = "annotationUserControl";
            this.annotationUserControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.annotationUserControl.Location = new System.Drawing.Point(0, 20);
            this.annotationUserControl.Annotation = null;
            this.annotationUserControl.Size = new System.Drawing.Size(223, 223);
            this.annotationUserControl.TabIndex = 2;
            this.annotationUserControl.Visible = false;
            // 
            // GenProfileEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "GenProfileEditor";
            this.Size = new System.Drawing.Size(670, 472);
            this.Load += new System.EventHandler(this.GenProfileEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            this.splitContainer3.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.TreeView ProfileNavigatorTreeView;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ProfileTextBox;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox ProfileExpansionTextBox;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ImageList ProfileNavigatorImageList;
        private UserControls.TextUserControl textUserControl;
        private UserControls.PlaceholderUserControl placeholderUserControl;
        private UserControls.ProfileUserControl profileUserControl;
        private UserControls.SegmentUserControl segmentUserControl;
        private UserControls.BlockUserControl blockUserControl;
        private UserControls.LookupUserControl lookupUserControl;
        private UserControls.ConditionUserControl conditionUserControl;
        private UserControls.FunctionUserControl functionUserControl;
        private UserControls.TextBlockUserControl textBlockUserControl;
        private UserControls.AnnotationUserControl annotationUserControl;
    }
}
