using System;
using System.Runtime.InteropServices;

namespace SymMngr.PE
{

    [Serializable()]
    [StructLayout(LayoutKind.Explicit)]
    internal abstract class _DataDirectory
    {
        internal uint Size
        {
            get { return _size; }
        }

        internal uint VirtualAddress
        {
            get { return _virtualAddress; }
        }

        [FieldOffset(0x00)]
        protected uint _virtualAddress;
        [FieldOffset(0x00)]
        protected uint _size;
    }

    internal class DataDirectory : _DataDirectory
    {
        internal DataDirectory(DataDirectoryKind kind)
        {
            Kind = kind;
        }

        internal DataDirectoryKind Kind { get; private set; }

        internal void Dump()
        {
            Console.WriteLine("{0} : {1} bytes at 0x{2:X8}",
                Kind, _size, _virtualAddress);
        }
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
