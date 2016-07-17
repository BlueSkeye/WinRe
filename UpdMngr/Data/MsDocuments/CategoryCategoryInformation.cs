using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/msus/2002/12/UpdateHandlers/Category")]
    public partial class CategoryCategoryInformation
    {
        [XmlAttribute()]
        public string CategoryType { get; set; }
        [XmlAttribute()]
        public bool ProhibitsSubcategories { get; set; }
        [XmlIgnore()]
        public bool ProhibitsSubcategoriesSpecified { get; set; }
        [XmlAttribute()]
        public bool ExcludedByDefault { get; set; }
    }
}
