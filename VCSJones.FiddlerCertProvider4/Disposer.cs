using System;
using System.ComponentModel;

namespace VCSJones.FiddlerCertProvider4
{
    //This class is a work around to being able to add disposal logic to WinForms.
    internal class Disposer : Component
    {
        private readonly Action<bool> _dispose;

        internal Disposer(Action<bool> disposeCallback)
        {
            _dispose = disposeCallback;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _dispose(disposing);
        }
    }
}