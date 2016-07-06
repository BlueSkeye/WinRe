using System;
using System.Runtime.InteropServices;

namespace SymMngr.PE
{
    public class FileHeader
    {
        internal FileHeader(IntPtr at)
        {
            // Skip magic signature.
            int offset = MachineFileOffset;
            Machine = (Machine)ImageHelpers.ReadUint16(at, ref offset);
            SectionsCount = ImageHelpers.ReadUint16(at, ref offset);
            Timestamp = Constants.Epoch + TimeSpan.FromSeconds(ImageHelpers.ReadUint32(at, ref offset));
            _symbolTablePointer = ImageHelpers.ReadUint32(at, ref offset);
            _symbolsCount = ImageHelpers.ReadUint32(at, ref offset);
            _optionalHeaderSize = ImageHelpers.ReadUint32(at, ref offset);
            Characteristics = (Characteristics)ImageHelpers.ReadUint16(at, ref offset);
            return;
        }

        public Characteristics Characteristics { get; private set; }

        public Machine Machine { get; private set; }

        internal ushort SectionsCount { get; private set; }

        internal DateTime Timestamp { get; private set; }

        internal static Machine LookupMachineKind(IntPtr fileHeaderAt)
        {
            return (Machine)Marshal.ReadInt16(fileHeaderAt, MachineFileOffset);
        }

        private const int MachineFileOffset = 0x04;
        private uint _symbolTablePointer;
        private uint _symbolsCount;
        private uint _optionalHeaderSize;
    }
}
