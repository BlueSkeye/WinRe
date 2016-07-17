using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/Installers/CommandLineInstallation.xsd")]
    public enum InstallationResult
    {
        Failed,
        Succeeded,
        Cancelled,
    }
}
