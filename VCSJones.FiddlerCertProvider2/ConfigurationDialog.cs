using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using VCSJones.FiddlerCertGen;

namespace VCSJones.FiddlerCertProvider2
{
    public partial class ConfigurationDialog : Form
    {
        private readonly FiddlerCertificate _certificateProvider;

        public ConfigurationDialog()
        {
            InitializeComponent();
        }

        public ConfigurationDialog(FiddlerCertificate certificateProvider) : this()
        {
            _certificateProvider = certificateProvider;
        }

        private void exportPrivateKey_Click(object sender, EventArgs e)
        {
            var root = _certificateProvider.GetRootCertificate();
            var exportInfo = new CRYPTUI_WIZ_EXPORT_INFO();
            exportInfo.context = new CRYPTUI_WIZ_EXPORT_INFO_UNION { pCertContext = root.Handle };
            exportInfo.cStores = 0;
            exportInfo.dwSize = (uint)Marshal.SizeOf(typeof(CRYPTUI_WIZ_EXPORT_INFO));
            exportInfo.dwSubjectType = CryptUIExportInfoSubjectType.CRYPTUI_WIZ_EXPORT_CERT_CONTEXT;
            exportInfo.pwszExportFileName = null;
            exportInfo.rghStores = IntPtr.Zero;
            NativeMethods.CryptUIWizExport(CryptUIWizExportFlags.CRYPTUI_WIZ_EXPORT_PRIVATE_KEY | CryptUIWizExportFlags.CRYPTUI_WIZ_EXPORT_NO_DELETE_PRIVATE_KEY, Handle, "Export Fiddler Root Certificate", ref exportInfo, IntPtr.Zero);
        }

        private void ConfigurationDialog_Load(object sender, EventArgs e)
        {
            Icon = Owner?.Icon;
            rootHashAlgorithm.Items.Add(HashAlgorithm.SHA1);
            rootAlgorithm.Items.Add(Algorithm.RSA);
            eeAlgorithm.Items.Add(Algorithm.RSA);
            if (PlatformSupport.HasCngSupport)
            {
                eeAlgorithm.Items.Add(Algorithm.ECDSA_P256);
                eeAlgorithm.Items.Add(Algorithm.ECDSA_P384);
                rootAlgorithm.Items.Add(Algorithm.ECDSA_P256);
                rootAlgorithm.Items.Add(Algorithm.ECDSA_P384);
                rootHashAlgorithm.Items.Add(HashAlgorithm.SHA256);
                rootHashAlgorithm.Items.Add(HashAlgorithm.SHA384);
            }
            rootAlgorithm.SelectedItem = CertificateConfiguration.RootCertificateAlgorithm;
            eeAlgorithm.SelectedItem = CertificateConfiguration.EECertificateAlgorithm;
            rootHashAlgorithm.SelectedItem = CertificateConfiguration.RootCertificateHashAlgorithm;
            BindRootKeySize();
            BindEEKeySize();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            var selectedRootHashAlgorithm = (HashAlgorithm)rootHashAlgorithm.SelectedItem;
            var selectedEElgorithm = (Algorithm)eeAlgorithm.SelectedItem;
            var selectedRootAlgorithm = (Algorithm)rootAlgorithm.SelectedItem;
            bool requiresRootRegeneration =
                selectedRootHashAlgorithm != CertificateConfiguration.RootCertificateHashAlgorithm ||
                selectedRootAlgorithm != CertificateConfiguration.RootCertificateAlgorithm ||
                (keySize.Value != CertificateConfiguration.RootRsaKeySize && selectedRootAlgorithm == Algorithm.RSA);
            if (selectedRootHashAlgorithm == HashAlgorithm.SHA1 && selectedRootHashAlgorithm != CertificateConfiguration.RootCertificateHashAlgorithm)
            {
                var result = MessageBox.Show(this, "Browsers are phasing out support for SHA1 certificates and may display warnings or cease to load sites. It is recommended that you use SHA256 or SHA384. Continue anyway?", "Root Certificate Changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result != DialogResult.Yes)
                {
                    return;
                }
            }
            if (requiresRootRegeneration)
            {
                MessageBox.Show(this, "Changing configuration of the root certificate requires disabling HTTPS interception, removing the root certificate, and re-creating it.", "Root Certificate Changes", MessageBoxButtons.OK);
            }
            lock (typeof(CertificateConfiguration))
            {
                if (selectedRootAlgorithm == Algorithm.RSA)
                {
                    CertificateConfiguration.RootRsaKeySize = (int)keySize.Value;
                }
                CertificateConfiguration.RootCertificateHashAlgorithm = selectedRootHashAlgorithm;
                CertificateConfiguration.RootCertificateAlgorithm = selectedRootAlgorithm;

                if (selectedEElgorithm == Algorithm.RSA)
                {
                    CertificateConfiguration.EERsaKeySize = (int)eeKeySize.Value;
                }
                CertificateConfiguration.EECertificateAlgorithm = selectedEElgorithm;
            }
            _certificateProvider.ForceEECertificateClear();
            Close();
        }

        private void rootAlgorithm_SelectionChangeCommitted(object sender, EventArgs e)
        {
            BindRootKeySize();
        }

        private void eeAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindEEKeySize();
        }

    private void BindRootKeySize()
        {
            var algorithm = (Algorithm)rootAlgorithm.SelectedItem;
            if (algorithm != Algorithm.RSA)
            {
                keySize.Enabled = false;
                keySize.Minimum = decimal.MinValue;
                keySize.Maximum = decimal.MaxValue;
                keySize.Increment = 1m;
                if (algorithm == Algorithm.ECDSA_P256) keySize.Value = 256m;
                else if (algorithm == Algorithm.ECDSA_P384) keySize.Value = 384m;
                else if (algorithm == Algorithm.ECDSA_P521) keySize.Value = 521m;
            }
            else
            {
                keySize.Enabled = true;
                keySize.Minimum = 1024m;
                keySize.Maximum = 4096m;
                keySize.Increment = 512m;
                keySize.Value = CertificateConfiguration.RootRsaKeySize;
            }
        }

        private void BindEEKeySize()
        {
            var algorithm = (Algorithm)eeAlgorithm.SelectedItem;
            if (algorithm != Algorithm.RSA)
            {
                eeKeySize.Enabled = false;
                eeKeySize.Minimum = decimal.MinValue;
                eeKeySize.Maximum = decimal.MaxValue;
                eeKeySize.Increment = 1m;
                if (algorithm == Algorithm.ECDSA_P256) eeKeySize.Value = 256m;
                else if (algorithm == Algorithm.ECDSA_P384) eeKeySize.Value = 384m;
                else if (algorithm == Algorithm.ECDSA_P521) eeKeySize.Value = 521m;
            }
            else
            {
                eeKeySize.Enabled = true;
                eeKeySize.Minimum = 1024m;
                eeKeySize.Maximum = 4096m;
                eeKeySize.Increment = 512m;
                eeKeySize.Value = CertificateConfiguration.EERsaKeySize;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
