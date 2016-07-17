using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.microsoft.com/msus/2002/12/UpdateHandlers/Category")]
    public partial class Category
    {
        public CategoryCategoryInformation CategoryInformation { get; set; }
    }
}
