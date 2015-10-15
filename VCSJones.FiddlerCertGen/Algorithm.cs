using System;

namespace VCSJones.FiddlerCertGen
{
    public class Algorithm : IEquatable<Algorithm>
    {
        private Algorithm(string name)
        {
            Name = name;
        }

        public static bool TryParse(string algorithmName, out Algorithm algorithm)
        {
            switch (algorithmName)
            {
                case nameof(RSA):
                    algorithm = RSA;
                    return true;
                case nameof(ECDSA_P256):
                    algorithm = ECDSA_P256;
                    return true;
                case nameof(ECDSA_P384):
                    algorithm = ECDSA_P384;
                    return true;
                case nameof(ECDSA_P521):
                    algorithm = ECDSA_P521;
                    return true;
                default:
                    algorithm = default(Algorithm);
                    return false;
            }
        }

        internal string Name { get; }

        public static Algorithm RSA { get; } = new Algorithm(nameof(RSA));
        public static Algorithm ECDSA_P256 { get; } = new Algorithm(nameof(ECDSA_P256));
        public static Algorithm ECDSA_P384 { get; } = new Algorithm(nameof(ECDSA_P384));
        public static Algorithm ECDSA_P521 { get; } = new Algorithm(nameof(ECDSA_P521));

        public bool Equals(Algorithm other)
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
            return Equals((Algorithm) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(Algorithm left, Algorithm right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Algorithm left, Algorithm right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}