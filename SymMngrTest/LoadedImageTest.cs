using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SymMngr;
using SymMngr.PE;

namespace SymMngrTest
{
    [TestClass]
    public class LoadedImageTest
    {
        public LoadedImageTest()
        {
        }

        private static FileInfo NtDllPath
        {
            get
            {
                return new FileInfo(
                    Path.Combine(
                        Environment.GetFolderPath(Environment.SpecialFolder.System),
                        "NTDLL.DLL"));
            }
        }

        private TestContext testContextInstance;

        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        [TestMethod]
        public void TestNtDll()
        {
            using (LoadedImage image = new LoadedImage(NtDllPath)) {
                INTHeader ntHeader = image.NTHeader; 
            }
            return;
        }
    }
}
