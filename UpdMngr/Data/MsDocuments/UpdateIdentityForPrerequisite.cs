﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace UpdMngr.Data.MsDocuments
{
    [Serializable()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlType(Namespace = "http://schemas.microsoft.com/msus/2002/12/Update")]
    public partial class UpdateIdentityForPrerequisite
    {
        /// <remarks/>
        [XmlAttribute()]
        public string UpdateID { get; set; }
    }
}