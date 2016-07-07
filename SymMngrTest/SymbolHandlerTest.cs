using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using SymMngr;
using SymMngr.Natives;

namespace SymMngrTest
{
    [TestClass]
    public class SymbolHandlerTest
    {
        [TestMethod]
        public void FindNtDllFile()
        {
            throw new NotImplementedException();
            //using (SymbolHandler handler = new SymbolHandler()) {
            //    string fullPath = handler.FindExeFile("NTDLL.DLL");
            //    int i = 1;
            //}
            //return;
        }

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
            IntPtr dllLibraryCookie = IntPtr.Zero;

            try {
                AppDomain currentDomain = AppDomain.CurrentDomain;
                dllLibraryCookie = Kernel32.AddDllDirectory(currentDomain.BaseDirectory);
                IntPtr nativeDbgHelp = Kernel32.LoadLibraryEx("DBGHELP.DLL", IntPtr.Zero,
                    Kernel32.LoadFlags.SeachUserDirectories);
                if (IntPtr.Zero == nativeDbgHelp) {
                    throw new ApplicationException();
                }
                IntPtr nativeSymSrv = Kernel32.LoadLibraryEx("SYMSRV.DLL", IntPtr.Zero,
                    Kernel32.LoadFlags.SeachUserDirectories);
                if (IntPtr.Zero == nativeSymSrv) {
                    throw new ApplicationException();
                }
                string algExeSearchPath;
                string algPdbSearchPath;
                using (SymbolHandler handler = new SymbolHandler()) {
                    handler.SearchPath =
                        @"srv*c:\symbols*https://msdl.microsoft.com/download/symbols";
                    algExeSearchPath = handler.FindExeFile("alg.exe", 0x5632D7B5, 0x1c000);
                    Guid pdbGuid;
                    uint age;
                    string pdbFileName = SymbolHandler.GetPdbFileInfoFromExe(
                        new FileInfo(algExeSearchPath), out pdbGuid, out age);
                    if (null == pdbFileName) {
                        throw new ApplicationException();
                    }
                    algPdbSearchPath = handler.FindPdbFile(pdbFileName, pdbGuid, age);
                }
                return;
            }
            finally {
                if (null != dllLibraryCookie) {
                    Kernel32.RemoveDllDirectory(dllLibraryCookie);
                }
            }
        }
    }
}
