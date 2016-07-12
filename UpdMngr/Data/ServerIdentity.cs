using System;
using System.Xml.Serialization;

using UpdMngr.Api;

namespace UpdMngr.Data
{
    [XmlRoot("srvid")]
    public class ServerIdentity : BasePersistenceDocument, IServerIdentity
    {
        [XmlAttribute("id")]
        public string ServerId { get; set; }

        [XmlAttribute("name")]
        public string ServerName { get; set; }
    }
}
