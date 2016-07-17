using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/SoftwareDistributionPackage.xsd")]
    public enum UpdateClassification
    {
        Updates,
        [XmlEnum("Critical Updates")]
        CriticalUpdates,
        [XmlEnum("Security Updates")]
        SecurityUpdates,
        [XmlEnum("Feature Packs")]
        FeaturePacks,
        [XmlEnum("Update Rollups")]
        UpdateRollups,
        [XmlEnum("Service Packs")]
        ServicePacks,
        Tools,
        Hotfixes,
        Drivers,
    }
}