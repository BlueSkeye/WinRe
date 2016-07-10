using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.Services.Protocols;

#pragma warning disable 1591
namespace UpdMngr.WebServices.DSSAuthorization
{
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [WebServiceBindingAttribute(Name = "DssAuthWebServiceSoap",
        Namespace = "http://www.microsoft.com/SoftwareDistribution/Server/DssAuthWebService")]
    public partial class DssAuthWebService
        : SoapHttpClientProtocol
    {
        public DssAuthWebService()
        {
            this.Url = DefaultUrl;
        }

        public new string Url
        {
            get { return base.Url; }
            set { base.Url = value; }
        }

        public new bool UseDefaultCredentials
        {
            get { return base.UseDefaultCredentials; }
            set { base.UseDefaultCredentials = value; }
        }

        [SoapDocumentMethodAttribute("http://www.microsoft.com/SoftwareDistribution/Server/DssAuthWebService/GetAuthorizationCookie",
            RequestNamespace = "http://www.microsoft.com/SoftwareDistribution/Server/DssAuthWebService",
            ResponseNamespace = "http://www.microsoft.com/SoftwareDistribution/Server/DssAuthWebService",
            Use = SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public AuthorizationCookie GetAuthorizationCookie(string accountName,
            string accountGuid, Guid[] programKeys)
        {
            return this.Invoke<AuthorizationCookie>(
                "GetAuthorizationCookie", 
                accountName, accountGuid, programKeys);
        }

        [SoapDocumentMethodAttribute("http://www.microsoft.com/SoftwareDistribution/Server/IMonitorable/Ping",
            RequestNamespace = "http://www.microsoft.com/SoftwareDistribution/Server/IMonitorable",
            ResponseNamespace = "http://www.microsoft.com/SoftwareDistribution/Server/IMonitorable",
            Use = SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public MonitoredServicesResponse Ping(int pingLevel)
        {
            return this.Invoke<MonitoredServicesResponse>("Ping",
                pingLevel);
        }

        private T Invoke<T>(string methodName, params object[] args)
        {
            object[] results = this.Invoke(methodName, args);
            return (T)(results[0]);
        }

        private const string DefaultUrl = "http://update.microsoft.com/DssAuthWebService/DssAuthWebService.asmx";
    }

    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.microsoft.com/SoftwareDistribution/Server/DssAuthWebService")]
    public partial class AuthorizationCookie
    {
        public string PlugInId { get; set; }
        [XmlElementAttribute(DataType = "base64Binary")]
        public byte[] CookieData { get; set; }
    }

    [SerializableAttribute()]
    [DebuggerStepThroughAttribute()]
    [DesignerCategoryAttribute("code")]
    [XmlTypeAttribute(Namespace = "http://www.microsoft.com/SoftwareDistribution/Server/IMonitorable")]
    public partial class MonitoredServicesResponse
    {
        public string ConfigFileEnvironmentName { get; set; }
        public int ConfigFileExpirationModuloInMinutes { get; set; }
        public string CustomInfo { get; set; }
        public string DatabaseInfo { get; set; }
        public DateTime ConfigFileLastModifiedTime { get; set; }
        public DateTime ConfigFileNextExpirationTime { get; set; }
        public string ConfigFilePath { get; set; }
        public string ConfigFileProjectName { get; set; }
        public string ConfigFileVersion { get; set; }
        public bool IsHttps { get; set; }
        public string RequestContentType { get; set; }
        public string ServicesMachine { get; set; }
        public string ServicesName { get; set; }
        public DateTime ServicesTime { get; set; }
        public bool SuccessFlag { get; set; }
    }
#pragma warning restore 1591
}
