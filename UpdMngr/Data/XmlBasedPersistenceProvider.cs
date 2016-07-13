using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using UpdMngr.Api;

namespace UpdMngr.Data
{
    /// <summary>A simple, XML based persistence provider based on various
    /// XML files grouped in a single directory.</summary>
    public class XmlBasedPersistenceProvider : IPersistenceProvider
    {
        public XmlBasedPersistenceProvider(string directoryName)
        {
            if (string.IsNullOrEmpty(directoryName)) {
                throw new ArgumentNullException();
            }
            _baseDirectory = new DirectoryInfo(
                Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    directoryName));
            if (!_baseDirectory.Exists) {
                _baseDirectory.Create();
                _baseDirectory.Refresh();
            }
            if (null == UpdateSerializer) {
                UpdateSerializer = new XmlSerializer(typeof(UpdateDescriptor));
            }
        }

        private FileInfo ServerIdFile
        {
            get { return new FileInfo(Path.Combine(_baseDirectory.FullName, ServerIdFileName)); }
        }

        public IUpdateDescriptor CreateUpdateDescriptor(object container, Guid id, int revision)
        {
            if (null == container) {
                throw new ArgumentNullException();
            }
            UpdateContainer realContainer = container as UpdateContainer;
            if (null == realContainer) {
                throw new ArgumentException();
            }
            return new UpdateDescriptor(realContainer, id, revision);
        }

        private DirectoryInfo EnsureStorageDirectory(IServerIdentity owner)
        {
            // TODO : Should perform some kind of canonicalization on server name.
            DirectoryInfo targetDirectory = new DirectoryInfo(
                Path.Combine(_baseDirectory.FullName, owner.ServerName));
            if (!targetDirectory.Exists) {
                targetDirectory.Create();
                targetDirectory.Refresh();
            }
            return targetDirectory;
        }

        public object EnsureUpdateContainer(IUpstreamServerContext context, Guid id, int revision)
        {
            return new UpdateContainer(
                EnsureUpstreamServerContextDirectory(
                    EnsureStorageDirectory(context.Owner), context.ServerName)
                .EnsureSubDirectory(id.ToString()));
        }

        private static DirectoryInfo EnsureUpstreamServerContextDirectory(DirectoryInfo storeInto,
            string serverName)
        {
            // TODO : Should perform some kind of canonicalization on server name.
            DirectoryInfo result = new DirectoryInfo(Path.Combine(storeInto.FullName, serverName));
            if (!result.Exists) {
                result.Create();
                result.Refresh();
            }
            return result;
        }

        private static XmlSerializer GetSerializer(Type forType)
        {
            if (null == _serializerByType) {
                _serializerByType = new Dictionary<Type, XmlSerializer>();
            }
            XmlSerializer result;
            if (!_serializerByType.TryGetValue(forType, out result)) {
                result = new XmlSerializer(forType);
                _serializerByType.Add(forType, result);
            }
            return result;
        }

        public IServerIdentity GetServerIdentity(string defaultName = null)
        {
            try {
                FileStream input = null;
                try {
                    try { input = File.Open(ServerIdFile.FullName, FileMode.Open, FileAccess.Read); }
                    catch { }
                    XmlSerializer serializer = GetSerializer(typeof(ServerIdentity));
                    ServerIdentity result = (ServerIdentity)serializer.Deserialize(input);
                    result.PersistanceHandler = this;
                    return result;
                }
                finally { if (null != input) { input.Close(); } }
            }
            catch {
                if (null == defaultName) {
                    throw new InvalidOperationException();
                }
                ServerIdentity result = new ServerIdentity() {
                    PersistanceHandler = this
                };
                result.ServerName = defaultName;
                result.ServerId = Guid.NewGuid().ToString();
                Rewrite(result);
                return result;
            }
        }

