using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    public class X509PoliciesExtension : X509Extension
    {
        public X509PoliciesExtension(IList<X509Policy> policies, bool critical) : base(OIDs.szOID_CERT_POLICIES, EncodeExtension(policies), critical)
        {
        }

        private static byte[] EncodeExtension(IList<X509Policy> policies)
        {
            var policiesInfo = new CERT_POLICIES_INFO();
            policiesInfo.cPolicyInfo = (uint) policies.Count;
            var policySize = Marshal.SizeOf(typeof (CERT_POLICY_INFO));
            var qualifierSize = Marshal.SizeOf(typeof (CERT_POLICY_QUALIFIER_INFO));
            var policiesBuffer = Marshal.AllocHGlobal(policySize*policies.Count);
            var freeValues = new List<IntPtr> {policiesBuffer};
            var disposePool = new List<IDisposable>();
            try
            {
                for (int index = 0, offset = 0; index < policies.Count; index++, offset += policySize)
                {
                    var policy = new CERT_POLICY_INFO();
                    policy.cPolicyQualifier = (uint) (policies[index].PolicyQualifier?.Count ?? 0);
                    policy.pszPolicyIdentifier = policies[index].PolicyIdentifier;
                    var qualifierBuffer = IntPtr.Zero;
                    if (policies[index].PolicyQualifier?.Count > 0)
                    {
                        qualifierBuffer = Marshal.AllocHGlobal(policies[index].PolicyQualifier.Count*qualifierSize);
                        freeValues.Add(qualifierBuffer);
                        for (int qualifierIndex = 0, qualifierOffset = 0; qualifierIndex < policies[index].PolicyQualifier.Count; qualifierIndex++, qualifierOffset += qualifierSize)
                        {
                            var currentQualifier = policies[index].PolicyQualifier[qualifierIndex];
                            var qualifierInfo = new CERT_POLICY_QUALIFIER_INFO();
                            qualifierInfo.pszPolicyQualifierId = currentQualifier.PolicyQualifierId;
                            var qualifier = new CRYPT_OBJID_BLOB();
                            int size;
                            var nativeQualifier = currentQualifier.Qualifier.ToNative(out size);
                            disposePool.Add(nativeQualifier);
                            qualifier.cbData = (uint) size;
                            qualifier.pbData = nativeQualifier.DangerousGetHandle();
                            qualifierInfo.Qualifier = qualifier;
                            Marshal.StructureToPtr(qualifierInfo, IntPtrArithmetic.Add(qualifierBuffer, qualifierOffset), false);
                        }
                    }
                    policy.rgPolicyQualifier = qualifierBuffer;
                    Marshal.StructureToPtr(policy, IntPtrArithmetic.Add(policiesBuffer, offset), false);
                }
                policiesInfo.rgPolicyInfo = policiesBuffer;
                uint dataSize = 0;
                LocalBufferSafeHandle data;
                if (!Crypt32.CryptEncodeObjectEx(EncodingType.X509_ASN_ENCODING, OIDs.szOID_CERT_POLICIES, ref policiesInfo, 0x8000, IntPtr.Zero, out data, ref dataSize))
                {
                    throw new Win32Exception(Marshal.GetLastWin32Error());
                }
                using (data)
                {
                    var buffer = new byte[dataSize];
                    Marshal.Copy(data.DangerousGetHandle(), buffer, 0, (int)dataSize);
                    return buffer;
                }
            }
            finally
            {
                freeValues.ForEach(Marshal.FreeHGlobal);
                disposePool.ForEach(m => m.Dispose());
            }
        }
    }
}