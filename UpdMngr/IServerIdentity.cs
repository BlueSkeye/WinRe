using System;

namespace UpdMngr
{
    public interface IServerIdentity
    {
        IReadOnlyAuthorizationCookie AuthorizationCookie { get; }
        IReadOnlyCookie UpstreamServerCookie { get; }
        string ServerId { get; }
        string ServerName { get; }

        void RegisterAuthorizationCookieData(string pluginId, byte[] data);
        void RegisterUpstreamCookieData(DateTime expiracy, byte[] data);
    }
}
