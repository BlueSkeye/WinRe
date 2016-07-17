using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.microsoft.com/wsus/2002/12/SoftwareDistributionPackage.xsd")]
    public partial class InstallProperties
    {
        public InstallProperties()
        {
            this.CanRequestUserInput = false;
            this.RequiresNetworkConnectivity = false;
            this.Impact = InstallationImpact.Normal;
            this.RebootBehavior = InstallationRebootBehavior.CanRequestReboot;
        }

        [XmlAttribute()]
        [DefaultValue(false)]
        public bool CanRequestUserInput { get; set; }
        [XmlAttribute()]
        [DefaultValue(false)]
        public bool RequiresNetworkConnectivity { get; set; }
        [XmlAttribute()]
        [DefaultValue(InstallationImpact.Normal)]
        public InstallationImpact Impact { get; set; }
        [XmlAttribute()]
        [DefaultValue(InstallationRebootBehavior.CanRequestReboot)]
        public InstallationRebootBehavior RebootBehavior { get; set; }
    }
}