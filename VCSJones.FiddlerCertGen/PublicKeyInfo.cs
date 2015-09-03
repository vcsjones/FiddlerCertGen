using System;
using System.Runtime.InteropServices;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    public class PublicKeyInfo : IDisposable
    {
        private readonly CERT_PUBLIC_KEY_INFO _publicKey;
        private readonly IntPtr _buffer;

        internal PublicKeyInfo(IntPtr buffer)
        {
            _publicKey = (CERT_PUBLIC_KEY_INFO)Marshal.PtrToStructure(buffer, typeof(CERT_PUBLIC_KEY_INFO));
            _buffer = buffer;
        }

        internal CERT_PUBLIC_KEY_INFO PublicKey => _publicKey;

        public byte[] Key
        {
            get
            {
                var key = new byte[_publicKey.PublicKey.cbData];
                Marshal.Copy(_publicKey.PublicKey.pbData, key, 0, key.Length);
                return key;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Marshal.FreeHGlobal(_buffer);

        }

        ~PublicKeyInfo()
        {
            Dispose();
        }
    }
}