using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SymMngr.PE
{
    /// <remarks>The StructLayout attribute is mandatory, otherwise this would
    /// trigger an invalid format exxception.</remarks>
    [Serializable()]
    [StructLayout(LayoutKind.Explicit)]
    public abstract class NTHeaderBase : FileHeader
    {
        internal NTHeaderBase(IntPtr native)
        {
            if (IntPtr.Zero == native) { throw new ArgumentNullException(); }
            if (Magic != Marshal.ReadInt32(native)) {
                throw new ArgumentException();
            }
            ExtractDataDirectories(native);
        }

        internal abstract List<DataDirectory> DataDirectories { get; }

        protected abstract int DataDirectoryOffset { get; }

        internal void DumpDataDirectories()
        {
            foreach(DataDirectory _scannedItem in DataDirectories) {
                _scannedItem.Dump();
            }
        }

        protected void ExtractDataDirectories(IntPtr native)
        {
            int dataDirectorySize = Marshal.SizeOf(typeof(DataDirectory));
            for (int index = 0; index < DirectoryEntriesCount; index++) {
                DataDirectory newDirectory = new DataDirectory((DataDirectoryKind)index);
                DataDirectories.Add(
                    Marshal.PtrToStructure<DataDirectory>(
                        native + DataDirectoryOffset + (index * dataDirectorySize)));
            }
        }

        private const int DirectoryEntriesCount = 16;
        private const uint Magic = 0x4550;
    }
}
