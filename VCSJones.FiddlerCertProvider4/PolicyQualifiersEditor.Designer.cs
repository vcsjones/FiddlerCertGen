namespace VCSJones.FiddlerCertProvider4
{
    partial class PolicyQualifiersEditor
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
            this.helpProvider = new System.Windows.Forms.HelpProvider();
            this.deleteButton = new System.Windows.Forms.Button();
            this.editButton = new System.Windows.Forms.Button();
            this.qualifiersListBox = new System.Windows.Forms.ListBox();
            this.addButton = new System.Windows.Forms.Button();
            this.policiesTextLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // deleteButton
            // 
            this.deleteButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpProvider.SetHelpString(this.deleteButton, "Delete all of the selected policies.");
            this.deleteButton.Location = new System.Drawing.Point(174, 230);
            this.deleteButton.Name = "deleteButton";
            this.helpProvider.SetShowHelp(this.deleteButton, true);
            this.deleteButton.Size = new System.Drawing.Size(75, 23);
            this.deleteButton.TabIndex = 10;
            this.deleteButton.Text = "&Delete";
            this.deleteButton.UseVisualStyleBackColor = true;
            // 
            // editButton
            // 
            this.editButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpProvider.SetHelpString(this.editButton, "Edit the currently selected policy. Only one policy may be selected at a time.");
            this.editButton.Location = new System.Drawing.Point(93, 230);
            this.editButton.Name = "editButton";
            this.helpProvider.SetShowHelp(this.editButton, true);
            this.editButton.Size = new System.Drawing.Size(75, 23);
            this.editButton.TabIndex = 9;
            this.editButton.Text = "&Edit...";
            this.editButton.UseVisualStyleBackColor = true;
            // 
            // qualifiersListBox
            // 
            this.qualifiersListBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.qualifiersListBox.DisplayMember = "Oid";
            this.qualifiersListBox.FormattingEnabled = true;
            this.helpProvider.SetHelpString(this.qualifiersListBox, "A list of qualifiers by their qualifier identifier.");
            this.qualifiersListBox.Location = new System.Drawing.Point(12, 25);
            this.qualifiersListBox.Name = "qualifiersListBox";
            this.qualifiersListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.helpProvider.SetShowHelp(this.qualifiersListBox, true);
            this.qualifiersListBox.Size = new System.Drawing.Size(357, 199);
            this.qualifiersListBox.TabIndex = 8;
            // 
            // addButton
            // 
            this.addButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.helpProvider.SetHelpString(this.addButton, "Add a new policy.");
            this.addButton.Location = new System.Drawing.Point(12, 230);
            this.addButton.Name = "addButton";
            this.helpProvider.SetShowHelp(this.addButton, true);
            this.addButton.Size = new System.Drawing.Size(75, 23);
            this.addButton.TabIndex = 7;
            this.addButton.Text = "&Add...";
            this.addButton.UseVisualStyleBackColor = true;
            // 
            // policiesTextLabel
            // 
            this.policiesTextLabel.AutoSize = true;
            this.policiesTextLabel.Location = new System.Drawing.Point(12, 9);
            this.policiesTextLabel.Name = "policiesTextLabel";
            this.policiesTextLabel.Size = new System.Drawing.Size(43, 13);
            this.policiesTextLabel.TabIndex = 6;
            this.policiesTextLabel.Text = "Policies";
            // 
            // PolicyQualifiersEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(382, 265);
            this.Controls.Add(this.deleteButton);
            this.Controls.Add(this.editButton);
            this.Controls.Add(this.qualifiersListBox);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.policiesTextLabel);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(250, 250);
            this.Name = "PolicyQualifiersEditor";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "KeyUs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.HelpProvider helpProvider;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button editButton;
        private System.Windows.Forms.ListBox qualifiersListBox;
        private System.Windows.Forms.Button addButton;
        private System.Windows.Forms.Label policiesTextLabel;
    }
}