using System;

using SymMngr.Api;

namespace SymMngr.PE
{
    public class NTHeader32 : NTHeaderBase, INTHeader
    {
        internal NTHeader32(IntPtr at)
            : base(at)
        {
            // Forget Marshal.PtrToStructure. This won't work due to the
            // _dataDirectories field in the base class.
            int offset = 0x18;
            _magic = ImageHelpers.ReadUint16(at, ref offset);
            _majorLinkerVersion = ImageHelpers.ReadByte(at, ref offset);
            _minorLinkerVersion = ImageHelpers.ReadByte(at, ref offset);
            _sizeOfCode = ImageHelpers.ReadUint32(at, ref offset);
            _sizeOfInitializedData = ImageHelpers.ReadUint32(at, ref offset);
            _sizeOfUninitializedData = ImageHelpers.ReadUint32(at, ref offset);
            _addressOfEntryPoint = ImageHelpers.ReadUint32(at, ref offset);
            _baseOfCode = ImageHelpers.ReadUint32(at, ref offset);
            _baseOfData = ImageHelpers.ReadUint32(at, ref offset);
            _imageBase = ImageHelpers.ReadUint32(at, ref offset);
            _sectionAlignment = ImageHelpers.ReadUint32(at, ref offset);
            _fileAlignment = ImageHelpers.ReadUint32(at, ref offset);
            _majorOperatingSystemVersion = ImageHelpers.ReadUint16(at, ref offset);
            _minorOperatingSystemVersion = ImageHelpers.ReadUint16(at, ref offset);
            _majorImageVersion = ImageHelpers.ReadUint16(at, ref offset);
            _minorImageVersion = ImageHelpers.ReadUint16(at, ref offset);
            _majorSubsystemVersion = ImageHelpers.ReadUint16(at, ref offset);
            _minorSubsystemVersion = ImageHelpers.ReadUint16(at, ref offset);
            _win32VersionValue = ImageHelpers.ReadUint32(at, ref offset);
            _sizeOfImage = ImageHelpers.ReadUint32(at, ref offset);
            _sizeOfHeaders = ImageHelpers.ReadUint32(at, ref offset);
            _checkSum = ImageHelpers.ReadUint32(at, ref offset);
            _subsystem = ImageHelpers.ReadUint16(at, ref offset);
            _dllCharacteristics = ImageHelpers.ReadUint16(at, ref offset);
            _sizeOfStackReserve = ImageHelpers.ReadUint32(at, ref offset);
            _sizeOfStackCommit = ImageHelpers.ReadUint32(at, ref offset);
            _sizeOfHeapReserve = ImageHelpers.ReadUint32(at, ref offset);
            _sizeOfHeapCommit = ImageHelpers.ReadUint32(at, ref offset);
            _loaderFlags = ImageHelpers.ReadUint32(at, ref offset);
            _numberOfRvaAndSize = ImageHelpers.ReadUint32(at, ref offset);
            return;
        }

        protected override int DataDirectoryOffset
        {
            get { return 78; }
        }

        internal ushort Magic { get; private set; }

        public override uint NumberOfRvaAndSize
        { 
            get { return _numberOfRvaAndSize; }
        }

        // OPTIONAL HEADER
        private ushort _magic;
        private byte _majorLinkerVersion;
        private byte _minorLinkerVersion;
        private uint _sizeOfCode;
        private uint _sizeOfInitializedData;
        private uint _sizeOfUninitializedData;
        private uint _addressOfEntryPoint;
        private uint _baseOfCode;
        private uint _baseOfData;
        private uint _imageBase;
        private uint _sectionAlignment;
        private uint _fileAlignment;
        private ushort _majorOperatingSystemVersion;
        private ushort _minorOperatingSystemVersion;
        private ushort _majorImageVersion;
        private ushort _minorImageVersion;
        private ushort _majorSubsystemVersion;
        private ushort _minorSubsystemVersion;
        private uint _win32VersionValue;
        private uint _sizeOfImage;
        private uint _sizeOfHeaders;
        private uint _checkSum;
        private ushort _subsystem;
        private ushort _dllCharacteristics;
        private uint _sizeOfStackReserve;
        private uint _sizeOfStackCommit;
        private uint _sizeOfHeapReserve;
        private uint _sizeOfHeapCommit;
        private uint _loaderFlags;
        private uint _numberOfRvaAndSize;
    }