        private FileInfo GetUpdateDescriptorFile(UpdateDescriptor descriptor)
        {
            UpdateContainer container = descriptor.Container as UpdateContainer;
            if (null == container) {
                throw new ArgumentException();
            }
            return new FileInfo(
                Path.Combine(container.Container.FullName,
                    string.Format("{0}-Rev{1}.xml", descriptor.Id, descriptor.Revision)));
        }

        private static string GetUpstreamServerContextFileName(DirectoryInfo storeInto,
            UpstreamServerContext context)
        {
            return GetUpstreamServerContextFileName(storeInto, context.ServerName);
        }

        private static string GetUpstreamServerContextFileName(DirectoryInfo storeInto,
            string serverName)
        {
            storeInto = EnsureUpstreamServerContextDirectory(storeInto, serverName);
            // TODO : Should perform some kind of canonicalization on server name.
            return Path.Combine(storeInto.FullName,
                string.Format(UpstreamServerContextNamePattern, serverName));
        }

        internal void Rewrite(ServerIdentity newIdentity)
        {
            Rewrite(ServerIdFile.FullName, typeof(ServerIdentity), newIdentity);
        }

        internal void Rewrite(UpstreamServerContext context)
        {
            DirectoryInfo storeInto = EnsureStorageDirectory(context.Owner);
            // TODO : Consider persisting the context owner server name or
            // identifier as an attribute for consistency checking.
            Rewrite(
                GetUpstreamServerContextFileName(storeInto, context),
                typeof(UpstreamServerContext), context);
        }

        internal void Rewrite(string fullFileName, Type serializedType, object item)
        {
            using (FileStream output = File.Open(fullFileName, FileMode.Create, FileAccess.Write)) {
                XmlSerializer serializer = GetSerializer(serializedType);
                using (StreamWriter writer = new StreamWriter(output)) {
                    serializer.Serialize(writer, item);
                }
            }
        }

        private T ReadFromStorage<T>(string  fullFileName)
            where T : BasePersistenceDocument
        {
            using (FileStream input = File.Open(fullFileName, FileMode.Open, FileAccess.Read)) {
                XmlSerializer serializer = GetSerializer(typeof(T));
                using (StreamReader reader = new StreamReader(input)) {
                    T result = (T)serializer.Deserialize(reader);
                    result.PersistanceHandler = this;
                    return result;
                }
            }
        }

        public void Save(IUpdateDescriptor descriptor)
        {
            if (null == descriptor) { throw new ArgumentNullException(); }
            UpdateDescriptor realDescriptor = descriptor as UpdateDescriptor;
            if (null == realDescriptor) {
                throw new ArgumentException();
            }
            FileInfo descriptorFile = GetUpdateDescriptorFile(realDescriptor);
            using (FileStream into = File.Open(descriptorFile.FullName, FileMode.Create, FileAccess.Write)) {
                UpdateSerializer.Serialize(into, descriptor);
            }
            return;
        }

        public IUpstreamServerContext TryGetContext(IServerIdentity owner, string upstreamServerName,
            bool createIfNotFound)
        {
            if (null == owner) {
                throw new ArgumentNullException();
            }
            if (string.IsNullOrEmpty(upstreamServerName)) {
                throw new ArgumentNullException();
            }
            DirectoryInfo storeInto = EnsureStorageDirectory(owner);
            try {
                UpstreamServerContext result = ReadFromStorage<UpstreamServerContext>(
                    GetUpstreamServerContextFileName(storeInto, upstreamServerName));

                result.Owner = owner;
                return result;
            }
            catch {
                return (createIfNotFound) ? new UpstreamServerContext(this) : null;
            }
        }

        private const string ServerIdFileName = "updsrvid.xml";
        private const string UpstreamServerContextNamePattern = "USS-{0}.xml";
        private DirectoryInfo _baseDirectory;
        private static Dictionary<Type, XmlSerializer> _serializerByType;
        private static XmlSerializer UpdateSerializer;

        internal class UpdateContainer
        {
            internal UpdateContainer(DirectoryInfo container)
            {
                Container = container;
            }

            internal DirectoryInfo Container { get; private set; }
        }
    }
}
