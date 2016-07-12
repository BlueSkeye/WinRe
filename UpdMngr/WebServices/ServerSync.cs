using System;
using System.Web.Services;
using System.Diagnostics;
using System.Web.Services.Description;
using System.Web.Services.Protocols;
using System.Xml.Serialization;
using System.ComponentModel;

namespace UpdMngr.WebServices.ServerSync
{
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [WebServiceBinding(Name = "ServerSyncProxySoap",
        Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class ServerSyncWebService : BaseUpdateWebService
    {
        public ServerSyncWebService(string serverName = Constants.MicrosoftServerName)
            : base(serverName, VersionPrefix, "ServerSyncWebService/ServerSyncWebService.asmx")
        {
            return;
        }

        public new bool UseDefaultCredentials
        {
            get { return base.UseDefaultCredentials; }
            set { base.UseDefaultCredentials = value; }
        }

        public static string VersionPrefix
        {
            get { return "v6"; }
        }

        [SoapDocumentMethod("http://www.microsoft.com/SoftwareDistribution/GetAuthConfig",
            RequestNamespace = "http://www.microsoft.com/SoftwareDistribution",
            ResponseNamespace = "http://www.microsoft.com/SoftwareDistribution",
            Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public ServerAuthConfig GetAuthConfig()
        {
            return Invoke<ServerAuthConfig>("GetAuthConfig");
        }

        [SoapDocumentMethod("http://www.microsoft.com/SoftwareDistribution/GetCookie",
            RequestNamespace = "http://www.microsoft.com/SoftwareDistribution",
            ResponseNamespace = "http://www.microsoft.com/SoftwareDistribution",
            Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public Cookie GetCookie(AuthorizationCookie[] authCookies, Cookie oldCookie, string protocolVersion)
        {
            return this.Invoke<Cookie>(
                "GetCookie", 
                authCookies, oldCookie, protocolVersion);
        }

        [SoapDocumentMethod("http://www.microsoft.com/SoftwareDistribution/GetConfigData",
            RequestNamespace = "http://www.microsoft.com/SoftwareDistribution",
            ResponseNamespace = "http://www.microsoft.com/SoftwareDistribution",
            Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public ServerSyncConfigData GetConfigData(Cookie cookie, string configAnchor)
        {
            return this.Invoke<ServerSyncConfigData>(
                "GetConfigData",
                cookie, configAnchor);
        }

        [SoapDocumentMethod("http://www.microsoft.com/SoftwareDistribution/GetRevisionIdList",
            RequestNamespace = "http://www.microsoft.com/SoftwareDistribution",
            ResponseNamespace = "http://www.microsoft.com/SoftwareDistribution",
            Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public RevisionIdList GetRevisionIdList(Cookie cookie, ServerSyncFilter filter)
        {
            return this.Invoke<RevisionIdList>(
                "GetRevisionIdList",
                cookie, filter);
        }

        [SoapDocumentMethod("http://www.microsoft.com/SoftwareDistribution/GetUpdateData",
            RequestNamespace = "http://www.microsoft.com/SoftwareDistribution",
            ResponseNamespace = "http://www.microsoft.com/SoftwareDistribution",
            Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public ServerUpdateData GetUpdateData(Cookie cookie, UpdateIdentity[] updateIds)
        {
            return this.Invoke<ServerUpdateData>(
                "GetUpdateData",
                cookie, updateIds);
        }

        [SoapDocumentMethod("http://www.microsoft.com/SoftwareDistribution/DownloadFiles",
            RequestNamespace = "http://www.microsoft.com/SoftwareDistribution",
            ResponseNamespace = "http://www.microsoft.com/SoftwareDistribution",
            Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public void DownloadFiles(Cookie cookie, byte[][] fileDigestList)
        {
            this.Invoke("DownloadFiles",
                new object[] { cookie, fileDigestList});
        }

        [SoapDocumentMethod("http://www.microsoft.com/SoftwareDistribution/GetDeployments",
            RequestNamespace = "http://www.microsoft.com/SoftwareDistribution",
            ResponseNamespace = "http://www.microsoft.com/SoftwareDistribution",
            Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public ServerSyncDeploymentResult GetDeployments(Cookie cookie, string deploymentAnchor, string syncAnchor)
        {
            return this.Invoke<ServerSyncDeploymentResult>(
                "GetDeployments", 
                cookie, deploymentAnchor, syncAnchor);
        }

        [SoapDocumentMethod("http://www.microsoft.com/SoftwareDistribution/GetRelatedRevisionsForUpdates",
            RequestNamespace = "http://www.microsoft.com/SoftwareDistribution",
            ResponseNamespace = "http://www.microsoft.com/SoftwareDistribution",
            Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public UpdateIdentity[] GetRelatedRevisionsForUpdates(Cookie cookie, Guid[] updateIDs)
        {
            return this.Invoke<UpdateIdentity[]>(
                "GetRelatedRevisionsForUpdates",
                cookie, updateIDs);
        }

        [SoapDocumentMethod("http://www.microsoft.com/SoftwareDistribution/Server/IMonitorable/Ping",
            RequestNamespace = "http://www.microsoft.com/SoftwareDistribution/Server/IMonitorable",
            ResponseNamespace = "http://www.microsoft.com/SoftwareDistribution/Server/IMonitorable",
            Use = SoapBindingUse.Literal, ParameterStyle = SoapParameterStyle.Wrapped)]
        public MonitoredServicesResponse Ping(int pingLevel)
        {
            return this.Invoke<MonitoredServicesResponse>(
                "Ping",
                pingLevel);
        }

        private T Invoke<T>(string methodName, params object[] args)
        {
            object[] results = this.Invoke(methodName, args);
            return (T)(results[0]);
        }

        private const string DefaultUrl = "http://update.microsoft.com/serversyncwebservice/ServerSyncProxy.asmx";
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class ServerAuthConfig
    {
        public DateTime LastChange { get; set; }
        public AuthPlugInInfo[] AuthInfo { get; set; }
        public int[] AllowedEventIds { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class AuthPlugInInfo
    {
        public string PlugInID { get; set; }
        public string ServiceUrl { get; set; }
        public string Parameter { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution/Server/IMonitorable")]
    public partial class MonitoredServicesResponse
    {
        public bool SuccessFlag { get; set; }
        public DateTime ServicesTime { get; set; }
        public string ServicesName { get; set; }
        public string ServicesMachine { get; set; }
        public bool IsHttps { get; set; }
        public string RequestContentType { get; set; }
        public string ConfigFilePath { get; set; }
        public string ConfigFileProjectName { get; set; }
        public string ConfigFileEnvironmentName { get; set; }
        public DateTime ConfigFileLastModifiedTime { get; set; }
        public string ConfigFileVersion { get; set; }
        public DateTime ConfigFileNextExpirationTime { get; set; }
        public int ConfigFileExpirationModuloInMinutes { get; set; }
        public string DatabaseInfo { get; set; }
        public string CustomInfo { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class ServerSyncDeployment
    {
        public Guid UpdateId { get; set; }
        public int RevisionNumber { get; set; }
        public int Action { get; set; }
        public string AdminName { get; set; }
        public DateTime Deadline { get; set; }
        public bool IsAssigned { get; set; }
        public DateTime GoLiveTime { get; set; }
        public Guid DeploymentGuid { get; set; }
        public Guid TargetGroupId { get; set; }
        public byte DownloadPriority { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class ServerSyncTargetGroup
    {
        public Guid TargetGroupID { get; set; }
        public Guid ParentGroupId { get; set; }
        public string Name { get; set; }
        public bool IsBuiltin { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class ServerSyncDeploymentResult
    {
        public string Anchor { get; set; }
        public ServerSyncTargetGroup[] Groups { get; set; }
        public ServerSyncDeployment[] Deployments { get; set; }
        public Guid[] DeadDeployments { get; set; }
        public Guid[] HiddenUpdates { get; set; }
        public Guid[] AcceptedEulas { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class ServerSyncUrlData
    {
        [XmlElement(DataType = "base64Binary")]
        public byte[] FileDigest { get; set; }
        public string MUUrl { get; set; }
        public string UssUrl { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class ServerSyncUpdateData
    {
        public UpdateIdentity Id { get; set; }
        public string XmlUpdateBlob { get; set; }
        public byte[][] FileDigestList { get; set; }
        [XmlElement(DataType = "base64Binary")]
        public byte[] XmlUpdateBlobCompressed { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class UpdateIdentity
    {
        public System.Guid UpdateID { get; set; }
        public int RevisionNumber { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class ServerUpdateData
    {
        public ServerSyncUpdateData[] updates { get; set; }
        public ServerSyncUrlData[] fileUrls { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class RevisionIdList
    {
        public string Anchor { get; set; }
        public UpdateIdentity[] NewRevisions { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class LanguageAndDelta
    {
        public int Id { get; set; }
        public bool Delta { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class IdAndDelta
    {
        public System.Guid Id { get; set; }
        public bool Delta { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class Version
    {
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class ServerSyncFilter
    {
        public Version DssProtocolVersion { get; set; }
        public string Anchor { get; set; }
        public bool GetConfig { get; set; }
        public bool Get63LanguageOnly { get; set; }
        public IdAndDelta[] Categories { get; set; }
        public IdAndDelta[] Classifications { get; set; }
        public LanguageAndDelta[] Languages { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class ServerSyncLanguageData
    {
        public int LanguageID { get; set; }
        public string ShortLanguage { get; set; }
        public string LongLanguage { get; set; }
        public bool Enabled { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class ServerSyncConfigData
    {
        public bool CatalogOnlySync { get; set; }
        public bool LazySync { get; set; }
        public bool ServerHostsPsfFiles { get; set; }
        public int MaxNumberOfUpdatesPerRequest { get; set; }
        public string NewConfigAnchor { get; set; }
        public string ProtocolVersion { get; set; }
        public ServerSyncLanguageData[] LanguageUpdateList { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class Cookie
    {
        public DateTime Expiration { get; set; }
        [XmlElement(DataType = "base64Binary")]
        public byte[] EncryptedData { get; set; }
    }

    [Serializable()]
    [DebuggerStepThrough()]
    [DesignerCategory("code")]
    [XmlType(Namespace = "http://www.microsoft.com/SoftwareDistribution")]
    public partial class AuthorizationCookie
    {
        public string PlugInId { get; set; }
        [XmlElement(DataType = "base64Binary")]
        public byte[] CookieData { get; set; }
    }

    public enum ErrorCode
    {
        InvalidCookie,
        ConfigChanged,
        RegistrationRequired,
        ServerChanged,
        InternalServerError,
        CookieExpired,
        InvalidParameters,
        InvalidAuthorizationCookie,
        RegistrationNotRequired,
        RedirectorLocationChanged,
        ServerBusy,
        FileLocationChanged,
        DeploymentSynchronizationNotAllowed,
        IncompatibleProtocolVersion,
        FileDigestsMissing
    }
#pragma warning restore 1591
}
