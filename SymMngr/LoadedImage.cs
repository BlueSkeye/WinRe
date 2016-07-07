using System;
using System.IO;
using System.Runtime.InteropServices;

using SymMngr.Api;
using SymMngr.Natives;
using SymMngr.PE;

namespace SymMngr
{
    internal class LoadedImage : IDisposable
    {
        internal LoadedImage(FileInfo from)
        {
            if (null == from) { throw new ArgumentNullException(); }
            if (!from.Exists) { throw new ArgumentException(); }
            try {
                // Nota : library loading provides an handle. This is not really a base address.
                // The least significant bits seems to be used for some kind of flags depending
                // on value of last function parameter.
                _hModule = Kernel32.LoadLibraryEx(from.FullName, IntPtr.Zero,
                    Kernel32.LoadFlags.DontResolveDllReferences
                    | Kernel32.LoadFlags.IgnoreCodeAuthorizationLevel
                    | Kernel32.LoadFlags.LoadLibraryAsImageResources);
                _imageBase = _hModule - (int)((ulong)_hModule.ToInt64() % (ulong)IntPtr.Size);
            }
            catch {
                Dispose(true);
                throw;
            }
        }

        ~LoadedImage()
        {
            Dispose(false);
        }

        internal INTHeader NTHeader
        {
            get
            {
                if (null == _ntHeader) {
                    _nativeHeader = Natives.DbgHelp.ImageNtHeader(_imageBase);
                    switch (FileHeader.LookupMachineKind(_nativeHeader)) {
                        case Machine.I386:
                            _ntHeader = new NTHeader32(_nativeHeader);
                            break;
                        case Machine.X64:
                            _ntHeader = new NTHeader64(_nativeHeader);
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }
                return _ntHeader;
            }
        }

        internal DateTime Timestamp
        {
            get
            {
                lock (Globals.DebugHelpLock) {
                    return Constants.Epoch +
                        TimeSpan.FromSeconds(Natives.DbgHelp.GetTimestampForLoadedLibrary(_imageBase));
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) { GC.SuppressFinalize(this); }
            if (IntPtr.Zero != _hModule) {
                Kernel32.FreeLibrary(_hModule);
                _hModule = IntPtr.Zero;
                _imageBase = IntPtr.Zero;
            }
        }

        private void EnsureNativeHeader()
        {
            if (IntPtr.Zero == _nativeHeader) {
                // Retrieving NTHeader property will initialize the _nativeHeader member.
                INTHeader trash = NTHeader;
            }
            return;
        }

        internal IntPtr MapRva(uint rva)
        {
            return _imageBase + (int)rva;
            //EnsureNativeHeader();
            //IntPtr result = Natives.DbgHelp.ImageRvaToVa(_nativeHeader, _imageBase, rva, ref _lastRvaSection);
            //if (IntPtr.Zero != result) { return result; }
            //int errorCode = Marshal.GetLastWin32Error();
            //throw new ApplicationException(string.Format("Error : {0}", errorCode));
        }

        private IntPtr _hModule;
        private IntPtr _imageBase;
        private IntPtr _lastRvaSection;
        private IntPtr _nativeHeader;
        private INTHeader _ntHeader;
    }
}
