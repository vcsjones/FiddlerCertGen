using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using VCSJones.FiddlerCertGen.Interop;

namespace VCSJones.FiddlerCertGen
{
    internal class MarshalX509ExtensionCollection : IDisposable
    {
        private class Freezer : IDisposable
        {
            private readonly MarshalX509ExtensionCollection _collection;

            public Freezer(MarshalX509ExtensionCollection collection)
            {
                _collection = collection;
                _collection._frozen = true;
            }

            public void Dispose()
            {
                _collection._frozen = false;
                _collection.RebuildExtensions();
            }
        }

        private bool _frozen;
        private IntPtr _blob;
        private readonly List<MarshalX509Extension> _marshaledExtensions = new List<MarshalX509Extension>();

        private void RebuildExtensions()
        {
            if (_frozen)
            {
                return;
            }
            Marshal.FreeHGlobal(_blob);
            var structSize = Marshal.SizeOf(typeof(CERT_EXTENSION));
            _blob = Marshal.AllocHGlobal(structSize * _marshaledExtensions.Count);
            for (int index = 0, offset = 0; index < _marshaledExtensions.Count; index++, offset += structSize)
            {
                var marshalX509Extension = _marshaledExtensions[index];
                Marshal.StructureToPtr(marshalX509Extension.Value, IntPtrArithmetic.Add(_blob, offset), false);
            }
        }

        public IDisposable Freeze()
        {
            return new Freezer(this);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        public CERT_EXTENSIONS Extensions => new CERT_EXTENSIONS { cExtension = (uint)Count, rgExtension = _blob };

        private void Dispose(bool disposing)
        {
            GC.SuppressFinalize(this);
            if (disposing)
            {
                foreach (var marshalX509Extension in _marshaledExtensions)
                {
                    marshalX509Extension.Dispose();
                }
                _marshaledExtensions.Clear();
            }
            Marshal.FreeHGlobal(_blob);
        }

        public void Add(X509Extension item)
        {
            _marshaledExtensions.Add(new MarshalX509Extension(item));
            RebuildExtensions();
        }

        public void Clear()
        {
            _marshaledExtensions.Clear();
            RebuildExtensions();
        }

        public int Count => _marshaledExtensions.Count;

        ~MarshalX509ExtensionCollection()
        {
            Dispose(false);
        }
    }

    public static class IntPtrArithmetic
    {
        public static IntPtr Add(IntPtr ptr, int val)
        {
            return (IntPtr)(ptr.ToInt64() + val);
        }
    }

}