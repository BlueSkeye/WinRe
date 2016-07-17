using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [XmlType(AnonymousType = true, Namespace = "http://www.microsoft.com/msi/patch_applicability.xsd")]
    public enum ValidateVersionComparisonFilter
    {
        Major,
        MajorMinor,
        MajorMinorUpdate,
        None,
    }
}