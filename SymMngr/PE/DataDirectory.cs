using System;

using SymMngr.Api;

namespace SymMngr.PE
{
    internal class DataDirectory : IDataDirectory
    {
        internal DataDirectory(DataDirectoryKind kind, IntPtr at, ref int offset)
        {
            // Forget Marshal.PtrToStructure. This won't work due to the Kind property.
            RelativeVirtualAddress = ImageHelpers.ReadUint32(at, ref offset);
            Size = ImageHelpers.ReadUint32(at, ref offset);
            Kind = kind;
            return;
        }

        public DataDirectoryKind Kind { get; private set; }

        public uint Size { get; private set; }

        public uint RelativeVirtualAddress { get; private set; }

        internal void Dump()
        {
            Console.WriteLine("{0} : {1} bytes at 0x{2:X8}",
                Kind, Size, RelativeVirtualAddress);
        }

        internal const int NativeSize = (2 * sizeof(uint));
    }
}
