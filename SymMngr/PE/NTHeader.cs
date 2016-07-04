using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace SymMngr.PE
{
    [Serializable()]
    [StructLayout(LayoutKind.Explicit)]
    public abstract class _NTHeader32 : NTHeaderBase, INTHeader
    {
        internal _NTHeader32(IntPtr native)
            : base(native)
        {
            return;
        }

        protected override int DataDirectoryOffset
        {
            get { return 0x80; }
        }

        // OPTIONAL HEADER
        [FieldOffset(0x20)]
        private ushort _magic;
        [FieldOffset(0x22)]
        private byte _majorLinkerVersion;
        [FieldOffset(0x23)]
        private byte _minorLinkerVersion;
        [FieldOffset(0x24)]
        private uint _sizeOfCode;
        [FieldOffset(0x28)]
        private uint _sizeOfInitializedData;
        [FieldOffset(0x2C)]
        private uint _sizeOfUninitializedData;
        [FieldOffset(0x30)]
        private uint _addressOfEntryPoint;
        [FieldOffset(0x34)]
        private uint _baseOfCode;
        [FieldOffset(0x38)]
        private uint _baseOfData;
        [FieldOffset(0x3C)]
        private uint _imageBase;
        [FieldOffset(0x40)]
        private uint _sectionAlignment;
        [FieldOffset(0x44)]
        private uint _fileAlignment;
        [FieldOffset(0x48)]
        private ushort _majorOperatingSystemVersion;
        [FieldOffset(0x4A)]
        private ushort _minorOperatingSystemVersion;
        [FieldOffset(0x4C)]
        private ushort _majorImageVersion;
        [FieldOffset(0x4E)]
        private ushort _minorImageVersion;
        [FieldOffset(0x50)]
        private ushort _majorSubsystemVersion;
        [FieldOffset(0x52)]
        private ushort _minorSubsystemVersion;
        [FieldOffset(0x54)]
        private uint _win32VersionValue;
        [FieldOffset(0x58)]
        private uint _sizeOfImage;
        [FieldOffset(0x5C)]
        private uint _sizeOfHeaders;
        [FieldOffset(0x60)]
        private uint _checkSum;
        [FieldOffset(0x64)]
        private ushort _subsystem;
        [FieldOffset(0x66)]
        private ushort _dllCharacteristics;
        [FieldOffset(0x68)]
        private uint _sizeOfStackReserve;
        [FieldOffset(0x6C)]
        private uint _sizeOfStackCommit;
        [FieldOffset(0x70)]
        private uint _sizeOfHeapReserve;
        [FieldOffset(0x74)]
        private uint _sizeOfHeapCommit;
        [FieldOffset(0x78)]
        private uint _loaderFlags;
        [FieldOffset(0x7C)]
        private uint _numberOfRvaAndSizes;
    }

    public class NTHeader32 : _NTHeader32, INTHeader
    {
        internal  NTHeader32(IntPtr native)
            : base (native)
        {
            return;
        }

        internal override List<DataDirectory> DataDirectories
        {
            get
            {
                if (null == _dataDirectories) {
                    _dataDirectories = new List<DataDirectory>();
                }
                return _dataDirectories;
            }
        }

        private List<DataDirectory> _dataDirectories;
    }

    [Serializable()]
    [StructLayout(LayoutKind.Explicit)]
    public abstract class _NTHeader64 : NTHeaderBase, INTHeader
    {
        internal _NTHeader64(IntPtr native)
            : base(native)
        {
            return;
        }

        protected override int DataDirectoryOffset
        {
            get { return 0x90; }
        }

        // OPTIONAL HEADER
        [FieldOffset(0x20)]
        private ushort _magic;
        [FieldOffset(0x22)]
        private byte _majorLinkerVersion;
        [FieldOffset(0x23)]
        private byte _minorLinkerVersion;
        [FieldOffset(0x24)]
        private uint _sizeOfCode;
        [FieldOffset(0x28)]
        private uint _sizeOfInitializedData;
        [FieldOffset(0x2C)]
        private uint _sizeOfUninitializedData;
        [FieldOffset(0x30)]
        private uint _addressOfEntryPoint;
        [FieldOffset(0x34)]
        private uint _baseOfCode;
        // Nota : No base of data.
        [FieldOffset(0x38)]
        private ulong _imageBase;
        [FieldOffset(0x40)]
        private uint _sectionAlignment;
        [FieldOffset(0x44)]
        private uint _fileAlignment;
        [FieldOffset(0x48)]
        private ushort _majorOperatingSystemVersion;
        [FieldOffset(0x4A)]
        private ushort _minorOperatingSystemVersion;
        [FieldOffset(0x4C)]
        private ushort _majorImageVersion;
        [FieldOffset(0x4E)]
        private ushort _minorImageVersion;
        [FieldOffset(0x50)]
        private ushort _majorSubsystemVersion;
        [FieldOffset(0x52)]
        private ushort _minorSubsystemVersion;
        [FieldOffset(0x54)]
        private uint _win32VersionValue;
        [FieldOffset(0x58)]
        private uint _sizeOfImage;
        [FieldOffset(0x5C)]
        private uint _sizeOfHeaders;
        [FieldOffset(0x60)]
        private uint _checkSum;
        [FieldOffset(0x64)]
        private ushort _subsystem;
        [FieldOffset(0x66)]
        private ushort _dllCharacteristics;
        [FieldOffset(0x68)]
        private ulong _sizeOfStackReserve;
        [FieldOffset(0x70)]
        private ulong _sizeOfStackCommit;
        [FieldOffset(0x78)]
        private ulong _sizeOfHeapReserve;
        [FieldOffset(0x80)]
        private ulong _sizeOfHeapCommit;
        [FieldOffset(0x88)]
        private uint _loaderFlags;
        [FieldOffset(0x8C)]
        private uint _numberOfRvaAndSizes;
    }

    public class NTHeader64 : _NTHeader64, INTHeader
    {
        internal  NTHeader64(IntPtr native)
            : base (native)
        {
            return;
        }

        internal override List<DataDirectory> DataDirectories
        {
            get
            {
                if (null == _dataDirectories) {
                    _dataDirectories = new List<DataDirectory>();
                }
                return _dataDirectories;
            }
        }

        private List<DataDirectory> _dataDirectories;
    }

}
