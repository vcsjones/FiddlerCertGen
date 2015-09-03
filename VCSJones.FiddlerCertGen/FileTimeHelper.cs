using System;
using System.Runtime.InteropServices.ComTypes;

namespace VCSJones.FiddlerCertGen
{
    public static class FileTimeHelper
    {
        public static FILETIME ToFileTimeStructureUtc(DateTime dateTime)
        {
            var value = dateTime.ToFileTimeUtc();
            return new FILETIME
            {
                dwHighDateTime = unchecked((int)((value >> 32) & 0xFFFFFFFF)),
                dwLowDateTime = unchecked((int)(value & 0xFFFFFFFF))
            };
        }
    }
}