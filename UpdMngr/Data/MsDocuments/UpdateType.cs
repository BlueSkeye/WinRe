using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [XmlType(TypeName = "UpdateType", Namespace = "http://schemas.microsoft.com/msus/2002/12/Update")]
    public enum UpdateType1
    {
        Software,
        Driver,
        Detectoid,
        Category,
    }

    [Serializable()]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/SoftwareDistributionPackage.xsd")]
    public enum UpdateType
    {
        Software,
        Driver,
        Detectoid,
    }
}