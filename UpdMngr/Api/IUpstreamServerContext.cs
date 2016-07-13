using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdMngr.Api
{
    public interface IUpstreamServerContext
    {
        IReadOnlyAuthorizationCookie AuthorizationCookie { get; }
        string ConfigAnchor { get; }
        IServerIdentity Owner { get; }
        string ServerName { get; }
        string SyncAnchor { get; }
        IReadOnlyCookie UpstreamServerCookie { get; }

        void RegisterAuthorizationCookieData(string pluginId, byte[] data);
        void RegisterUpstreamCookieData(DateTime expiracy, byte[] data);
        void UpdateConfigAnchor(string newValue);
        void UpdateSyncAnchor(string newValue);
    }
}
