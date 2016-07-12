using System;

namespace UpdMngr.Api
{
    public interface IReadOnlyCookie
    {
        byte[] EncryptedData { get; }
        DateTime Expiration { get; }
    }
}
