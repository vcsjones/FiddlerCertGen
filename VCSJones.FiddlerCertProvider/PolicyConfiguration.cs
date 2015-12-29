using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace VCSJones.FiddlerCertProvider
{
    public class PolicyConfiguration
    {

        private readonly XmlDocument _configDocument;
        private static readonly string _configurationPath = Path.Combine(Fiddler.CONFIG.GetPath("Root"), "certPolicies.xml");
        public static PolicyConfiguration Instance { get; } = new PolicyConfiguration(_configurationPath);


        private PolicyConfiguration(string path)
        {
            if (!File.Exists(path))
            {
                CreateEmptyPolicyConfiguration(path);
            }
            _configDocument = new XmlDocument();
            _configDocument.Load(path);
        }

        private void CreateEmptyPolicyConfiguration(string documentPath)
        {
            var emptyPolicy = "<?xml version=\"1.0\" encoding=\"utf-16\" ?>\r\n<policies>\r\n</policies>";
            File.WriteAllText(documentPath, emptyPolicy, System.Text.Encoding.Unicode);
        }

        public void AddPolicy(PolicyModel policy)
        {
            if (PolicyExists(policy.Oid))
            {
                throw new Exception($"Policy with oid {policy.Oid} already exists.");
            }
            var policyElement = _configDocument.CreateElement("policy");
            var policyOidElement = _configDocument.CreateElement("oid");
            policyOidElement.InnerText = policy.Oid;
            policyElement.AppendChild(policyOidElement);
            var qualifiersElement = _configDocument.CreateElement("qualifiers");
            policyElement.AppendChild(qualifiersElement);
            foreach (var qualifier in policy.Qualifiers)
            {
                var qualifierElement = _configDocument.CreateElement("qualifier");
                var qualifierOidElement = _configDocument.CreateElement("oid");
                qualifierOidElement.InnerText = qualifier.QualifierOid;
                qualifierElement.AppendChild(qualifierOidElement);
                qualifiersElement.AppendChild(qualifierElement);
                if (qualifier.Contents?.Length > 0 && qualifier.Type != PolicyQualifierType.None)
                {
                    var qualifierDataElement = _configDocument.CreateElement("data");
                    var qualifierDataValue = _configDocument.CreateCDataSection(Convert.ToBase64String(qualifier.Contents));
                    qualifierDataElement.AppendChild(qualifierDataValue);
                    qualifierElement.AppendChild(qualifierDataElement);
                }
                var qualifierTypeElement = _configDocument.CreateElement("type");
                qualifierTypeElement.InnerText = qualifier.Type.ToString();
                qualifierElement.AppendChild(qualifierTypeElement);
            }
            var rootElement = _configDocument.SelectSingleNode("/policies");
            if (rootElement == null)
            {
                rootElement = _configDocument.CreateElement("policies");
                _configDocument.AppendChild(rootElement);
            }
            rootElement.AppendChild(policyElement);
            _configDocument.Save(_configurationPath);
        }

        public bool PolicyExists(string oid)
        {
            var nodes = _configDocument.SelectNodes("/policies/policy/oid");
            if (nodes == null)
            {
                return false;
            }
            foreach (XmlElement element in nodes)
            {
                if (element.InnerText.Trim() == oid)
                {
                    return true;
                }
            }
            return false;
        }
    }

    public class PolicyModel
    {
        public string Oid { get; set; }
        public IList<PolicyQualifierModel> Qualifiers { get; set; } = new List<PolicyQualifierModel>();
    }

    public class PolicyQualifierModel
    {
        public string QualifierOid { get; set; }
        public byte[] Contents { get; set; }
        public PolicyQualifierType Type { get; set; } 
    }

    public enum PolicyQualifierType
    {
        None,
        Binary,
        String
    }

}
