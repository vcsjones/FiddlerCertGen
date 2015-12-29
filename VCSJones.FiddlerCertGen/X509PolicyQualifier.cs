namespace VCSJones.FiddlerCertGen
{
    public class X509PolicyQualifier
    {
        public string PolicyQualifierId { get; set; }
        public IA5StringOrByteArray Qualifier { get; set; }

        public X509PolicyQualifier()
        {
        }

        public X509PolicyQualifier(string policyQualifierId, IA5StringOrByteArray qualifier)
        {
            PolicyQualifierId = policyQualifierId;
            Qualifier = qualifier;
        }
    }
}