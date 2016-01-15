namespace VCSJones.FiddlerCertProvider4
{
    partial class PolicyQualifierEditor
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
            this.qualifierIdTextBox = new System.Windows.Forms.TextBox();
            this.qualifierIdLabel = new System.Windows.Forms.Label();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.editorPanel = new System.Windows.Forms.Panel();
            this.noneRadioButton = new System.Windows.Forms.RadioButton();
            this.binaryRadioButton = new System.Windows.Forms.RadioButton();
            this.stringRadioButton = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // qualifierIdTextBox
            // 
            this.qualifierIdTextBox.Location = new System.Drawing.Point(68, 3);
            this.qualifierIdTextBox.Name = "qualifierIdTextBox";
            this.qualifierIdTextBox.Size = new System.Drawing.Size(226, 20);
            this.qualifierIdTextBox.TabIndex = 0;
            this.qualifierIdTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.qualifierIdTextBox_Validating);
            // 
            // qualifierIdLabel
            // 
            this.qualifierIdLabel.AutoSize = true;
            this.qualifierIdLabel.Location = new System.Drawing.Point(3, 6);
            this.qualifierIdLabel.Name = "qualifierIdLabel";
            this.qualifierIdLabel.Size = new System.Drawing.Size(59, 13);
            this.qualifierIdLabel.TabIndex = 1;
            this.qualifierIdLabel.Text = "Qualifier ID";
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // editorPanel
            // 
            this.editorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.editorPanel.Location = new System.Drawing.Point(6, 29);
            this.editorPanel.Name = "editorPanel";
            this.editorPanel.Size = new System.Drawing.Size(288, 164);
            this.editorPanel.TabIndex = 2;
            // 
            // noneRadioButton
            // 
            this.noneRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.noneRadioButton.AutoSize = true;
            this.noneRadioButton.Location = new System.Drawing.Point(6, 199);
            this.noneRadioButton.Name = "noneRadioButton";
            this.noneRadioButton.Size = new System.Drawing.Size(51, 17);
            this.noneRadioButton.TabIndex = 3;
            this.noneRadioButton.TabStop = true;
            this.noneRadioButton.Text = "None";
            this.noneRadioButton.UseVisualStyleBackColor = true;
            this.noneRadioButton.CheckedChanged += new System.EventHandler(this.qualifierType_CheckChanged);
            // 
            // binaryRadioButton
            // 
            this.binaryRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.binaryRadioButton.AutoSize = true;
            this.binaryRadioButton.Location = new System.Drawing.Point(63, 199);
            this.binaryRadioButton.Name = "binaryRadioButton";
            this.binaryRadioButton.Size = new System.Drawing.Size(54, 17);
            this.binaryRadioButton.TabIndex = 4;
            this.binaryRadioButton.TabStop = true;
            this.binaryRadioButton.Text = "Binary";
            this.binaryRadioButton.UseVisualStyleBackColor = true;
            this.binaryRadioButton.CheckedChanged += new System.EventHandler(this.qualifierType_CheckChanged);
            // 
            // stringRadioButton
            // 
            this.stringRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.stringRadioButton.AutoSize = true;
            this.stringRadioButton.Location = new System.Drawing.Point(123, 199);
            this.stringRadioButton.Name = "stringRadioButton";
            this.stringRadioButton.Size = new System.Drawing.Size(52, 17);
            this.stringRadioButton.TabIndex = 5;
            this.stringRadioButton.TabStop = true;
            this.stringRadioButton.Text = "String";
            this.stringRadioButton.UseVisualStyleBackColor = true;
            this.stringRadioButton.CheckedChanged += new System.EventHandler(this.qualifierType_CheckChanged);
            // 
            // PolicyQualifierEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.stringRadioButton);
            this.Controls.Add(this.binaryRadioButton);
            this.Controls.Add(this.noneRadioButton);
            this.Controls.Add(this.editorPanel);
            this.Controls.Add(this.qualifierIdLabel);
            this.Controls.Add(this.qualifierIdTextBox);
            this.Name = "PolicyQualifierEditor";
            this.Size = new System.Drawing.Size(297, 220);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox qualifierIdTextBox;
        private System.Windows.Forms.Label qualifierIdLabel;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.RadioButton binaryRadioButton;
        private System.Windows.Forms.RadioButton noneRadioButton;
        private System.Windows.Forms.Panel editorPanel;
        private System.Windows.Forms.RadioButton stringRadioButton;
    }
}
