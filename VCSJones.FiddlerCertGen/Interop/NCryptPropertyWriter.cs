using System;
using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    internal static class NCryptPropertyWriter
    {
        public static void WriteStringUni(NCryptHandleBase handle, string propertyName, string value)
        {
            var ptr = Marshal.StringToHGlobalUni(value);
            try
            {
                if (NCrypt.NCryptSetProperty(handle, propertyName, ptr, (uint)value.Length * 2, 0u) != SECURITY_STATUS.ERROR_SUCCESS)
                {
                    throw new InvalidOperationException("Unable to set property.");
                }

            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }

        public static void WriteUInt32(NCryptHandleBase handle, string propertyName, uint value)
        {
            WriteInt32(handle, propertyName, unchecked((int)value));
        }


        public static void WriteInt32(NCryptHandleBase handle, string propertyName, int value)
        {
            var size = Marshal.SizeOf(typeof(int));
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.WriteInt32(ptr, value);
            try
            {
                if (NCrypt.NCryptSetProperty(handle, propertyName, ptr, (uint)size, 0u) != SECURITY_STATUS.ERROR_SUCCESS)
                {
                    throw new InvalidOperationException("Unable to set property.");
                }
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
        }


        public static void WriteEnum<TEnum>(NCryptHandleBase handle, string propertyName, TEnum value)
        {
            if (!typeof (TEnum).IsEnum)
            {
                throw new ArgumentException("Value must be an enumeration.", nameof(value));
            }
            var type = Enum.GetUnderlyingType(typeof (TEnum));
            if (type == typeof(uint))
            {
                WriteUInt32(handle, propertyName, (uint)(object)value);
            }
            else if (type == typeof(int))
            {
                WriteInt32(handle, propertyName, (int)(object)value);
            }
            else
            {
                throw new NotSupportedException();
            }
        }
    }
}