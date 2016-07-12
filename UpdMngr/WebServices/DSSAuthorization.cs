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
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [WebServiceBinding(Name = "DssAuthWebServiceSoap",
        Namespace = "http://www.microsoft.com/SoftwareDistribution/Server/DssAuthWebService")]
    public partial class DssAuthWebService : BaseUpdateWebService
    {
        public DssAuthWebService(string serverName, string versionPrefix, string relativeUrl)
            : base(serverName, versionPrefix, relativeUrl)
        {
            return;
        }

        public new bool UseDefaultCredentials
        {
            get { return base.UseDefaultCredentials; }
            set { base.UseDefaultCredentials = value; }
        }

        [SoapDocumentMethod("http://www.microsoft.com/SoftwareDistribution/Server/DssAuthWebService/GetAuthorizationCookie",
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

        [SoapDocumentMethod("http://www.microsoft.com/SoftwareDistribution/Server/IMonitorable/Ping",
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
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution/Server/DssAuthWebService")]
    public partial class AuthorizationCookie
    {
        public string PlugInId { get; set; }
        [XmlElement(DataType = "base64Binary")]
        public byte[] CookieData { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution/Server/IMonitorable")]
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
