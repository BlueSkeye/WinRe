using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/SoftwareDistributionPackage.xsd")]
    public partial class File
    {
        [XmlAttribute(DataType = "base64Binary")]
        public byte[] Digest { get; set; }
        [XmlAttribute()]
        public string FileName { get; set; }
        [XmlAttribute()]
        public long Size { get; set; }
        [XmlAttribute()]
        public System.DateTime Modified { get; set; }
        [XmlAttribute(DataType = "anyURI")]
        public string OriginUri { get; set; }
    }
}