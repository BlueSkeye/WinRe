using System;
using System.Net;
using System.Web.Services.Protocols;

namespace UpdMngr.WebServices
{
    public abstract class BaseUpdateWebService
        : SoapHttpClientProtocol
    {
        public delegate bool SoapExceptionHandlerDelegate<T>(FaultDetails<T> details);
        public delegate X WSInvokeDelegate<X>();

        public BaseUpdateWebService(string serverName, string versionPrefix, string relativeUrl)
        {
            if (string.IsNullOrEmpty(serverName)) {
                throw new ArgumentNullException();
            }
            if (string.IsNullOrEmpty(relativeUrl)) {
                throw new ArgumentNullException();
            }
            if (relativeUrl.StartsWith("/")) {
                throw new ArgumentException();
            }
            if (!string.IsNullOrEmpty(versionPrefix) && versionPrefix.StartsWith("/")) {
                throw new ArgumentException();
            }
            ServerName = serverName;
            base.Url = string.Format("https://{0}{1}/{2}", _serverName,
                string.IsNullOrEmpty(versionPrefix) ? "" : "/" + versionPrefix, relativeUrl);
            base.UserAgent = Constants.UserAgentValue;
            return;
        }

        public string ServerName
        {
            get { return _serverName; }
            set
            {
                if (string.IsNullOrEmpty(value)) {
                    throw new ArgumentNullException();
                }
                _serverName = value;
            }
        }

        public new string Url
        {
            get { return base.Url; }
            set { throw new NotSupportedException(); }
        }

        /// <summary></summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="E"></typeparam>
        /// <param name="handler"></param>
        /// <param name="defaultError"></param>
        /// <param name="errorHandler"></param>
        /// <returns></returns>
        /// <remarks>Parameters order is important because we want the defaultError
        /// to be provided whenever an errorHandler is. This is far to be bullet proof
        /// due to "named arguments" syntax. We intend this as a hint only.</remarks>
        public T AttemptWithRetry<T,E>(WSInvokeDelegate<T> handler,
            E defaultError = default(E), SoapExceptionHandlerDelegate<E> errorHandler = null)
        {
            _retryCount = 0;
            while (true) {
                try { return handler(); }
                catch (Exception e) {
                    if (null != errorHandler) {
                        SoapException soap = e as SoapException;
                        if (errorHandler(FaultDetails<E>.Create(soap, defaultError))) {
                            continue;
                        }
                    }
                    if (!this.IsExceptionRecoverable(e)) { throw; }
                }
            }
        }

        public virtual bool IsExceptionRecoverable(Exception e)
        {
            return ((e is WebException) && (3 > this._retryCount++));
        }

        private int _retryCount;
        private string _serverName;
    }
}
