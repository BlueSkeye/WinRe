
namespace SymMngr.Api
{
    public enum DataDirectoryKind
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
