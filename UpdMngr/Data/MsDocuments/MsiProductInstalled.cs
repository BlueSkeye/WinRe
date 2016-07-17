using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/wsus/2002/12/MsiApplicabilityRules.xsd")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/wsus/2002/12/MsiApplicabilityRules.xsd", IsNullable = false)]
    public partial class MsiProductInstalled
    {
        public MsiProductInstalled()
        {
            this.ExcludeVersionMax = false;
            this.ExcludeVersionMin = false;
        }

        [XmlAttribute()]
        public string ProductCode { get; set; }
        [XmlAttribute()]
        public string VersionMax { get; set; }
        [XmlAttribute()]
        [DefaultValue(false)]
        public bool ExcludeVersionMax { get; set; }
        [XmlAttribute()]
        public string VersionMin { get; set; }
        [XmlAttribute()]
        [DefaultValue(false)]
        public bool ExcludeVersionMin { get; set; }
        [XmlAttribute()]
        public int Language { get; set; }
        [XmlIgnore()]
        public bool LanguageSpecified { get; set; }
    }
}
