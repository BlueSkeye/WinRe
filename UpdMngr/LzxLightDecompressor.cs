using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace UpdMngr
{
    /// <summary>We implement a light wrapper around the cabinet.dll library
    /// for decompression of compressed BLOBs. This class is able to extract
    /// the uncompressed representation from a CAB file stream containing a
    /// single file. No other scenario has been tested.</summary>
    internal class LzxLightDecompressor : IDisposable
    {
        internal LzxLightDecompressor(byte[] data)
        {
            if (null == data) { throw new ArgumentNullException(); }  
            _erf = Marshal.AllocCoTaskMem(12);
            _hDecompressor = FDICreate(
                Marshal.GetFunctionPointerForDelegate(new AllocateDelegate(Allocate)),
                Marshal.GetFunctionPointerForDelegate(new FreeDelegate(Free)),
                Marshal.GetFunctionPointerForDelegate(new OpenDelegate(Open)),
                Marshal.GetFunctionPointerForDelegate(new ReadDelegate(Read)),
                Marshal.GetFunctionPointerForDelegate(new WriteDelegate(Write)),
                Marshal.GetFunctionPointerForDelegate(new CloseDelegate(Close)),
                Marshal.GetFunctionPointerForDelegate(new SeekDelegate(Seek)),
                -1, _erf);
            if (IntPtr.Zero == _hDecompressor) {
                int error = Marshal.GetLastWin32Error();
                throw new ApplicationException();
            }
        }

        ~LzxLightDecompressor()
        {
            Dispose(false);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr AllocateDelegate(uint cb);
        private IntPtr Allocate(uint cb)
        {
            return Marshal.AllocCoTaskMem((int)cb);
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void CloseDelegate(IntPtr hf);
        private void Close(IntPtr hf)
        {
            if (FakeInputHandle == hf) { return; }
            if (FakeOutputHandle == hf) {
                _uncompressedContent = _rawUncompressed.ToArray();
                _rawUncompressed = null;
                return;
            }
            throw new ArgumentException();
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) {
                GC.SuppressFinalize(this);
                _disposed = true;
            }
            if (IntPtr.Zero!= _erf) {
                Marshal.FreeCoTaskMem(_erf);
                _erf = IntPtr.Zero;
            }
            return;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate void FreeDelegate(IntPtr pv);
        private void Free(IntPtr pv)
        {
            Marshal.FreeCoTaskMem(pv);
        }

        public byte[] GetUncompressedContent()
        {
            if (_disposed) { throw new ObjectDisposedException("LzxLightDecompressor"); }
            if (null == _uncompressedContent) {
                if (IntPtr.Zero == _hDecompressor) {
                    throw new InvalidOperationException();
                }
                try {
                    int copyResult =
                    FDICopy(_hDecompressor, "Test.cab", @"C:\TEMP\", 0,
                        Marshal.GetFunctionPointerForDelegate(new NotifyDelegate(Notify)),
                        IntPtr.Zero, IntPtr.Zero);
                    if (0 == copyResult) {
                        int error = Marshal.GetLastWin32Error();
                        throw new ApplicationException();
                    }
                }
                finally {
                    FDIDestroy(_hDecompressor);
                    _hDecompressor = IntPtr.Zero;
                }
            }
            return _uncompressedContent;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate IntPtr NotifyDelegate(int fdint, IntPtr pfdin);
        private IntPtr Notify(int fdint, IntPtr pfdin)
        {
            switch (fdint) {
                case 0:
                    return IntPtr.Zero;
                case 1:
                    throw new NotSupportedException();
                case 2:
                    _rawUncompressed = new List<byte>();
                    return FakeOutputHandle;
                case 3:
                    return new IntPtr(0xFF);
                case 4:
                    throw new NotSupportedException();
                case 5:
                    return IntPtr.Zero;
                default:
                    throw new ArgumentException();
            }
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private delegate IntPtr OpenDelegate(string pszFile, int oflag, int pmode);
        private IntPtr Open(string pszFile, int oflag, int pmode)
        {
            return FakeInputHandle;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint ReadDelegate(IntPtr hf, IntPtr pv, uint cb);
        private uint Read(IntPtr hf, IntPtr pv, uint cb)
        {
            if (FakeInputHandle != hf) { throw new ArgumentException(); }
            uint effectiveSize = Math.Min(cb, (uint)_rawInput.Length - _rawInputPosition);
            Marshal.Copy(_rawInput, (int)_rawInputPosition, pv, (int)effectiveSize);
            _rawInputPosition += effectiveSize;
            return effectiveSize;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate int SeekDelegate(IntPtr hf, int dist, int seektype);
        private int Seek(IntPtr hf, int dist, int seektype)
        {
            if (FakeInputHandle != hf) { throw new ArgumentException(); }
            switch (seektype) {
                case 0:
                    _rawInputPosition = (uint)Math.Max(0, Math.Min((int)_rawInput.Length, dist));
                    break;
                case 1:
                    _rawInputPosition = (uint)Math.Max(0,
                        Math.Min((int)_rawInput.Length, dist + _rawInputPosition));
                    break;
                case 2:
                    _rawInputPosition = (uint)Math.Max(0,
                        Math.Min((int)_rawInput.Length, dist + _rawInput.Length));
                    break;
                default:
                    throw new ArgumentException();
            }
            return (int)_rawInputPosition;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate uint WriteDelegate(IntPtr hf, IntPtr pv, uint cb);
        private uint Write(IntPtr hf, IntPtr pv, uint cb)
        {
            if (FakeOutputHandle != hf) { throw new ArgumentException(); }
            byte[] localBuffer = new byte[(int)cb];
            Marshal.Copy(pv, localBuffer, 0, (int)cb);
            _rawUncompressed.AddRange(localBuffer);
            return cb;
        }

        private bool _disposed;
        private IntPtr _erf;
        private IntPtr _hDecompressor;
        private byte[] _rawInput;
        private uint _rawInputPosition = 0;
        private List<byte> _rawUncompressed;
        private byte[] _uncompressedContent;
        private static IntPtr FakeInputHandle = new IntPtr(1);
        private static IntPtr FakeOutputHandle = new IntPtr(2);

        [DllImport("Cabinet.dll", SetLastError = true, PreserveSig = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int FDICopy(
            [In] IntPtr hfdi,
            [In] string pszCabinet,
            [In] string pszCabPath,
            [In] int flags,
            [In] IntPtr /* PFNFDINOTIFY */ pfnfdin,
            [In] IntPtr pfnfdid,
            [In] IntPtr pvUser);
        [DllImport("Cabinet.dll", SetLastError = true, PreserveSig = true, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr FDICreate(
            [In] IntPtr pfnalloc,
            [In] IntPtr pfnfree,
            [In] IntPtr pfnopen,
            [In] IntPtr pfnread,
            [In] IntPtr pfnwrite,
            [In] IntPtr pfnclose,
            [In] IntPtr pfnseek,
            [In] int cpuType,
            [In] IntPtr perf);
        [DllImport("Cabinet.dll", SetLastError = true, PreserveSig = true)]
        private static extern bool FDIDestroy(
            [In] IntPtr hfdi);
    }
}
