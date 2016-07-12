using System;
using UpdMngr.WebServices;
using UpdMngr.WebServices.DSSAuthorization;
using DSS = UpdMngr.WebServices.DSSAuthorization;
using UpdMngr.WebServices.ServerSync;
using SSWS = UpdMngr.WebServices.ServerSync;

using UpdMngr.Api;

namespace UpdMngr
{
    public class DownstreamUpdateServer : UpdateServer, IDisposable
    {
        public DownstreamUpdateServer(IPersistenceProvider persistenceProvider,
            string upstreamServerName = Constants.MicrosoftServerName)
        {
            if (null == persistenceProvider) { throw new ArgumentNullException(); }
            _serverIdentityProvider = persistenceProvider;
            _upstreamServerName = upstreamServerName;
            AcquireCookie();
            return;
        }

        private void AcquireCookie()
        {
            AuthPlugInInfo targetPlugin = null;
            using (ServerSyncWebService remote = new ServerSyncWebService(_upstreamServerName)) {
                ServerAuthConfig configuration =
                    remote.AttemptWithRetry<ServerAuthConfig,SSWS.ErrorCode>(remote.GetAuthConfig);
                foreach (AuthPlugInInfo candidate in configuration.AuthInfo) {
                    // This is the only plugin we support at current time.
                    if ("DssTargeting" != candidate.PlugInID) { continue; }
                    targetPlugin = candidate;
                    break;
                }
                if (null == targetPlugin) {
                    throw new UpdateManagerException("No known authentication plugin.");
                }
                IServerIdentity serverIdentity = _serverIdentityProvider.GetServerIdentity();
                IReadOnlyAuthorizationCookie authCookie = serverIdentity.AuthorizationCookie;
                using (DssAuthWebService remoteAuthenticator =
                    new DssAuthWebService(_upstreamServerName, ServerSyncWebService.VersionPrefix, targetPlugin.ServiceUrl))
                {
                    // Retrieve a locally stored authorization cookie or acquire it
                    // from the server.
                    if (null == authCookie) {
                        DSS.AuthorizationCookie newAuthorizationCookie =
                            remoteAuthenticator.GetAuthorizationCookie(serverIdentity.ServerName,
                            serverIdentity.ServerId, new Guid[0]);
                        serverIdentity.RegisterAuthorizationCookieData(
                            newAuthorizationCookie.PlugInId, newAuthorizationCookie.CookieData);
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
                bool registerAgain = (null == currentCookie);
                SSWS.Cookie newCookie = remote.AttemptWithRetry(delegate () {
                    return remote.GetCookie(
                        new SSWS.AuthorizationCookie[] {
                        new SSWS.AuthorizationCookie() {
                            PlugInId = authCookie.PlugInId,
                            CookieData = (byte[])authCookie.CookieData.Clone() } },
                            currentCookie, "1.8");
                    },
                    SSWS.ErrorCode.InternalServerError,
                    // Be prepared here for clean cookie expiration handling.
                    delegate (FaultDetails<SSWS.ErrorCode> details) {
                        switch (details.ErrorCode) {
                            case ErrorCode.InvalidCookie:
                                break;
                            case ErrorCode.ServerChanged:
                                if (null != currentCookie) { break; }
                                return false;
                            default:
                                return false;
                        }
                        currentCookie = null;
                        registerAgain = true;
                        return true;
                    });
                if ((null != currentCookie) && (null != newCookie) && !registerAgain) {
                    byte[] currentData = currentCookie.EncryptedData;
                    int dataLength = currentData.Length;
                    byte[] newData = newCookie.EncryptedData;
                    if (dataLength != newData.Length) {
                        registerAgain = true;
                    }
                    else {
                        for (int index = 0; index < dataLength; index++) {
                            if (newData[index] != currentData[index]) {
                                registerAgain = true;
                                break;
                            }
                        }
                    }
                }
                if (registerAgain) {
                    serverIdentity.RegisterUpstreamCookieData(currentCookie.Expiration, currentCookie.EncryptedData);
                }
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

        internal void RetrieveUpstreamConfigurationData(string lastConfigAnchor)
        {
            if (null == _cookie) {
                throw new InvalidOperationException();
            }
            using (ServerSyncWebService remote = new ServerSyncWebService(_upstreamServerName)) {
                // TODO : Should attempt with retry in case cookie has expired.
                ServerSyncConfigData configData = remote.GetConfigData(_cookie, lastConfigAnchor);

                int i = 1;
            }
        }

        private SSWS.Cookie _cookie;
        private IPersistenceProvider _serverIdentityProvider;
        private string _upstreamServerName;
    }
}
