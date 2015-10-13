using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;

namespace VCSJones.FiddlerCertProvider4
{
    public partial class ConfigurationDialog : Form
    {
        private X509Certificate2 _root;

        public ConfigurationDialog()
        {
            InitializeComponent();
        }

        public ConfigurationDialog(X509Certificate2 root) : this()
        {
            _root = root;
        }

        private void exportPrivateKey_Click(object sender, EventArgs e)
        {
            var exportInfo = new CRYPTUI_WIZ_EXPORT_INFO();
            exportInfo.context = new CRYPTUI_WIZ_EXPORT_INFO_UNION { pCertContext = _root.Handle };
            exportInfo.cStores = 0;
            exportInfo.dwSize = (uint)Marshal.SizeOf(typeof(CRYPTUI_WIZ_EXPORT_INFO));
            exportInfo.dwSubjectType = CryptUIExportInfoSubjectType.CRYPTUI_WIZ_EXPORT_CERT_CONTEXT;
            exportInfo.pwszExportFileName = null;
            exportInfo.rghStores = IntPtr.Zero;
            NativeMethods.CryptUIWizExport(CryptUIWizExportFlags.CRYPTUI_WIZ_EXPORT_PRIVATE_KEY | CryptUIWizExportFlags.CRYPTUI_WIZ_EXPORT_NO_DELETE_PRIVATE_KEY, this.Handle, "Export PFX", ref exportInfo, IntPtr.Zero);
        }
    }
}
