using System;
using System.IO;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using UpdMngr;
using UpdMngr.Api;
using UpdMngr.Data;

namespace UpdMngrTest
{
    [TestClass]
    public class DSSToMS
    {
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
