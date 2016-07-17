using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/msus/2002/12/Update")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/msus/2002/12/Update", IsNullable = true)]
    public partial class IsInstallable
    {
        public object ApplicabilityRuleElement { get; set; }
    }
}
