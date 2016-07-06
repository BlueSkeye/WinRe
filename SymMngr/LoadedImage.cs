using System;
using System.IO;
using System.Runtime.InteropServices;

using SymMngr.PE;
using SymMngr.Natives;

namespace SymMngr
{
    internal class LoadedImage : IDisposable
    {
        internal LoadedImage(FileInfo from)
        {
            if (null == from) { throw new ArgumentNullException(); }
            if (!from.Exists) { throw new ArgumentException(); }
            try {
                _imageBase = Kernel32.LoadLibraryEx(from.FullName, IntPtr.Zero,
                    Kernel32.LoadFlags.DontResolveDllReferences
                    | Kernel32.LoadFlags.IgnoreCodeAuthorizationLevel
                    | Kernel32.LoadFlags.LoadLibraryAsImageResources);
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
                    IntPtr nativeHeader = Natives.DbgHelp.ImageNtHeader(_imageBase);
                    switch (FileHeader.LookupMachineKind(nativeHeader)) {
                        case Machine.I386:
                            _ntHeader = new NTHeader32(nativeHeader);
                            break;
                        case Machine.X64:
                            _ntHeader = new NTHeader64(nativeHeader);
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
            if (IntPtr.Zero != _imageBase) {
                Kernel32.FreeLibrary(_imageBase);
                _imageBase = IntPtr.Zero;
            }
        }

        private IntPtr _imageBase;
        private INTHeader _ntHeader;
    }
}
