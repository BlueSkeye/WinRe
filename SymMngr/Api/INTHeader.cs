
namespace SymMngr.Api
{
    public interface INTHeader
    {
        IDataDirectory this[DataDirectoryKind kind] { get; }
        Machine Machine { get; }
        uint NumberOfRvaAndSize { get; }
    }
}
