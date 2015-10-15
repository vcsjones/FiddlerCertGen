namespace VCSJones.FiddlerCertProvider2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationDialog));
            this.exportPrivateKey = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rootAlgorithm = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rootHashAlgorithm = new System.Windows.Forms.ComboBox();
            this.keySize = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.seperator = new System.Windows.Forms.Panel();
            this.eeKeySize = new System.Windows.Forms.NumericUpDown();
            this.eeAlgorithm = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.keySize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.eeKeySize)).BeginInit();
            this.SuspendLayout();
            // 
            // exportPrivateKey
            // 
            this.exportPrivateKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpProvider.SetHelpString(this.exportPrivateKey, "Exports the root certificate along with the private key. This is useful if the pr" +
        "ivate key is used for other capturing purposes such as Wireshark.");
            this.exportPrivateKey.Location = new System.Drawing.Point(12, 175);
            this.exportPrivateKey.Name = "exportPrivateKey";
            this.helpProvider.SetShowHelp(this.exportPrivateKey, true);
            this.exportPrivateKey.Size = new System.Drawing.Size(166, 23);
            this.exportPrivateKey.TabIndex = 0;
            this.exportPrivateKey.Text = "&Export Root Certificate to PFX";
            this.exportPrivateKey.UseVisualStyleBackColor = true;
            this.exportPrivateKey.Click += new System.EventHandler(this.exportPrivateKey_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.helpProvider.SetHelpString(this.saveButton, "");
            this.saveButton.Location = new System.Drawing.Point(204, 175);
            this.saveButton.Name = "saveButton";
            this.helpProvider.SetShowHelp(this.saveButton, false);
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
            this.cancelButton.Location = new System.Drawing.Point(285, 175);
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
            this.label1.Location = new System.Drawing.Point(30, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Root Certificate Algorithm";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // rootAlgorithm
            // 
            this.rootAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rootAlgorithm.FormattingEnabled = true;
            this.helpProvider.SetHelpString(this.rootAlgorithm, resources.GetString("rootAlgorithm.HelpString"));
            this.rootAlgorithm.Location = new System.Drawing.Point(172, 6);
            this.rootAlgorithm.Name = "rootAlgorithm";
            this.helpProvider.SetShowHelp(this.rootAlgorithm, true);
            this.rootAlgorithm.Size = new System.Drawing.Size(188, 21);
            this.rootAlgorithm.TabIndex = 4;
            this.rootAlgorithm.SelectionChangeCommitted += new System.EventHandler(this.rootAlgorithm_SelectionChangeCommitted);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(2, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(154, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Root Certificate Hash Algorithm";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // rootHashAlgorithm
            // 
            this.rootHashAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rootHashAlgorithm.FormattingEnabled = true;
            this.helpProvider.SetHelpString(this.rootHashAlgorithm, resources.GetString("rootHashAlgorithm.HelpString"));
            this.rootHashAlgorithm.Location = new System.Drawing.Point(172, 59);
            this.rootHashAlgorithm.Name = "rootHashAlgorithm";
            this.helpProvider.SetShowHelp(this.rootHashAlgorithm, true);
            this.rootHashAlgorithm.Size = new System.Drawing.Size(188, 21);
            this.rootHashAlgorithm.TabIndex = 6;
            // 
            // keySize
            // 
            this.helpProvider.SetHelpString(this.keySize, resources.GetString("keySize.HelpString"));
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
            this.helpProvider.SetShowHelp(this.keySize, true);
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
            this.label3.Location = new System.Drawing.Point(82, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Root Key Size";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
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
            this.helpProvider.SetHelpString(this.eeKeySize, resources.GetString("eeKeySize.HelpString"));
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
            this.helpProvider.SetShowHelp(this.eeKeySize, true);
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
            this.eeAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.eeAlgorithm.FormattingEnabled = true;
            this.helpProvider.SetHelpString(this.eeAlgorithm, resources.GetString("eeAlgorithm.HelpString"));
            this.eeAlgorithm.Location = new System.Drawing.Point(172, 92);
            this.eeAlgorithm.Name = "eeAlgorithm";
            this.helpProvider.SetShowHelp(this.eeAlgorithm, true);
            this.eeAlgorithm.Size = new System.Drawing.Size(188, 21);
            this.eeAlgorithm.TabIndex = 11;
            this.eeAlgorithm.SelectedIndexChanged += new System.EventHandler(this.eeAlgorithm_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 95);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(151, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "End Entity Certificate Algorithm";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(57, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "End Entity Key Size";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // ConfigurationDialog
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(372, 210);
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
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ConfigurationDialog";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Certificate Configuration";
            this.Load += new System.EventHandler(this.ConfigurationDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.keySize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.eeKeySize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button exportPrivateKey;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox rootAlgorithm;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox rootHashAlgorithm;
        private System.Windows.Forms.NumericUpDown keySize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel seperator;
        private System.Windows.Forms.NumericUpDown eeKeySize;
        private System.Windows.Forms.ComboBox eeAlgorithm;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.HelpProvider helpProvider;
    }
}