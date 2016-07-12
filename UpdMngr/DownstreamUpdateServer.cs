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
            _persistenceProvider = persistenceProvider;
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
                // TODO : Consider caching this item. This must be done consistently
                // relatively to rewriting its content to persistence storage.
                IServerIdentity serverIdentity = _persistenceProvider.GetServerIdentity();
                IUpstreamServerContext context =
                    _persistenceProvider.TryGetContext(serverIdentity, _upstreamServerName, true);
                // context is guaranteed to exist, albeit can be empty.
                IReadOnlyAuthorizationCookie authCookie = context.AuthorizationCookie;
                // Retrieve a locally stored authorization cookie or acquire it
                // from the server.
                if (null == authCookie) {
                    using (DssAuthWebService remoteAuthenticator =
                        new DssAuthWebService(_upstreamServerName, ServerSyncWebService.VersionPrefix, targetPlugin.ServiceUrl))
                    {
                        DSS.AuthorizationCookie newAuthorizationCookie =
                            remoteAuthenticator.GetAuthorizationCookie(serverIdentity.ServerName,
                            serverIdentity.ServerId, new Guid[0]);
                        context.RegisterAuthorizationCookieData(
                            newAuthorizationCookie.PlugInId, newAuthorizationCookie.CookieData);
                        authCookie = context.AuthorizationCookie;
                    }
                }
                // Now we have an authorization cookie. Go on with the cookie.
                SSWS.Cookie currentCookie = null;
                if (null != context.UpstreamServerCookie) {
                    currentCookie = new SSWS.Cookie() {
                        Expiration = context.UpstreamServerCookie.Expiration,
                        EncryptedData = (byte[])context.UpstreamServerCookie.EncryptedData.Clone() };
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
                    context.RegisterUpstreamCookieData(currentCookie.Expiration, currentCookie.EncryptedData);
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
            IServerIdentity serverIdentity = _persistenceProvider.GetServerIdentity();
            IUpstreamServerContext context =
                _persistenceProvider.TryGetContext(serverIdentity, _upstreamServerName, false);
            if (null == context) {
                throw new InvalidOperationException();
            }
            if (null == lastConfigAnchor) {
                lastConfigAnchor = context.ConfigAnchor;
            }
            using (ServerSyncWebService remote = new ServerSyncWebService(_upstreamServerName)) {
                // TODO : Should attempt with retry in case cookie has expired.
                ServerSyncConfigData configData = remote.GetConfigData(_cookie, lastConfigAnchor);
                context.UpdateAnchor(configData.NewConfigAnchor);
                int i = 1;
            }
        }

        private SSWS.Cookie _cookie;
        private IPersistenceProvider _persistenceProvider;
        private string _upstreamServerName;
    }
}
