using System;
using System.Runtime.InteropServices;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    public class IA5StringOrByteArray : IComparable<IA5StringOrByteArray>, IEquatable<IA5StringOrByteArray>
    {
        private readonly byte[] _byteValue;

        public int CompareTo(IA5StringOrByteArray other)
        {
            if (_byteValue.Length != other._byteValue.Length)
            {
                return _byteValue.Length.CompareTo(other._byteValue.Length);
            }
            return Kernel32.memcmp(_byteValue, other._byteValue, new IntPtr(_byteValue.Length));
        }

        private IA5StringOrByteArray(byte[] value)
        {
            _byteValue = value;
        }

        private IA5StringOrByteArray(string value)
        {
            _byteValue = IA5StringEncoding.Encode(value);
        }

        public static implicit operator IA5StringOrByteArray(byte[] bytes)
        {
            return new IA5StringOrByteArray(bytes);
        }

        public static implicit operator IA5StringOrByteArray(string str)
        {
            return new IA5StringOrByteArray(str);
        }

        public bool Equals(IA5StringOrByteArray other)
        {
            return CompareTo(other) == 0;
        }

        public override string ToString() => BitConverter.ToString(_byteValue);

        public bool TryIA5Parse(out string value)
        {
            try
            {
                value = IA5StringEncoding.Decode(_byteValue);
                return true;
            }
            catch
            {
                value = null;
                return false;
            }
        }

        public override int GetHashCode() => _byteValue.GetHashCode();

        public override bool Equals(object obj)
        {
            if (!(obj is IA5StringOrByteArray)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return Equals((IA5StringOrByteArray)obj);
        }

        public HGlobalHandle ToNative(out int size)
        {
            size = _byteValue.Length;
            var handle = HGlobalHandle.Alloc(size);
            Marshal.Copy(_byteValue, 0, handle.DangerousGetHandle(), _byteValue.Length);
            return handle;
        }

        public byte[] ToByteArray()
        {
            return (byte[]) _byteValue.Clone();
        }
    }
}