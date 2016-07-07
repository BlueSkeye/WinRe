using System;

namespace SymMngr.Api
{
    public interface IDebugDirectory
    {
        DateTime Timestamp { get; }
        ushort MajorVersion { get; }
        ushort MinorVersion { get; }
        DebugInformationType Type { get; }
        uint SizeOfData { get; }
        uint AddressOfRawData { get; }
        uint PointerToRawData { get; }
    }
}
