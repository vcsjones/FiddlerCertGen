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

        public static LocalBufferSafeHandle Null
        {
            get
            {
                var buffer = new LocalBufferSafeHandle(true);
                buffer.SetHandle(IntPtr.Zero);
                return buffer;
            }
        }


        public static LocalBufferSafeHandle Alloc(long size)
        {
            var native = Kernel32.LocalAlloc(0u, new IntPtr(size));
            var buffer = new LocalBufferSafeHandle(true);
            buffer.SetHandle(native);
            return buffer;
        }

        protected override bool ReleaseHandle()
        {
            return IntPtr.Zero == Kernel32.LocalFree(handle);
        }
    }
}