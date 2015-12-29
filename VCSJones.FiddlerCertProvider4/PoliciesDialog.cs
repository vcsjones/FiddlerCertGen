using System.Collections.Generic;
using System.Windows.Forms;
using VCSJones.FiddlerCertProvider;

namespace VCSJones.FiddlerCertProvider4
{
    public partial class PoliciesDialog : Form
    {
        public PoliciesDialog()
        {
            InitializeComponent();
        }

        private void addButton_Click(object sender, System.EventArgs e)
        {
            var policyModel = new PolicyModel();
            using (var addPolicyDialog = new AddEditPolicyDialog(policyModel))
            {
                addPolicyDialog.ShowDialog(this);
            }
        }
    }
}
