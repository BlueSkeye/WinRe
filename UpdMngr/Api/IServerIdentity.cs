using System;

namespace UpdMngr.Api
{
    public interface IServerIdentity
    {
        string ServerId { get; }
        string ServerName { get; }
    }
}
