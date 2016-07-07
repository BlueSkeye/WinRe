using System;

using SymMngr.Api;

namespace SymMngr.PE
{
    internal class DebugDirectory : IDebugDirectory
    {
        internal DebugDirectory(IntPtr at, ref int offset)
        {
            offset += sizeof(uint);
            Timestamp = Constants.Epoch + TimeSpan.FromSeconds(ImageHelpers.ReadUint32(at, ref offset));
            MajorVersion = ImageHelpers.ReadUint16(at, ref offset);
            MinorVersion = ImageHelpers.ReadUint16(at, ref offset);
            Type = (DebugInformationType)ImageHelpers.ReadUint32(at, ref offset);
            SizeOfData = ImageHelpers.ReadUint32(at, ref offset);
            AddressOfRawData = ImageHelpers.ReadUint32(at, ref offset);
            PointerToRawData = ImageHelpers.ReadUint32(at, ref offset);
        }

        public DateTime Timestamp { get; private set; }
        public ushort MajorVersion { get; private set; }
        public ushort MinorVersion { get; private set; }
        public DebugInformationType Type { get; private set; }
        public uint SizeOfData { get; private set; }
        public uint AddressOfRawData { get; private set; }
        public uint PointerToRawData { get; private set; }
    }
}
