using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SymMngr;

namespace SymMngrTest
{
    [TestClass]
    public class SymbolHandlerTest
    {
        [TestMethod]
        public void InitAndClose()
        {
            using (new SymbolHandler()) {
            }
        }

        [TestMethod]
        public void TestHomeDirectories()
        {
            using (SymbolHandler handler = new SymbolHandler()) {
                handler.HomeDirectory = handler.HomeDirectory;
                handler.SourceDirectory = handler.SourceDirectory;
                handler.SymbolDirectory = handler.SymbolDirectory;
            }
            return;
        }

        [TestMethod]
        public void TestSearchPath()
        {
            string searchPath;
            using (SymbolHandler handler = new SymbolHandler()) {
                searchPath = handler.SearchPath;
            }
            using (SymbolHandler handler = new SymbolHandler()) {
                handler.SearchPath = searchPath;
            }
            return;
        }
    }
}