    public class NTHeader64 : NTHeaderBase, INTHeader
    {
        internal NTHeader64(IntPtr at)
            : base(at)
        {
            // Forget Marshal.PtrToStructure. This won't work due to the
            // _dataDirectories field in the base class.
            int offset = 0x18;
            _magic = ImageHelpers.ReadUint16(at, ref offset);
            _majorLinkerVersion = ImageHelpers.ReadByte(at, ref offset);
            _minorLinkerVersion = ImageHelpers.ReadByte(at, ref offset);
            _sizeOfCode = ImageHelpers.ReadUint32(at, ref offset);
            _sizeOfInitializedData = ImageHelpers.ReadUint32(at, ref offset);
            _sizeOfUninitializedData = ImageHelpers.ReadUint32(at, ref offset);
            _addressOfEntryPoint = ImageHelpers.ReadUint32(at, ref offset);
            _baseOfCode = ImageHelpers.ReadUint32(at, ref offset);
            // Nota : No base of data.
            _imageBase = ImageHelpers.ReadUint64(at, ref offset);
            _sectionAlignment = ImageHelpers.ReadUint32(at, ref offset);
            _fileAlignment = ImageHelpers.ReadUint32(at, ref offset);
            _majorOperatingSystemVersion = ImageHelpers.ReadUint16(at, ref offset);
            _minorOperatingSystemVersion = ImageHelpers.ReadUint16(at, ref offset);
            _majorImageVersion = ImageHelpers.ReadUint16(at, ref offset);
            _minorImageVersion = ImageHelpers.ReadUint16(at, ref offset);
            _majorSubsystemVersion = ImageHelpers.ReadUint16(at, ref offset);
            _minorSubsystemVersion = ImageHelpers.ReadUint16(at, ref offset);
            _win32VersionValue = ImageHelpers.ReadUint32(at, ref offset);
            _sizeOfImage = ImageHelpers.ReadUint32(at, ref offset);
            _sizeOfHeaders = ImageHelpers.ReadUint32(at, ref offset);
            _checkSum = ImageHelpers.ReadUint32(at, ref offset);
            _subsystem = ImageHelpers.ReadUint16(at, ref offset);
            _dllCharacteristics = ImageHelpers.ReadUint16(at, ref offset);
            _sizeOfStackReserve = ImageHelpers.ReadUint64(at, ref offset);
            _sizeOfStackCommit = ImageHelpers.ReadUint64(at, ref offset);
            _sizeOfHeapReserve = ImageHelpers.ReadUint64(at, ref offset);
            _sizeOfHeapCommit = ImageHelpers.ReadUint64(at, ref offset);
            _loaderFlags = ImageHelpers.ReadUint32(at, ref offset);
            _numberOfRvaAndSize = ImageHelpers.ReadUint32(at, ref offset);
            return;
        }

        protected override int DataDirectoryOffset
        {
            get { return 0x88; }
        }

        public override uint NumberOfRvaAndSize
        {
            get { return _numberOfRvaAndSize; }
        }

        // OPTIONAL HEADER
        private ushort _magic;
        private byte _majorLinkerVersion;
        private byte _minorLinkerVersion;
        private uint _sizeOfCode;
        private uint _sizeOfInitializedData;
        private uint _sizeOfUninitializedData;
        private uint _addressOfEntryPoint;
        private uint _baseOfCode;
        // Nota : No base of data.
        private ulong _imageBase;
        private uint _sectionAlignment;
        private uint _fileAlignment;
        private ushort _majorOperatingSystemVersion;
        private ushort _minorOperatingSystemVersion;
        private ushort _majorImageVersion;
        private ushort _minorImageVersion;
        private ushort _majorSubsystemVersion;
        private ushort _minorSubsystemVersion;
        private uint _win32VersionValue;
        private uint _sizeOfImage;
        private uint _sizeOfHeaders;
        private uint _checkSum;
        private ushort _subsystem;
        private ushort _dllCharacteristics;
        private ulong _sizeOfStackReserve;
        private ulong _sizeOfStackCommit;
        private ulong _sizeOfHeapReserve;
        private ulong _sizeOfHeapCommit;
        private uint _loaderFlags;
        private uint _numberOfRvaAndSize;
    }
}
