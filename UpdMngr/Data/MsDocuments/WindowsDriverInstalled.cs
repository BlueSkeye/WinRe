using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/wsus/2002/12/Installers/WindowsDriver.xsd")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/wsus/2002/12/Installers/WindowsDriver.xsd", IsNullable = false)]
    public partial class WindowsDriverInstalled
    {
    }
}
