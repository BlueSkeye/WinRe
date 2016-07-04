using System;
using System.Runtime.InteropServices;

namespace SymMngr.PE
{
    [Serializable()]
    [StructLayout(LayoutKind.Explicit)]
    public class FileHeader
    {
        internal FileHeader()
        {
            return;
        }

        public Characteristics Characteristics
        {
            get { return _characteristics; }
        }

        public Machine Machine
        {
            get { return _machine; }
        }

        internal ushort SectionsCount
        {
            get { return _sectionsCount; }
        }

        internal DateTime Timestamp
        {
            get { return Constants.Epoch + TimeSpan.FromSeconds(_timestamp); }
        }

        internal static Machine LookupMachineKind(IntPtr fileHeaderAt)
        {
            return (Machine)Marshal.ReadInt16(fileHeaderAt, MachineFileOffset);
        }

        private const int MachineFileOffset = 0x04;
        // FILE HEADER
        [FieldOffset(MachineFileOffset)]
        private Machine _machine;
        [FieldOffset(0x06)]
        private ushort _sectionsCount;
        [FieldOffset(0x08)]
        private uint _timestamp;
        [FieldOffset(0x0C)]
        private uint _symbolTablePointer;
        [FieldOffset(0x10)]
        private uint _symbolsCount;
        [FieldOffset(0x14)]
        private uint _optionalHeaderSize;
        [FieldOffset(0x16)]
        private Characteristics _characteristics;
    }
}
