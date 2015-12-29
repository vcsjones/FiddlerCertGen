namespace VCSJones.FiddlerCertProvider4
{
    partial class AddEditPolicyDialog
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
            this.label1 = new System.Windows.Forms.Label();
            this.policyIdTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.contentPanel = new System.Windows.Forms.Panel();
            this.stringRadioButton = new System.Windows.Forms.RadioButton();
            this.binaryRadioButton = new System.Windows.Forms.RadioButton();
            this.noValueRadioButton = new System.Windows.Forms.RadioButton();
            this.cancelButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.contentTypePanel = new System.Windows.Forms.Panel();
            this.qualifierListBox = new System.Windows.Forms.ListBox();
            this.contentTypePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Policy Identifier";
            // 
            // policyIdTextBox
            // 
            this.policyIdTextBox.Location = new System.Drawing.Point(96, 12);
            this.policyIdTextBox.Name = "policyIdTextBox";
            this.policyIdTextBox.Size = new System.Drawing.Size(269, 20);
            this.policyIdTextBox.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Qualifiers";
            // 
            // contentPanel
            // 
            this.contentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.contentPanel.BackColor = System.Drawing.SystemColors.Control;
            this.contentPanel.Location = new System.Drawing.Point(180, 59);
            this.contentPanel.Name = "contentPanel";
            this.contentPanel.Size = new System.Drawing.Size(465, 186);
            this.contentPanel.TabIndex = 4;
            // 
            // stringRadioButton
            // 
            this.stringRadioButton.AutoSize = true;
            this.stringRadioButton.Location = new System.Drawing.Point(3, 3);
            this.stringRadioButton.Name = "stringRadioButton";
            this.stringRadioButton.Size = new System.Drawing.Size(52, 17);
            this.stringRadioButton.TabIndex = 5;
            this.stringRadioButton.TabStop = true;
            this.stringRadioButton.Text = "&String";
            this.stringRadioButton.UseVisualStyleBackColor = true;
            this.stringRadioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // binaryRadioButton
            // 
            this.binaryRadioButton.AutoSize = true;
            this.binaryRadioButton.Location = new System.Drawing.Point(61, 3);
            this.binaryRadioButton.Name = "binaryRadioButton";
            this.binaryRadioButton.Size = new System.Drawing.Size(54, 17);
            this.binaryRadioButton.TabIndex = 6;
            this.binaryRadioButton.TabStop = true;
            this.binaryRadioButton.Text = "&Binary";
            this.binaryRadioButton.UseVisualStyleBackColor = true;
            this.binaryRadioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // noValueRadioButton
            // 
            this.noValueRadioButton.AutoSize = true;
            this.noValueRadioButton.Location = new System.Drawing.Point(121, 3);
            this.noValueRadioButton.Name = "noValueRadioButton";
            this.noValueRadioButton.Size = new System.Drawing.Size(69, 17);
            this.noValueRadioButton.TabIndex = 7;
            this.noValueRadioButton.TabStop = true;
            this.noValueRadioButton.Text = "&No Value";
            this.noValueRadioButton.UseVisualStyleBackColor = true;
            this.noValueRadioButton.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(570, 296);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 11;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(489, 296);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 12;
            this.saveButton.Text = "&Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // contentTypePanel
            // 
            this.contentTypePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.contentTypePanel.Controls.Add(this.stringRadioButton);
            this.contentTypePanel.Controls.Add(this.binaryRadioButton);
            this.contentTypePanel.Controls.Add(this.noValueRadioButton);
            this.contentTypePanel.Enabled = false;
            this.contentTypePanel.Location = new System.Drawing.Point(445, 251);
            this.contentTypePanel.Name = "contentTypePanel";
            this.contentTypePanel.Size = new System.Drawing.Size(200, 25);
            this.contentTypePanel.TabIndex = 14;
            // 
            // qualifierListBox
            // 
            this.qualifierListBox.DisplayMember = "QualifierOid";
            this.qualifierListBox.FormattingEnabled = true;
            this.qualifierListBox.Items.AddRange(new object[] {
            "New..."});
            this.qualifierListBox.Location = new System.Drawing.Point(12, 59);
            this.qualifierListBox.Name = "qualifierListBox";
            this.qualifierListBox.Size = new System.Drawing.Size(162, 186);
            this.qualifierListBox.TabIndex = 15;
            this.qualifierListBox.ValueMember = "QualifierOid";
            this.qualifierListBox.SelectedIndexChanged += new System.EventHandler(this.qualifiersListBox_SelectedIndexChanged);
            this.qualifierListBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.qualifierListBox_KeyUp);
            // 
            // AddEditPolicyDialog
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(657, 331);
            this.Controls.Add(this.qualifierListBox);
            this.Controls.Add(this.contentTypePanel);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.contentPanel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.policyIdTextBox);
            this.Controls.Add(this.label1);
            this.Name = "AddEditPolicyDialog";
            this.Text = "Policy";
            this.Load += new System.EventHandler(this.AddEditPolicyDialog_Load);
            this.contentTypePanel.ResumeLayout(false);
            this.contentTypePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox policyIdTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.RadioButton stringRadioButton;
        private System.Windows.Forms.RadioButton binaryRadioButton;
        private System.Windows.Forms.RadioButton noValueRadioButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Panel contentTypePanel;
        private System.Windows.Forms.ListBox qualifierListBox;
    }
}