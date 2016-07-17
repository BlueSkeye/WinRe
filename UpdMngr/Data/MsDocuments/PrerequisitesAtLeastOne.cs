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
    public partial class PrerequisitesAtLeastOne
    {
        public PrerequisitesAtLeastOne()
        {
            this.IsCategory = false;
        }

        [XmlElement("UpdateIdentity")]
        public UpdateIdentityForPrerequisite[] UpdateIdentity { get; set; }
        [XmlAttribute()]
        [DefaultValue(false)]
        public bool IsCategory { get; set; }
    }
}
