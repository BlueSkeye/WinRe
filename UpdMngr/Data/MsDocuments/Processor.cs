using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/wsus/2002/12/BaseApplicabilityRules.xsd")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/wsus/2002/12/BaseApplicabilityRules.xsd", IsNullable = false)]
    public partial class Processor
    {
        [XmlAttribute()]
        public ushort Architecture { get; set; }
        [XmlAttribute()]
        public ushort Level { get; set; }
        [XmlIgnore()]
        public bool LevelSpecified { get; set; }
        [XmlAttribute()]
        public ushort Revision { get; set; }
        [XmlIgnore()]
        public bool RevisionSpecified { get; set; }
    }
}
