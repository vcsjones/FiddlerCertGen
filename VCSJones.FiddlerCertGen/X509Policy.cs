using System.Collections.Generic;

namespace VCSJones.FiddlerCertGen
{
    public class X509Policy
    {
        public string PolicyIdentifier { get; set; }
        public IList<X509PolicyQualifier> PolicyQualifier { get; set; }

        public X509Policy()
        {
        }

        public X509Policy(string policyIdentifier, IList<X509PolicyQualifier> policyQualifier)
        {
            PolicyIdentifier = policyIdentifier;
            PolicyQualifier = policyQualifier;
        }
    }
}