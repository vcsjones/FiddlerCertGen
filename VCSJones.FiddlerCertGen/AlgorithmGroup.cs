using System;

namespace VCSJones.FiddlerCertGen
{
    public class AlgorithmGroup : IEquatable<AlgorithmGroup>
    {
        internal AlgorithmGroup(string name)
        {
            Name = name;
        }

        internal string Name { get; }

        public static AlgorithmGroup RSA { get; } = new AlgorithmGroup("RSA");
        public static AlgorithmGroup ECDSA { get; } = new AlgorithmGroup("ECDSA");
        public bool Equals(AlgorithmGroup other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AlgorithmGroup)obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(AlgorithmGroup left, AlgorithmGroup right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AlgorithmGroup left, AlgorithmGroup right)
        {
            return !Equals(left, right);
        }
    }
}