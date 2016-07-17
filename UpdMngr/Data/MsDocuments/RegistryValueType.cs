using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/BaseTypes.xsd")]
    public enum RegistryValueType
    {
        REG_BINARY,
        REG_DWORD,
        REG_EXPAND_SZ,
        REG_MULTI_SZ,
        REG_QWORD,
        REG_SZ,
        REG_FULL_RESOURCE_DESCRIPTOR,
    }
}
