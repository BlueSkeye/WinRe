using System;
using System.Xml;
using System.Web.Services.Protocols;

namespace UpdMngr.WebServices
{
    public class FaultDetails<E>
    {
        public FaultDetails(E errorCode, string message, string method, Guid id)
        {
            ErrorCode = errorCode;
            Message = message;
            Id = id;
            Method = method;
        }

        public static FaultDetails<E> Create(SoapException e, E defaultError)
        {
            if (e == null) {
                throw new ArgumentNullException("e");
            }
            XmlNode detail = e.Detail;
            if (null == detail) { return null; }
            E errorCode = defaultError;
            Guid id = Guid.Empty;
            string message = string.Empty;
            string method = string.Empty;
            for (XmlNode xmlNode = detail.FirstChild; null != xmlNode; xmlNode = xmlNode.NextSibling) {
                string innerText = xmlNode.InnerText;
                switch (xmlNode.LocalName) {
                    case null:
                        continue;
                    case "ErrorCode":
                        if (null != innerText) {
                            errorCode = (E)Enum.Parse(typeof(E), innerText);
                        }
                        continue;
                    case "ID":
                        if (null != innerText) {
                            id = new Guid(innerText);
                        }
                        continue;
                    case "Message":
                        message = xmlNode.InnerText;
                        continue;
                    case "Method":
                        method = xmlNode.InnerText ?? string.Empty;
                        continue;
                }
            }
            return new FaultDetails<E>(errorCode, message, method, id);
        }

        public E ErrorCode { get; private set; }
        public Guid Id { get; private set; }
        public string Message { get; private set; }
        public string Method { get; private set; }
    }
}
