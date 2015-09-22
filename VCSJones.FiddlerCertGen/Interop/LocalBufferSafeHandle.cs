using System;
using Microsoft.Win32.SafeHandles;

namespace VCSJones.FiddlerCertGen.Interop
{
    internal sealed class LocalBufferSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public LocalBufferSafeHandle(bool ownsHandle) : base(ownsHandle)
        {
        }

        public LocalBufferSafeHandle() : this(true)
        {
        }

        protected override bool ReleaseHandle()
        {
            return IntPtr.Zero == Kernel32.LocalFree(handle);
        }
    }
}