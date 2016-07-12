using System;

namespace UpdMngr
{
    public interface IReadOnlyCookie
    {
        byte[] EncryptedData { get; }
        DateTime Expiration { get; }
    }
}
