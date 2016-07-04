using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

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
                _file = File.Open(from.FullName, FileMode.Open, FileAccess.Read);
                _mappedFile = MemoryMappedFile.CreateFromFile(_file, Guid.NewGuid().ToString(),
                    0, MemoryMappedFileAccess.Read, null, HandleInheritability.None, false);
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
                            Marshal.PtrToStructure<NTHeader32>(nativeHeader, (NTHeader32)_ntHeader);
                            break;
                        case Machine.X64:
                            _ntHeader = new NTHeader64(nativeHeader);
                            Marshal.PtrToStructure<NTHeader64>(nativeHeader, (NTHeader64)_ntHeader);
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
                        TimeSpan.FromSeconds(Natives.DbgHelp.GetTimestampForLoadedLibrary(
                            _mappedFile.SafeMemoryMappedFileHandle.DangerousGetHandle()));
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
            if (null != _mappedFile) {
                _mappedFile.Dispose();
                _mappedFile = null;
            }
            if (null != _file) {
                _file.Close();
                _file = null;
            }
        }

        private FileStream _file;
        private IntPtr _imageBase;
        private MemoryMappedFile _mappedFile;
        private INTHeader _ntHeader;
    }
}
