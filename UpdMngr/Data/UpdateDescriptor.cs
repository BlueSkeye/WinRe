using System;
using System.Collections.Generic;
using System.Xml.Serialization;

using UpdMngr.Api;

namespace UpdMngr.Data
{
    [XmlRoot("updDescriptor")]
    public class UpdateDescriptor : IUpdateDescriptor
    {
        public UpdateDescriptor()
        {
            return;
        }

        internal UpdateDescriptor(XmlBasedPersistenceProvider.UpdateContainer container,
            Guid id, int revision)
        {
            if (null == container) {
                throw new ArgumentNullException();
            }
            _container = container;
            _files = new List<File>();
            Id = id;
            Revision = revision;
            return;
        }

        [XmlIgnore()]
        public object Container
        {
            get { return _container; }
        }

        [XmlAttribute("id")]
        public Guid Id { get; set; }

        [XmlIgnore()]
        public bool IsReadOnly { get; private set; }

        [XmlArray("files")]
        public List<File> Files
        {
            get { return _files; }
            set { _files = value; }
        }

        [XmlAttribute("revision")]
        public int Revision { get; set; }

        [XmlElement("blob")]
        public string UpdateBlob
        {
            get { return _updateBlob; }
            set {
                AssertNotReadOnly();
                _updateBlob = value;
            }
        }

        public void AddFile(byte[] digest, string updateUrl)
        {
            _files.Add(new File(updateUrl, digest));
        }

        private void AssertNotReadOnly()
        {
            if (!IsReadOnly) { return; }
            throw new InvalidOperationException();
        }

        private XmlBasedPersistenceProvider.UpdateContainer _container;
        private List<File> _files;
        private string _updateBlob;

        public class File
        {
            public File()
            {
            }

            public File(string updateUrl, byte[] hash)
            {
                Url = updateUrl;
                Hash = hash;
            }

            [XmlAttribute("sha1Hash", DataType = "base64Binary")]
            public byte[] Hash { get; set; }

            [XmlElement("url", IsNullable = true)]
            public string Url { get; set; }
        }
    }
}
