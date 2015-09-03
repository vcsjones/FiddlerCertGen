using System;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    internal class MarshalX509Extension : IDisposable
    {
        private readonly X509Extension _extension;
        private readonly CERT_EXTENSION _value;
        private readonly IntPtr _blobPtr;
        private bool _isDisposed = false;

        public MarshalX509Extension(X509Extension extension)
        {
            _extension = extension;
            _blobPtr = Marshal.AllocHGlobal(extension.RawData.Length);
            Marshal.Copy(extension.RawData, 0, _blobPtr, extension.RawData.Length);
            var blob = new CRYPT_OBJID_BLOB();
            blob.cbData = (uint)extension.RawData.Length;
            blob.pbData = _blobPtr;
            var nativeExtension = new CERT_EXTENSION();
            nativeExtension.fCritical = extension.Critical;
            nativeExtension.pszObjId = extension.Oid.Value;
            nativeExtension.Value = blob;
            _value = nativeExtension;
        }

        public CERT_EXTENSION Value
        {
            get
            {
                if (_isDisposed)
                {
                    throw new ObjectDisposedException("blob");
                }
                return _value;
            }
        }

        public X509Extension Extension => _extension;

        public void Dispose()
        {
            _isDisposed = true;
            Marshal.FreeHGlobal(_blobPtr);
            GC.SuppressFinalize(this);
        }

        ~MarshalX509Extension()
        {
            Dispose();
        }
    }
}