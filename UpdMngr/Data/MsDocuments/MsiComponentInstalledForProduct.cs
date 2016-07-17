using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(AnonymousType = true, Namespace = "http://schemas.microsoft.com/wsus/2002/12/MsiApplicabilityRules.xsd")]
    [XmlRoot(Namespace = "http://schemas.microsoft.com/wsus/2002/12/MsiApplicabilityRules.xsd", IsNullable = false)]
    public partial class MsiComponentInstalledForProduct
    {
        public MsiComponentInstalledForProduct()
        {
            this.AllComponentsRequired = false;
            this.AllProductsRequired = false;
        }

        [XmlElement("Component")]
        public string[] Component { get; set; }
        [XmlElement("Product")]
        public string[] Product { get; set; }
        [XmlAttribute()]
        [DefaultValue(false)]
        public bool AllComponentsRequired { get; set; }
        [XmlAttribute()]
        [DefaultValue(false)]
        public bool AllProductsRequired { get; set; }
    }
}
