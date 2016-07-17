using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/BaseApplicabilityRules.xsd")]
    public partial class FileExistsType
    {
        [XmlAttribute()]
        public string Path { get; set; }
        [XmlAttribute()]
        public string Version { get; set; }
        [XmlAttribute()]
        public System.DateTime Created { get; set; }
        [XmlIgnore()]
        public bool CreatedSpecified { get; set; }
        [XmlAttribute()]
        public System.DateTime Modified { get; set; }
        [XmlIgnore()]
        public bool ModifiedSpecified { get; set; }
        [XmlAttribute()]
        public long Size { get; set; }
        [XmlIgnore()]
        public bool SizeSpecified { get; set; }
        [XmlAttribute()]
        public ushort Language { get; set; }
        [XmlIgnore()]
        public bool LanguageSpecified { get; set; }
    }
}
