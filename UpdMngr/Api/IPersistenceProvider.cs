using System;
using System.Collections.Generic;
using System.Xml;

namespace UpdMngr.Api
{
    /// <summary>An interface that is implemented by the back end class
    /// handling data persistence.</summary>
    public interface IPersistenceProvider
    {
        IUpdateDescriptor CreateUpdateDescriptor(object container, Guid id, int revision);
        /// <summary>Enumerate an update descriptor object for each update descriptor stored
        /// within the given context.</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IEnumerable<IUpdateDescriptor> EnumerateUpdateDescriptors(IUpstreamServerContext context);
        /// <summary>Enumerate an XML document for each update descriptor stored within
        /// the given context.</summary>
        /// <param name="context"></param>
        /// <returns></returns>
        IEnumerable<XmlDocument> EnumerateUpdateDescriptorDocuments(IUpstreamServerContext context);
        /// <summary></summary>
        /// <param name="context"></param>
        /// <param name="id"></param>
        /// <param name="revision"></param>
        /// <returns>An opaque object that is to be used as the container parameter in
        /// the <see cref="CreateUpdateDescriptor"/> method call.</returns>
        object EnsureUpdateContainer(IUpstreamServerContext context, Guid id, int revision);
        IServerIdentity GetServerIdentity(string defaultName = null);
        IUpstreamServerContext TryGetContext(IServerIdentity owner, string upstreamServerName,
            bool createIfNotFound = false);
        void Save(IUpdateDescriptor descriptor);
    }
}
