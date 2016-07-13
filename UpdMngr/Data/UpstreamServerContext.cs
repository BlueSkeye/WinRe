using System;
using System.Xml.Serialization;

using UpdMngr.Api;

namespace UpdMngr.Data
{
    [XmlRoot("ussContext")]
    public class UpstreamServerContext : BasePersistenceDocument, IUpstreamServerContext
    {
        public UpstreamServerContext()
        {
        }

        internal UpstreamServerContext(XmlBasedPersistenceProvider persistenceProvider)
        {
            base.PersistanceHandler = persistenceProvider;
        }

        [XmlIgnore()]
        public IReadOnlyAuthorizationCookie AuthorizationCookie
        {
            get { return _AuthorizationCookie; }
            private set { _AuthorizationCookie = (AuthCookie)value; }
        }

        [XmlElement("authorizationCookie", Type = typeof(AuthCookie), IsNullable = true)]
        public AuthCookie _AuthorizationCookie { get; set; }

        [XmlIgnore()]
        public string ConfigAnchor
        {
            get { return _ConfigAnchor; }
            private set { _ConfigAnchor = value; }
        }

        [XmlElement("configAnchor", IsNullable = true)]
        public string _ConfigAnchor { get; set; }

        [XmlIgnore()]
        public IServerIdentity Owner
        {
            get { return _owner; }
            set
            {
                if (null == value) { throw new ArgumentNullException(); }
                if (null != _owner) {
                    throw new InvalidOperationException();
                }
                _owner = value;
            }
        }

        [XmlAttribute("serverName")]
        public string ServerName { get; set; }

        [XmlIgnore()]
        public string SyncAnchor
        {
            get { return _SyncAnchor; }
            private set { _SyncAnchor = value; }
        }

        [XmlElement("syncAnchor", IsNullable = true)]
        public string _SyncAnchor { get; set; }

        private IServerIdentity _owner;

        [XmlIgnore()]
        public IReadOnlyCookie UpstreamServerCookie
        {
            get { return _UpstreamServerCookie; }
            private set { _UpstreamServerCookie = (Cookie)value; }
        }

        [XmlElement("upstreamCookie", IsNullable = true)]
        public Cookie _UpstreamServerCookie { get; set; }

        public void RegisterAuthorizationCookieData(string pluginId, byte[] data)
        {
            AuthorizationCookie = new AuthCookie(pluginId, data);
            PersistanceHandler.Rewrite(this);
            return;
        }

        public void RegisterUpstreamCookieData(DateTime expiracy, byte[] data)
        {
            UpstreamServerCookie = new Cookie(expiracy, data);
            PersistanceHandler.Rewrite(this);
            return;
        }

        public void UpdateConfigAnchor(string newValue)
        {
            ConfigAnchor = newValue;
            PersistanceHandler.Rewrite(this);
            return;
        }

        public void UpdateSyncAnchor(string newValue)
        {
            SyncAnchor = newValue;
            PersistanceHandler.Rewrite(this);
            return;
        }

        public class AuthCookie : IReadOnlyAuthorizationCookie
        {
            public AuthCookie()
            {
            }

            internal AuthCookie(string id, byte[] data)
            {
                if (string.IsNullOrEmpty(id)) { throw new ArgumentNullException(); }
                if ((null == data) || (0 == data.Length)) { throw new ArgumentNullException(); }
                PlugInId = id;
                CookieData = data;
            }

            [XmlAttribute("data", DataType = "hexBinary")]
            public byte[] CookieData { get; set; }

            [XmlAttribute("pluginId")]
            public string PlugInId { get; set; }
        }

        public class Cookie : IReadOnlyCookie
        {
            public Cookie()
            {
            }

            internal Cookie(DateTime expiracy, byte[] data)
            {
                EncryptedData = data;
                Expiration = expiracy;
            }

            [XmlAttribute("data", DataType = "hexBinary")]
            public byte[] EncryptedData { get; set; }

            [XmlAttribute("expiration")]
            public DateTime Expiration { get; set; }
        }
    }
}
