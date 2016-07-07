using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using SymMngr.Api;

namespace SymMngr.PE
{
    public abstract class NTHeaderBase : FileHeader
    {
        internal NTHeaderBase(IntPtr at)
            : base(at)
        {
            if (IntPtr.Zero == at) { throw new ArgumentNullException(); }
            if (Magic != Marshal.ReadInt32(at)) {
                throw new ArgumentException();
            }
            ExtractDataDirectories(at);
        }

        public IDataDirectory this[DataDirectoryKind kind]
        {
            get
            {
                int index = (int)kind;
                if (0 > index) { throw new ArgumentOutOfRangeException(); }
                if (this.NumberOfRvaAndSize <= index) { return null; }
                return _dataDirectories[index];
            }
        }

        internal List<DataDirectory> DataDirectories
        {
            get
            {
                if (null == _dataDirectories) {
                    _dataDirectories = new List<DataDirectory>();
                }
                return _dataDirectories;
            }
        }

        protected abstract int DataDirectoryOffset { get; }

        internal void DumpDataDirectories()
        {
            foreach(DataDirectory _scannedItem in DataDirectories) {
                _scannedItem.Dump();
            }
        }

        protected void ExtractDataDirectories(IntPtr headerAt)
        {
            int offset = DataDirectoryOffset;
            for (int index = 0; index < DirectoryEntriesCount; index++) {
                DataDirectories.Add(new DataDirectory((DataDirectoryKind)index, headerAt, ref offset));
            }
            return;
        }

        public abstract uint NumberOfRvaAndSize { get; }

        private const int DirectoryEntriesCount = 16;
        private const uint Magic = 0x4550;
        private List<DataDirectory> _dataDirectories;
    }
}
