using System;
using System.Net;
using UpdMngr.WebServices.DSSAuthorization;
using DSS = UpdMngr.WebServices.DSSAuthorization;
using UpdMngr.WebServices.ServerSync;

namespace UpdMngr
{
    public class DownstreamUpdateServer : UpdateServer, IDisposable
    {
        public DownstreamUpdateServer(IServerIdentityManager serverIdentityManager,
            string upstreamServerName = Constants.MicrosoftServerName)
        {
            if (null == serverIdentityManager) { throw new ArgumentNullException(); }
            _serverIdentityManager = serverIdentityManager;
            _upstreamServerName = upstreamServerName;
            AcquireCookie();
            return;
        }

        private void AcquireCookie()
        {
            AuthPlugInInfo targetPlugin = null;
            using (ServerSyncWebService remote = new ServerSyncWebService(_upstreamServerName)) {
                ServerAuthConfig configuration = remote.AttemptWithRetry(remote.GetAuthConfig);
                foreach (AuthPlugInInfo candidate in configuration.AuthInfo) {
                    // This is the only plugin we support at current time.
                    if ("DssTargeting" != candidate.PlugInID) { continue; }
                    targetPlugin = candidate;
                    break;
                }
                if (null == targetPlugin) {
                    throw new UpdateManagerException("No known authentication plugin.");
                }
            }
            using (DssAuthWebService remote =
                new DssAuthWebService(_upstreamServerName, ServerSyncWebService.VersionPrefix, targetPlugin.ServiceUrl))
            {
                IReadOnlyAuthorizationCookie authCookie = _serverIdentityManager.AuthorizationCookie;
                if (null == authCookie) {
                    DSS.AuthorizationCookie newCookie =
                        remote.GetAuthorizationCookie(_serverIdentityManager.ServerName,
                        _serverIdentityManager.ServerId, new Guid[0]);
                    _serverIdentityManager.RegisterAuthorizationCookieData(
                        newCookie.PlugInId, newCookie.CookieData);
                }
                int j = 1;
            }
        }

        ~DownstreamUpdateServer()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing) { GC.SuppressFinalize(this); }
        }

        private IServerIdentityManager _serverIdentityManager;
        private string _upstreamServerName;
    }
}
