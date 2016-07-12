using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using UpdMngr;

namespace UpdMngrTest
{
    [TestClass]
    public class DSSToMS
    {
        [TestMethod]
        public void RetrieveAndStoreCookie()
        {
            using (DownstreamUpdateServer dss = new DownstreamUpdateServer(GetServerIdentity)) {
            }
        }

        private static FileInfo ServerIdFile
        {
            get
            {
                return new FileInfo(
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "updsrvid.txt"));
            }
        }

        private IServerIdentity GetServerIdentity()
        {
            try {
                FileStream input = null;
                try {
                    try { input = File.Open(ServerIdFile.FullName, FileMode.Open, FileAccess.Read); }
                    catch { }
                    XmlSerializer serializer = new XmlSerializer(typeof(ServerIdentity));
                    return (ServerIdentity)serializer.Deserialize(input);
                }
                finally { if (null != input) { input.Close(); } }
            }
            catch {
                ServerIdentity result = new ServerIdentity();
                result.ServerName = "garamond.contoso.com";
                result.ServerId = Guid.NewGuid().ToString();
                RewriteIdentity(result);
                return result;
            }
        }

        private static void RewriteIdentity(ServerIdentity newIdentity)
        {
            FileStream output = null;
            try {
                try { output = File.Open(ServerIdFile.FullName, FileMode.Create, FileAccess.Write); }
                catch { }
                XmlSerializer serializer = new XmlSerializer(typeof(ServerIdentity));
                using (StreamWriter writer = new StreamWriter(output)) {
                    serializer.Serialize(writer, newIdentity);
                }
            }
            finally { if (null != output) { output.Close(); } }
        }

        [XmlRoot("srvid")]
        public class ServerIdentity : IServerIdentity
        {
            [XmlIgnore()]
            public IReadOnlyAuthorizationCookie AuthorizationCookie
            {
                get { return _AuthorizationCookie; }
                set { _AuthorizationCookie = (AuthCookie)value; }
            }

            [XmlElement("authorizationCookie", Type = typeof(AuthCookie), IsNullable = true)]
            public AuthCookie _AuthorizationCookie { get; set; }

            [XmlAttribute("id")]
            public string ServerId { get; set; }

            [XmlAttribute("name")]
            public string ServerName { get; set; }

            [XmlIgnore()]
            public IReadOnlyCookie UpstreamServerCookie
            {
                get { return _UpstreamServerCookie; }
                set { _UpstreamServerCookie = (Cookie)value; }
            }

            [XmlElement("upstreamCookie", IsNullable = true)]
            public Cookie _UpstreamServerCookie { get; set; }

            public void RegisterAuthorizationCookieData(string pluginId, byte[] data)
            {
                AuthorizationCookie = new AuthCookie(pluginId, data);
                RewriteIdentity(this);
                return;
            }

            public void RegisterUpstreamCookieData(DateTime expiracy, byte[] data)
            {
                UpstreamServerCookie = new Cookie(expiracy, data);
                RewriteIdentity(this);
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
}
