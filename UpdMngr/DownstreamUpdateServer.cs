using System;
using System.Collections.Generic;
using System.Xml;

using UpdMngr.Api;
using UpdMngr.WebServices;
using UpdMngr.WebServices.DSSAuthorization;
using DSS = UpdMngr.WebServices.DSSAuthorization;
using UpdMngr.WebServices.ServerSync;
using SSWS = UpdMngr.WebServices.ServerSync;

namespace UpdMngr
{
    public class DownstreamUpdateServer : UpdateServerBase, IDisposable
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
                    try {
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
                    finally {
                        // Note : anchor registered in persisted context remains valid.
                        _upstreamServerConfigurationData = null;
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
                SSWS.Cookie newCookie = null;
                try {
                    newCookie = remote.AttemptWithRetry(delegate () {
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
                }
                finally {
                    // Note : anchor registered in persisted context remains valid.
                    _upstreamServerConfigurationData = null;
                    if (registerAgain) { currentCookie = newCookie; }
                }
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
                    currentCookie = newCookie;
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

        /// <summary>Enumerate an update descriptor object for each update descriptor stored
        /// within the given context.</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<IUpdateDescriptor> EnumerateUpdateDescriptors()
        {
            foreach(IUpdateDescriptor item in _persistenceProvider.EnumerateUpdateDescriptors(GetMandatoryContext())) {
                yield return item;
            }
        }

        /// <summary>Enumerate an XML document for each update descriptor stored within
        /// the given context.</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public IEnumerable<XmlDocument> EnumerateUpdateDescriptorDocuments()
        {
            foreach(XmlDocument item in _persistenceProvider.EnumerateUpdateDescriptorDocuments(GetMandatoryContext())) {
                yield return item;
            }
        }

        private IUpstreamServerContext GetMandatoryContext()
        {
            IServerIdentity serverIdentity = _persistenceProvider.GetServerIdentity();
            IUpstreamServerContext context =
                _persistenceProvider.TryGetContext(serverIdentity, _upstreamServerName, false);
            if (null != context) { return context; }
            throw new InvalidOperationException();
        }

        public void GetRevisionsAndUpdateData(bool forConfiguration,
            bool forceNullConfigAnchor = false)
        {
            if (null == _upstreamServerConfigurationData) {
                RetrieveUpstreamConfigurationData(null, forceNullConfigAnchor);
            }
            IUpstreamServerContext context = GetMandatoryContext();
            ServerSyncFilter filter = new ServerSyncFilter() {
                GetConfig = forConfiguration,
                Get63LanguageOnly = false
            };
            if (!forceNullConfigAnchor && (null != context.SyncAnchor)) {
                filter.Anchor = context.SyncAnchor;
            }
            using (ServerSyncWebService remote = new ServerSyncWebService(_upstreamServerName)) {
                Cookie remoteCookie = new Cookie(context.UpstreamServerCookie);
                RevisionIdList revisionList = remote.GetRevisionIdList(remoteCookie, filter);
                int copyFromBaseIndex = 0;
                int revisionsCount = revisionList.NewRevisions.Length;
                while (revisionsCount > copyFromBaseIndex) {
                    int identitiesCount = Math.Min(revisionsCount - copyFromBaseIndex,
                        _upstreamServerConfigurationData.MaxNumberOfUpdatesPerRequest);
                    UpdateIdentity[] updateIdentities = new UpdateIdentity[identitiesCount];
                    Array.Copy(revisionList.NewRevisions, copyFromBaseIndex, updateIdentities, 0,
                        identitiesCount);
                    ServerUpdateData updateData =
                        remote.GetUpdateData(remoteCookie, updateIdentities);
                    Dictionary<byte[], string> fileUrlByHash =
                        new Dictionary<byte[], string>(HashComparer.Singleton);
                    foreach(ServerSyncUrlData urlData in updateData.fileUrls) {
                        fileUrlByHash.Add(urlData.FileDigest, urlData.MUUrl);
                    }
                    foreach(ServerSyncUpdateData update in updateData.updates) {
                        object container = _persistenceProvider.EnsureUpdateContainer(context,
                            update.Id.UpdateID, update.Id.RevisionNumber);
                        IUpdateDescriptor descriptor =
                            _persistenceProvider.CreateUpdateDescriptor(container,
                                update.Id.UpdateID, update.Id.RevisionNumber);
                        // TODO : Must perform decompression if needed instead of having the
                        // byte array representation for a compressed BLOB.
                        descriptor.UpdateBlob = update.XmlUpdateBlob ??
                            update.XmlUpdateBlobCompressed.HexadecimalString();
                        if (null != update.FileDigestList) {
                            foreach(byte[] searchedHash in update.FileDigestList) {
                                string updateUrl;
                                if (!fileUrlByHash.TryGetValue(searchedHash, out updateUrl)) {
                                    // TODO : Should relax this. We set it here for debugging purpose.
                                    throw new ApplicationException();
                                }
                                descriptor.AddFile(searchedHash, updateUrl);
                            }
                        }
                        _persistenceProvider.Save(descriptor);
                    }
                    copyFromBaseIndex += identitiesCount;
                }
                context.UpdateSyncAnchor(revisionList.Anchor);
            }
        }

        internal void RetrieveUpstreamConfigurationData(string lastConfigAnchor,
            bool forceNullConfigAnchor = false)
        {
            if (null == _cookie) {
                throw new InvalidOperationException();
            }
            IUpstreamServerContext context = GetMandatoryContext();
            lastConfigAnchor = (forceNullConfigAnchor)
                ? null
                : lastConfigAnchor ?? context.ConfigAnchor;
            using (ServerSyncWebService remote = new ServerSyncWebService(_upstreamServerName)) {
                // TODO : Should attempt with retry in case cookie has expired.
                _upstreamServerConfigurationData = remote.GetConfigData(_cookie, lastConfigAnchor);
                context.UpdateConfigAnchor(_upstreamServerConfigurationData.NewConfigAnchor);
            }
        }

        private SSWS.Cookie _cookie;
        private IPersistenceProvider _persistenceProvider;
        private ServerSyncConfigData _upstreamServerConfigurationData;
        private string _upstreamServerName;

        /// <summary>WARNING : This comparer is for use only on a result of a
        /// cryptographically secure hash function.</summary>
        private class HashComparer : IEqualityComparer<byte[]>
        {
            private HashComparer()
            {
            }

            public bool Equals(byte[] x, byte[] y)
            {
                return Helpers.AreEquals(x, y);
            }

            public int GetHashCode(byte[] obj)
            {
                if ((null == obj) || (0 == obj.Length)) { return 0; }
                if (4 > obj.Length) { return obj[0]; }
                // Due to expected even distribution of the hash function we can use
                // this simple calculation.
                return (((((obj[0] << 8) + obj[1]) << 8) + obj[2]) << 8) + obj[3];
            }

            internal static readonly HashComparer Singleton = new HashComparer();
        }
    }
}
