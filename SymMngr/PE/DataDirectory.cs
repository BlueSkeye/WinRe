using System;

namespace SymMngr.PE
{
    internal class DataDirectory
    {
        internal DataDirectory(DataDirectoryKind kind, IntPtr at, ref int offset)
        {
            // Forget Marshal.PtrToStructure. This won't work due to the Kind property.
            VirtualAddress = ImageHelpers.ReadUint32(at, ref offset);
            Size = ImageHelpers.ReadUint32(at, ref offset);
            Kind = kind;
            return;
        }

        internal DataDirectoryKind Kind { get; private set; }

        internal uint Size { get; private set; }

        internal uint VirtualAddress { get; private set; }

        internal void Dump()
        {
            Console.WriteLine("{0} : {1} bytes at 0x{2:X8}",
                Kind, Size, VirtualAddress);
        }

        internal const int NativeSize = (2 * sizeof(uint));
    }

    internal enum DataDirectoryKind
    {
        ExportTable = 0,
        ImportTable,
        ResourceTable,
        ExceptionTable,
        CertificateTable,
        RelocationTable,
        DebuggingInformation,
        ArchitectureSpecificData,
        GlobalPointerRegisterRVA,
        ThreadLocalStorage,
        LoadConfigurationTable,
        BoundImportTable,
        ImportAddressTable,
        DelayImportDescriptor,
        ClrHeader,
        Reserved
    }
}
