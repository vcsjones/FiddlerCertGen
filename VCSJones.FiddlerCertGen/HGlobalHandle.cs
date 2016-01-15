using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

namespace VCSJones.FiddlerCertGen
{
    public class HGlobalHandle : SafeHandleZeroOrMinusOneIsInvalid
    {
        public HGlobalHandle(bool ownsHandle) : base(ownsHandle)
        {
        }

        public static HGlobalHandle Alloc(int size)
        {
            var hGlobalHandle = new HGlobalHandle(true);
            hGlobalHandle.SetHandle(Marshal.AllocHGlobal(size));
            return hGlobalHandle;
        }


        protected override bool ReleaseHandle()
        {
            Marshal.FreeHGlobal(handle);
            return true;
        }
    }
}