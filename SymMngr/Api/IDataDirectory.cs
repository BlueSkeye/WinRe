
namespace SymMngr.Api
{
    public interface IDataDirectory
    {
        DataDirectoryKind Kind { get; }
        uint Size { get; }
        uint RelativeVirtualAddress { get; }
    }
}
