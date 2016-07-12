using System;
using System.Net;
using UpdMngr.WebServices.DSSAuthorization;
using DSS = UpdMngr.WebServices.DSSAuthorization;
using UpdMngr.WebServices.ServerSync;
using SSWS = UpdMngr.WebServices.ServerSync;

namespace UpdMngr
{
    public class DownstreamUpdateServer : UpdateServer, IDisposable
    {
        public delegate IServerIdentity ServerIdentityProviderDelegate();

        public DownstreamUpdateServer(ServerIdentityProviderDelegate serverIdentityManager,
            string upstreamServerName = Constants.MicrosoftServerName)
        {
            if (null == serverIdentityManager) { throw new ArgumentNullException(); }
            _serverIdentityProvider = serverIdentityManager;
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
                IServerIdentity serverIdentity = _serverIdentityProvider();
                IReadOnlyAuthorizationCookie authCookie = serverIdentity.AuthorizationCookie;
                using (DssAuthWebService remoteAuthenticator =
                    new DssAuthWebService(_upstreamServerName, ServerSyncWebService.VersionPrefix, targetPlugin.ServiceUrl))
                {
                    // Retrieve a locally stored authorization cookie or acquire it
                    // from the server.
                    if (null == authCookie) {
                        DSS.AuthorizationCookie newCookie =
                            remoteAuthenticator.GetAuthorizationCookie(serverIdentity.ServerName,
                            serverIdentity.ServerId, new Guid[0]);
                        serverIdentity.RegisterAuthorizationCookieData(
                            newCookie.PlugInId, newCookie.CookieData);
                        authCookie = serverIdentity.AuthorizationCookie;
                    }
                }
                // Now we have an authorization cookie. Go on with the cookie.
                SSWS.Cookie currentCookie = null;
                if (null != serverIdentity.UpstreamServerCookie) {
                    currentCookie = new SSWS.Cookie() {
                        Expiration = serverIdentity.UpstreamServerCookie.Expiration,
                        EncryptedData = (byte[])serverIdentity.UpstreamServerCookie.EncryptedData.Clone() };
                }
                currentCookie = remote.GetCookie(
                    new SSWS.AuthorizationCookie[] {
                        new SSWS.AuthorizationCookie() {
                            PlugInId = authCookie.PlugInId,
                            CookieData = (byte[])authCookie.CookieData.Clone() } },
                        currentCookie, "1.8");

                serverIdentity.RegisterUpstreamCookieData(currentCookie.Expiration, currentCookie.EncryptedData);
                _cookie = currentCookie;
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

        private SSWS.Cookie _cookie;
        private ServerIdentityProviderDelegate _serverIdentityProvider;
        private string _upstreamServerName;
    }
}
