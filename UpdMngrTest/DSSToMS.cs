using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using UpdMngr;
using UpdMngr.Api;
using UpdMngr.Data;
using UpdMngr.Data.MsDocuments;

namespace UpdMngrTest
{
    [TestClass]
    public class DSSToMS
    {
        [TestMethod]
        public void DeserializeUpdateDocuments()
        {
            XmlBasedPersistenceProvider dataProvider =
                new XmlBasedPersistenceProvider(TestDataDirectory);
            int blobIndex = 0;
            XmlSerializer updateSerializer = Update.Serializer;
            using (DownstreamUpdateServer dss = new DownstreamUpdateServer(dataProvider)) {
                foreach(IUpdateDescriptor descriptor in dss.EnumerateUpdateDescriptors()) {
                    blobIndex++;
                    try {
                        using (MemoryStream rawDescriptor = new MemoryStream(UTF8Encoding.UTF8.GetBytes(descriptor.UpdateBlob))) {
                            Update updateItem = (Update)updateSerializer.Deserialize(rawDescriptor);
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine("Failed to deserialize {0}th document.", blobIndex);
                        throw;
                    }
                }
            }
            Console.WriteLine("Retrieved {0} update blob.", blobIndex);
        }

        [TestMethod]
        public void EnumerateUpdateDocuments()
        {
            XmlBasedPersistenceProvider dataProvider =
                new XmlBasedPersistenceProvider(TestDataDirectory);
            int documentIndex = 0;
            using (DownstreamUpdateServer dss = new DownstreamUpdateServer(dataProvider)) {
                foreach(XmlDocument item in dss.EnumerateUpdateDescriptorDocuments()) {
                    documentIndex++;
                }
            }
            Console.WriteLine("Enumerated {0} documents.", documentIndex);
            int descriptorIndex = 0;
            using (DownstreamUpdateServer dss = new DownstreamUpdateServer(dataProvider)) {
                foreach(IUpdateDescriptor descriptor in dss.EnumerateUpdateDescriptors()) {
                    descriptorIndex++;
                }
            }
            Console.WriteLine("Retrieved {0} descriptors.", descriptorIndex);
        }

        [TestMethod]
        public void RetrieveAndStoreCookie()
        {
            XmlBasedPersistenceProvider dataProvider =
                new XmlBasedPersistenceProvider(TestDataDirectory);
            using (DownstreamUpdateServer dss = new DownstreamUpdateServer(dataProvider)) {
            }
        }

        [TestMethod]
        public void RetrieveAndStoreConfigData()
        {
            XmlBasedPersistenceProvider dataProvider =
                new XmlBasedPersistenceProvider(TestDataDirectory);
            using (DownstreamUpdateServer dss = new DownstreamUpdateServer(dataProvider)) {
                dss.RetrieveUpstreamConfigurationData(null);
                dss.GetRevisionsAndUpdateData(true, false);
            }
        }

        private const string TestDataDirectory = "UpdMngr";
    }
}
