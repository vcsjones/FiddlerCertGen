using System;
using System.ComponentModel.Design;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Be.Windows.Forms;
using Fiddler;
using VCSJones.FiddlerCertGen;
using VCSJones.FiddlerCertProvider;

namespace VCSJones.FiddlerCertProvider4
{
    public partial class AddEditPolicyDialog : Form
    {
        private readonly PolicyModel _model;
        private readonly TextBox _contentTextBox = new TextBox();
        private readonly HexBox _binaryEditor = new HexBox();
        private bool _suppressRadioButtonChanges = false;
        private readonly Regex _oidRegex;

        public AddEditPolicyDialog()
        {
            InitializeComponent();
            _contentTextBox.Dock = DockStyle.Fill;
            _binaryEditor.Dock = DockStyle.Fill;
            _oidRegex = new Regex(@"^(\d+\.)+\d+$");
        }

        public AddEditPolicyDialog(PolicyModel model) : this()
        {
            _model = model;
        }

        private void qualifiersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (qualifierListBox.SelectedItem?.ToString() == "New...")
            {
                var newItemIndex = qualifierListBox.Items.Count - 1;
                var qualifier = StringPrompt.PromptForString("Qualifier", "Please enter the identifier for the qualifier.", this);
                if (string.IsNullOrWhiteSpace(qualifier) || !_oidRegex.IsMatch(qualifier))
                {
                    qualifierListBox.SelectedIndex = -1;
                    return;
                }
                var policyQualifierModel = new PolicyQualifierModel { QualifierOid = qualifier };
                _model.Qualifiers.Add(policyQualifierModel);
                qualifierListBox.Items.Insert(newItemIndex, policyQualifierModel);
                qualifierListBox.SelectedIndex = newItemIndex;
            }
            else
            {
                BindModelContentEditor();
            }
        }

        private void BindModelContentEditor()
        {
            _suppressRadioButtonChanges = true;
            try
            {
                var model = qualifierListBox.SelectedItem as PolicyQualifierModel;
                if (model == null)
                {
                    binaryRadioButton.Checked = false;
                    stringRadioButton.Checked = false;
                    noValueRadioButton.Checked = false;
                    contentTypePanel.Enabled = false;
                    return;
                }
                contentPanel.Controls.Clear();
                contentTypePanel.Enabled = true;
                if (model.Type == PolicyQualifierType.None)
                {
                    noValueRadioButton.Checked = true;
                    return;
                }
                if (model.Type == PolicyQualifierType.String)
                {
                    IA5StringOrByteArray content = model.Contents;
                    string str;
                    if (content.TryIA5Parse(out str))
                    {
                        stringRadioButton.Checked = true;
                        _contentTextBox.Text = str;
                        contentPanel.Controls.Add(_contentTextBox);
                        return;
                    }
                }
                model.Type = PolicyQualifierType.Binary;
                binaryRadioButton.Checked = true;
                _binaryEditor.ByteProvider = new DynamicQualifierDataModelProvider(model);
                contentPanel.Controls.Add(_binaryEditor);
            }
            finally
            {
                _suppressRadioButtonChanges = false;
            }
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(policyIdTextBox.Text))
            {
                MessageBox.Show(this, "A policy identifer is required.", "Policy Editor");
                return;
            }
            if (!_oidRegex.IsMatch(policyIdTextBox.Text))
            {
                MessageBox.Show(this, "The policy identifier is not valid.", "Policy Editor");
                return;
            }
            _model.Oid = policyIdTextBox.Text;
        }

        private void AddEditPolicyDialog_Load(object sender, EventArgs e)
        {
            policyIdTextBox.Text = _model.Oid ?? string.Empty;
            foreach (var policyQualifierModel in _model.Qualifiers)
            {
                qualifierListBox.Items.Insert(qualifierListBox.Items.Count - 1, policyQualifierModel);
            }
        }

        private void radioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (_suppressRadioButtonChanges)
            {
                return;
            }
            var model = qualifierListBox.SelectedItem as PolicyQualifierModel;
            if (model == null)
            {
                return;
            }
            if (noValueRadioButton.Checked)
            {
                model.Type = PolicyQualifierType.None;
                model.Contents = null;
            }
            else if (stringRadioButton.Checked)
            {
                model.Type = PolicyQualifierType.String;
                model.Contents = IA5StringEncoding.Encode(string.Empty);
            }
            else if (binaryRadioButton.Checked)
            {
                model.Type = PolicyQualifierType.Binary;
                model.Contents = new byte[0];
            }
            BindModelContentEditor();
        }

        private void qualifierListBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && qualifierListBox.SelectedIndex > -1 && qualifierListBox.SelectedIndex != qualifierListBox.Items.Count-1)
            {
                var previousIndex = qualifierListBox.SelectedIndex;
                qualifierListBox.SelectedIndex = -1;
                qualifierListBox.Items.RemoveAt(previousIndex);
            }
        }
    }

    public class DynamicQualifierDataModelProvider : DynamicByteProvider
    {
        private readonly PolicyQualifierModel _model;

        public DynamicQualifierDataModelProvider(PolicyQualifierModel model) : base(model.Contents)
        {
            _model = model;
            base.Changed += DynamicQualifierDataModelProvider_Changed;
        }

        private void DynamicQualifierDataModelProvider_Changed(object sender, EventArgs e)
        {
            _model.Contents = GetAllBytes();
        }
    }
}
