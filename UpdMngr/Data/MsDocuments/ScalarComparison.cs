using System;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/BaseTypes.xsd")]
    public enum ScalarComparison
    {
        LessThan,
        LessThanOrEqualTo,
        EqualTo,
        GreaterThanOrEqualTo,
        GreaterThan,
    }
}