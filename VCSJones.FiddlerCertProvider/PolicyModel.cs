using System;
using System.Collections.Generic;

namespace VCSJones.FiddlerCertProvider
{
    public class PolicyModel : IEquatable<PolicyModel>
    {
        public bool Equals(PolicyModel other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Oid, other.Oid);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((PolicyModel) obj);
        }

        public override int GetHashCode()
        {
            return (Oid != null ? Oid.GetHashCode() : 0);
        }

        public static bool operator ==(PolicyModel left, PolicyModel right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(PolicyModel left, PolicyModel right)
        {
            return !Equals(left, right);
        }

        public string Oid { get; set; }
        public IList<PolicyQualifierModel> Qualifiers { get; set; } = new List<PolicyQualifierModel>();
    }
}