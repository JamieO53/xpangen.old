namespace GenEdit.UserControls
{
    partial class FunctionUserControl
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
            this.labelFunctionName = new System.Windows.Forms.Label();
            this.textBoxFunctionName = new System.Windows.Forms.TextBox();
            this.bindingSourceFunction = new System.Windows.Forms.BindingSource(this.components);
            this.panelUcHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFunction)).BeginInit();
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
            this.labelUcHeader.Text = "Function";
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
            this.textBoxName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFunction, "Name", true));
            this.textBoxName.Location = new System.Drawing.Point(6, 39);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(250, 20);
            this.textBoxName.TabIndex = 3;
            this.textBoxName.MouseEnter += new System.EventHandler(this.TextBoxNameMouseEnter);
            this.textBoxName.MouseLeave += new System.EventHandler(this.TextBoxNameMouseLeave);
            // 
            // labelFunctionName
            // 
            this.labelFunctionName.AutoSize = true;
            this.labelFunctionName.Location = new System.Drawing.Point(3, 62);
            this.labelFunctionName.Name = "labelFunctionName";
            this.labelFunctionName.Size = new System.Drawing.Size(250, 13);
            this.labelFunctionName.TabIndex = 4;
            this.labelFunctionName.Text = "Function name";
            // 
            // textBoxFunctionName
            // 
            this.textBoxFunctionName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxFunctionName.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.bindingSourceFunction, "FunctionName", true));
            this.textBoxFunctionName.Location = new System.Drawing.Point(6, 78);
            this.textBoxFunctionName.Name = "textBoxFunctionName";
            this.textBoxFunctionName.Size = new System.Drawing.Size(250, 20);
            this.textBoxFunctionName.TabIndex = 5;
            this.textBoxFunctionName.MouseEnter += new System.EventHandler(this.TextBoxFunctionNameMouseEnter);
            this.textBoxFunctionName.MouseLeave += new System.EventHandler(this.TextBoxFunctionNameMouseLeave);
            //
            // bindingSourceFunction
            // 
            this.bindingSourceFunction.DataSource = typeof(org.xpangen.Generator.Profile.Profile.Function);
            // 
            // FunctionUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelUcHeader);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelFunctionName);
            this.Controls.Add(this.textBoxFunctionName);
            this.Name = "FunctionUserControl";
            this.Size = new System.Drawing.Size(259, 106);
            this.panelUcHeader.ResumeLayout(false);
            this.panelUcHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceFunction)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelUcHeader;
        private System.Windows.Forms.Label labelUcHeader;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelFunctionName;
        private System.Windows.Forms.TextBox textBoxFunctionName;
        private System.Windows.Forms.BindingSource bindingSourceFunction;
    }
}
