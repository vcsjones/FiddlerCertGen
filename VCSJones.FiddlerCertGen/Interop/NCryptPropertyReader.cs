using System;
using System.Runtime.InteropServices;

namespace VCSJones.FiddlerCertGen.Interop
{
    internal static class NCryptPropertyReader
    {
        public static string ReadStringUni(NCryptHandleBase handle, string propertyName)
        {
            return Read(handle, propertyName, Marshal.PtrToStringUni);
        }

        public static int ReadInt32(NCryptHandleBase handle, string propertyName)
        {
            return Read(handle, propertyName, Marshal.ReadInt32);
        }

        public static T Read<T>(NCryptHandleBase handle, string propertyName, Func<IntPtr, T> convert)
        {
            var buffer = IntPtr.Zero;
            uint size;
            if (NCrypt.NCryptGetProperty(handle, propertyName, buffer, 0, out size, 0u) != SECURITY_STATUS.ERROR_SUCCESS)
            {
                throw new InvalidOperationException("Unable to query property.");
            }
            buffer = Marshal.AllocHGlobal((int)size);
            try
            {
                if (NCrypt.NCryptGetProperty(handle, propertyName, buffer, size, out size, 0u) != SECURITY_STATUS.ERROR_SUCCESS)
                {
                    throw new InvalidOperationException("Unable to query property.");
                }
                return convert(buffer);
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
    }
}