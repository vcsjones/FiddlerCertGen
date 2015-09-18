namespace VCSJones.FiddlerCertGen
{
    public enum X509AlternativeNameType
    {
        //CERT_ALT_NAME_OTHER_NAME = 1,
        //CERT_ALT_NAME_RFC822_NAME = 2,
        DnsName = 3,
        //CERT_ALT_NAME_X400_ADDRESS = 4,
        //CERT_ALT_NAME_DIRECTORY_NAME = 5,
        //CERT_ALT_NAME_EDI_PARTY_NAME = 6,
        Url = 7,
        IpAddress = 8,
        //CERT_ALT_NAME_REGISTERED_ID = 9,
    }
}