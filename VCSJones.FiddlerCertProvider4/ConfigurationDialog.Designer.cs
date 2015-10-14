namespace VCSJones.FiddlerCertProvider4
{
    partial class ConfigurationDialog
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
            this.components = new System.ComponentModel.Container();
            this.exportPrivateKey = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.rootAlgorithm = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rootHashAlgorithm = new System.Windows.Forms.ComboBox();
            this.keySize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.seperator = new System.Windows.Forms.Panel();
            this.eeKeySize = new System.Windows.Forms.NumericUpDown();
            this.eeAlgorithm = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.keySize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eeKeySize)).BeginInit();
            this.SuspendLayout();
            // 
            // exportPrivateKey
            // 
            this.exportPrivateKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.exportPrivateKey.Location = new System.Drawing.Point(12, 212);
            this.exportPrivateKey.Name = "exportPrivateKey";
            this.exportPrivateKey.Size = new System.Drawing.Size(166, 23);
            this.exportPrivateKey.TabIndex = 0;
            this.exportPrivateKey.Text = "&Export Root Certificate to PFX";
            this.toolTip.SetToolTip(this.exportPrivateKey, "Export the certificate with the private key. This is useful for other tools that " +
        "might need the private key like Wireshark.");
            this.exportPrivateKey.UseVisualStyleBackColor = true;
            this.exportPrivateKey.Click += new System.EventHandler(this.exportPrivateKey_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(204, 212);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(285, 212);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Root Certificate Algorithm";
            // 
            // rootAlgorithm
            // 
            this.rootAlgorithm.FormattingEnabled = true;
            this.rootAlgorithm.Location = new System.Drawing.Point(172, 6);
            this.rootAlgorithm.Name = "rootAlgorithm";
            this.rootAlgorithm.Size = new System.Drawing.Size(188, 21);
            this.rootAlgorithm.TabIndex = 4;
            this.rootAlgorithm.SelectionChangeCommitted += new System.EventHandler(this.rootAlgorithm_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Root Certificate Hash Algorithm";
            // 
            // rootHashAlgorithm
            // 
            this.rootHashAlgorithm.FormattingEnabled = true;
            this.rootHashAlgorithm.Location = new System.Drawing.Point(172, 59);
            this.rootHashAlgorithm.Name = "rootHashAlgorithm";
            this.rootHashAlgorithm.Size = new System.Drawing.Size(188, 21);
            this.rootHashAlgorithm.TabIndex = 6;
            // 
            // keySize
            // 
            this.keySize.Increment = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.keySize.Location = new System.Drawing.Point(172, 33);
            this.keySize.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.keySize.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.keySize.Name = "keySize";
            this.keySize.Size = new System.Drawing.Size(188, 20);
            this.keySize.TabIndex = 7;
            this.keySize.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(92, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Root Key Size";
            // 
            // seperator
            // 
            this.seperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.seperator.BackColor = System.Drawing.SystemColors.ControlDark;
            this.seperator.Location = new System.Drawing.Point(0, 86);
            this.seperator.Margin = new System.Windows.Forms.Padding(0);
            this.seperator.Name = "seperator";
            this.seperator.Size = new System.Drawing.Size(372, 3);
            this.seperator.TabIndex = 9;
            // 
            // eeKeySize
            // 
            this.eeKeySize.Increment = new decimal(new int[] {
            512,
            0,
            0,
            0});
            this.eeKeySize.Location = new System.Drawing.Point(172, 119);
            this.eeKeySize.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.eeKeySize.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.eeKeySize.Name = "eeKeySize";
            this.eeKeySize.Size = new System.Drawing.Size(188, 20);
            this.eeKeySize.TabIndex = 12;
            this.eeKeySize.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // eeAlgorithm
            // 
            this.eeAlgorithm.FormattingEnabled = true;
            this.eeAlgorithm.Location = new System.Drawing.Point(172, 92);
            this.eeAlgorithm.Name = "eeAlgorithm";
            this.eeAlgorithm.Size = new System.Drawing.Size(188, 21);
            this.eeAlgorithm.TabIndex = 11;
            this.eeAlgorithm.SelectedIndexChanged += new System.EventHandler(this.eeAlgorithm_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "End Entity Certificate Algorithm";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(67, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "End Entity Key Size";
            // 
            // ConfigurationDialog
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(372, 247);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.eeKeySize);
            this.Controls.Add(this.eeAlgorithm);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.seperator);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.keySize);
            this.Controls.Add(this.rootHashAlgorithm);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rootAlgorithm);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.exportPrivateKey);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConfigurationDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Certificate Configuration";
            this.Load += new System.EventHandler(this.ConfigurationDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.keySize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eeKeySize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button exportPrivateKey;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox rootAlgorithm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox rootHashAlgorithm;
        private System.Windows.Forms.NumericUpDown keySize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Panel seperator;
        private System.Windows.Forms.NumericUpDown eeKeySize;
        private System.Windows.Forms.ComboBox eeAlgorithm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}