using System.Collections.Generic;
using System.Windows.Forms;
using VCSJones.FiddlerCertProvider;

namespace VCSJones.FiddlerCertProvider4
{
    public partial class PolicyEditor : Form
    {
        public PolicyEditor()
        {
            InitializeComponent();
        }

        private void policiesListBox_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            BindButtonStatus();
        }

        private void BindButtonStatus()
        {
            editButton.Enabled = policiesListBox.SelectedIndices.Count == 1;
            deleteButton.Enabled = policiesListBox.SelectedIndices.Count > 0;
        }

        private void deleteButton_Click(object sender, System.EventArgs e)
        {
            var itemsToRemove = new List<PolicyModel>();
            foreach (var selectedItem in policiesListBox.SelectedItems)
            {
                var model = selectedItem as PolicyModel;
                if (model != null)
                {
                    itemsToRemove.Add(model);
                    PolicyConfiguration.Instance.RemovePolicy(model.Oid);
                }
            }
            foreach (var item in itemsToRemove)
            {
                policiesListBox.Items.Remove(item);
            }
        }

        private void addButton_Click(object sender, System.EventArgs e)
        {
            var input = StringPrompt.PromptForString("Policy Editor", "Please enter the ID for the new policy.", this);
            if (!string.IsNullOrEmpty(input))
            {
                if (PolicyConfiguration.Instance.PolicyExists(input))
                {
                    MessageBox.Show(this, "A policy with this OID already exists.", "Policy Editor");
                    return;
                }
                var policyModel = new PolicyModel {Oid = input};
                PolicyConfiguration.Instance.AddPolicy(policyModel);
                policiesListBox.Items.Add(policyModel);
            }
        }

        private void PolicyEditor_Load(object sender, System.EventArgs e)
        {
            var policies = PolicyConfiguration.Instance.GetAllPolicies();
            foreach (var policy in policies)
            {
                policiesListBox.Items.Add(policy);
            }
            BindButtonStatus();
        }

        private void editButton_Click(object sender, System.EventArgs e)
        {

        }
    }
}
