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
        }

        private FileInfo ServerIdFile
        {
            get { return new FileInfo(Path.Combine(_baseDirectory.FullName, ServerIdFileName)); }
        }

        public IServerIdentity GetServerIdentity(string defaultName = null)
        {
            try {
                FileStream input = null;
                try {
                    try { input = File.Open(ServerIdFile.FullName, FileMode.Open, FileAccess.Read); }
                    catch { }
                    XmlSerializer serializer = new XmlSerializer(typeof(ServerIdentity));
                    ServerIdentity result = (ServerIdentity)serializer.Deserialize(input);
                    result.Owner = this;
                    return result;
                }
                finally { if (null != input) { input.Close(); } }
            }
            catch {
                if (null == defaultName) {
                    throw new InvalidOperationException();
                }
                ServerIdentity result = new ServerIdentity() {
                    Owner = this
                };
                result.ServerName = defaultName;
                result.ServerId = Guid.NewGuid().ToString();
                RewriteIdentity(result);
                return result;
            }
        }

        internal void RewriteIdentity(ServerIdentity newIdentity)
        {
            FileStream output = null;
            try {
                try { output = File.Open(ServerIdFile.FullName, FileMode.Create, FileAccess.Write); }
                catch { }
                XmlSerializer serializer = new XmlSerializer(typeof(ServerIdentity));
                using (StreamWriter writer = new StreamWriter(output)) {
                    serializer.Serialize(writer, newIdentity);
                }
            }
            finally { if (null != output) { output.Close(); } }
        }

        private const string ServerIdFileName = "updsrvid.xml";
        private DirectoryInfo _baseDirectory;
    }
}
