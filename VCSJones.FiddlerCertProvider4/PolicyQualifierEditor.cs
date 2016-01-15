using System;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Be.Windows.Forms;
using VCSJones.FiddlerCertGen;
using VCSJones.FiddlerCertProvider;

namespace VCSJones.FiddlerCertProvider4
{
    public partial class PolicyQualifierEditor : UserControl
    {
        private static readonly Regex _qualifierValidator = new Regex(@"^(\d+\.)+\d+$", RegexOptions.Compiled);
        private PolicyQualifierModel _model;
        private readonly TextBox _stringEditorTextBox;
        private readonly HexBox _binaryEditorHexBox;
        private bool _suspendCheckChangeHandling;

        internal PolicyQualifierEditor()
        {
            InitializeComponent();
            var disposer = new Disposer(CleanUp);
            components.Add(disposer);
            _stringEditorTextBox = new TextBox { Dock = DockStyle.Fill, AcceptsReturn = true, Multiline = true};
            _binaryEditorHexBox = new HexBox {Dock = DockStyle.Fill, ByteProvider = new DynamicByteProvider(new byte[0])};
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public PolicyQualifierModel Model
        {
            get
            {
                if (_model == null)
                {
                    return null;
                }
                UpdateModel();
                return _model;
            }
            set
            {
                _model = value;
                if (_model != null)
                {
                    BindModel();
                }
            }
        }

        private void CleanUp(bool disposing)
        {
            if (disposing)
            {
                _stringEditorTextBox.Dispose();
                _binaryEditorHexBox.Dispose();
            }

        }

        private void qualifierIdTextBox_Validating(object sender, CancelEventArgs e)
        {
            e.Cancel = !_qualifierValidator.IsMatch(qualifierIdTextBox.Text);
            errorProvider.SetError(qualifierIdTextBox, "The specified OID is not valid.");
        }

        private void BindModel()
        {
            qualifierIdTextBox.Text = _model.QualifierOid;
            BindEditor(_model.Type, _model.Contents);
            _suspendCheckChangeHandling = true;
            BindRadioButtons();
            _suspendCheckChangeHandling = false;
        }

        private void qualifierType_CheckChanged(object sender, EventArgs e)
        {
            if (_suspendCheckChangeHandling)
            {
                return;
            }
            if (binaryRadioButton.Checked)
            {
                BindEditor(PolicyQualifierType.Binary, new byte[0]);
            }
            else if (stringRadioButton.Checked)
            {
                BindEditor(PolicyQualifierType.String, string.Empty);
            }
            else
            {
                BindEditor(PolicyQualifierType.None, new byte[0]);
            }
        }

        private void BindRadioButtons()
        {
            switch (_model.Type)
            {
                case PolicyQualifierType.Binary:
                    binaryRadioButton.Checked = true;
                    break;
                case PolicyQualifierType.None:
                    noneRadioButton.Checked = true;
                    break;
                case PolicyQualifierType.String:
                    stringRadioButton.Checked = true;
                    break;
                default:
                    throw new InvalidOperationException($"Specified qualifier type {_model.Type} is not supported.");
            }
        }

        private void BindEditor(PolicyQualifierType type, IA5StringOrByteArray value)
        {
            editorPanel.Controls.Clear();
            switch (type)
            {
                case PolicyQualifierType.Binary:
                    _binaryEditorHexBox.ByteProvider.DeleteBytes(0L, _binaryEditorHexBox.ByteProvider.Length);
                    _binaryEditorHexBox.ByteProvider.InsertBytes(0L, value.ToByteArray());
                    editorPanel.Controls.Add(_binaryEditorHexBox);
                    break;
                case PolicyQualifierType.String:
                    string val;
                    if (value.TryIA5Parse(out val))
                    {
                        _stringEditorTextBox.Text = val;
                        editorPanel.Controls.Add(_stringEditorTextBox);
                    }
                    else
                    {
                        //The content was specified as an IA5String but was invalid. Treat it as raw binary content.
                        goto case PolicyQualifierType.Binary;
                    }
                    break;
                case PolicyQualifierType.None:
                    break;
                default:
                    throw new ArgumentException($"Specified qualifier type {type} is not supported.", nameof(type));
            }
        }

        public void UpdateModel()
        {
            _model.QualifierOid = qualifierIdTextBox.Text;
            if (binaryRadioButton.Checked)
            {
                _model.Type = PolicyQualifierType.Binary;
                _model.Contents = _binaryEditorHexBox.GetAllBytes();
            }
            else if (stringRadioButton.Checked)
            {
                _model.Type = PolicyQualifierType.String;
                IA5StringOrByteArray str = _stringEditorTextBox.Text;
                _model.Contents = str.ToByteArray();
            }
            else if (noneRadioButton.Checked)
            {
                _model.Type = PolicyQualifierType.None;
                _model.Contents = null;
            }
        }
    }
}
