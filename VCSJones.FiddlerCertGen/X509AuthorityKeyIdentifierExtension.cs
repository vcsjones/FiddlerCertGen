using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    public class X509AuthorityKeyIdentifierExtension : X509Extension
    {
        public X509AuthorityKeyIdentifierExtension(byte[] publicKeyId, byte[] serialNumber)
            : base(OIDs.szOID_AUTHORITY_KEY_IDENTIFIER2, EncodeData(publicKeyId, serialNumber), false)
        {
        }

        private static byte[] EncodeData(byte[] publicKey, byte[] serialNumber)
        {
            IntPtr publicKeyPtr = IntPtr.Zero, serialNumberPtr = IntPtr.Zero;
            try
            {
                var identifier = new CERT_AUTHORITY_KEY_ID2_INFO();
                identifier.AuthorityCertIssuer = new CERT_ALT_NAME_INFO { cAltEntry = 0 };
                if (serialNumber != null)
                {
                    serialNumberPtr = Marshal.AllocHGlobal(serialNumber.Length);
                    Marshal.Copy(serialNumber, 0, serialNumberPtr, serialNumber.Length);
                    identifier.AuthorityCertSerialNumber = new CRYPTOAPI_BLOB { cbData = (uint)serialNumber.Length, pbData = serialNumberPtr };

                }
                if (publicKey != null)
                {
                    publicKeyPtr = Marshal.AllocHGlobal(publicKey.Length);
                    Marshal.Copy(publicKey, 0, publicKeyPtr, publicKey.Length);
                    identifier.KeyId = new CRYPTOAPI_BLOB { cbData = (uint)publicKey.Length, pbData = publicKeyPtr };

                }
                uint dataSize = 0;
                IntPtr data;
                if (!Crypt32.CryptEncodeObjectEx(EncodingType.X509_ASN_ENCODING, OIDs.szOID_AUTHORITY_KEY_IDENTIFIER2, ref identifier, 0x8000, IntPtr.Zero, out data, ref dataSize))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                var buffer = new byte[dataSize];
                Marshal.Copy(data, buffer, 0, (int)dataSize);
                return buffer;
            }
            finally
            {
                Marshal.FreeHGlobal(publicKeyPtr);
                Marshal.FreeHGlobal(serialNumberPtr);
            }
        }
    }
}