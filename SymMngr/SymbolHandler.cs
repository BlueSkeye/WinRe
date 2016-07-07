using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

using SymMngr.Api;
using SymMngr.Natives;
using SymMngr.PE;

namespace SymMngr
{
    public class SymbolHandler : SafeHandle
    {
        public SymbolHandler(ICollection<string> pathItems = null)
            : base(IntPtr.Zero, true)
        {
            Initialize(pathItems);
            return;
        }

        #region PROPERTIES
        public override bool IsInvalid
        {
            get { return (IntPtr.Zero == base.handle); }
        }

        public string HomeDirectory
        {
            get { return GetHomeDirectory(DbgHelp.DirectoryType.Base); }
            set { SetHomeDirectory(DbgHelp.DirectoryType.Base, value); }
        }

        public Option Options
        {
            get { return Natives.DbgHelp.SymGetOptions(); }
            set { Natives.DbgHelp.SymSetOptions(value); }
        }

        public string SearchPath
        {
            get
            {
                return GetStringProperty(delegate (IntPtr at, int bufferLength) {
                    return DbgHelp.SymGetSearchPath(base.handle, at, bufferLength);
                });
            }
            set
            {
                lock (Globals.DebugHelpLock) {
                    if (!Natives.DbgHelp.SymSetSearchPath(base.handle, value)) {
                        throw new SymbolHandlingException(string.Format("Error : 0x{0:X8}",
                            Marshal.GetLastWin32Error()));
                    }
                }
            }
        }

        public string SourceDirectory
        {
            get { return GetHomeDirectory(DbgHelp.DirectoryType.Source); }
            set { SetHomeDirectory(DbgHelp.DirectoryType.Source, value); }
        }

        public string SymbolDirectory
        {
            get { return GetHomeDirectory(DbgHelp.DirectoryType.Symbols); }
            set { SetHomeDirectory(DbgHelp.DirectoryType.Symbols, value); }
        }
        #endregion

        #region METHODS
        private string FindFile(FindFileDelegate finder)
        {
            Option oldOptions = this.Options;
            IntPtr nativePathBuffer = IntPtr.Zero;
            try {
                nativePathBuffer = Marshal.AllocCoTaskMem(
                    sizeof(char) * (Constants.MaxPath + 1));
                this.Options = oldOptions | Option.Debug;
                if (!finder(nativePathBuffer)) {
                    int lastError = Marshal.GetLastWin32Error();
                    throw new SymbolHandlingException(string.Format("Error : 0x{0:X8}", lastError));
                }
                return Marshal.PtrToStringUni(nativePathBuffer);
            }
            finally {
                this.Options = oldOptions;
                if (IntPtr.Zero != nativePathBuffer) {
                    Marshal.FreeCoTaskMem(nativePathBuffer);
                }
            }
        }

        /// <summary>Find a .exe, .dbg or other standard file.</summary>
        /// <param name="exeFilename">Name of searched file</param>
        /// <param name="datestamp"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        internal string FindExeFile(string exeFilename, uint datestamp, uint size)
        {
            return FindFile(delegate(IntPtr nativePathBuffer) {
                return DbgHelp.SymFindFileInPath(base.handle, null, exeFilename,
                    new IntPtr(datestamp), size, 0, 2, nativePathBuffer, null, IntPtr.Zero);
                });
        }

        internal string FindPdbFile(string pdbFilename, Guid id, uint age)
        {
            return FindFile(delegate (IntPtr nativePathBuffer) {
                return DbgHelp.SymFindFileInPath(base.handle, null, pdbFilename,
                    id, age, 0, 8, nativePathBuffer, null, IntPtr.Zero);
                });
        }

        private string GetHomeDirectory(DbgHelp.DirectoryType kind)
        {
            return GetStringProperty(delegate (IntPtr at, int bufferLength) {
                return (IntPtr.Zero != DbgHelp.SymGetHomeDirectory(kind, at, bufferLength));
            });
        }

        /// <summary>Extract PDB file identity from the given exe file.</summary>
        /// <param name="exeFile"></param>
        /// <returns>The PDB file name or a null reference if no PDB related information
        /// has been found in the exe file.</returns>
        public static string GetPdbFileInfoFromExe(FileInfo exeFile, out Guid id, out uint age)
        {
            if (null == exeFile) { throw new ArgumentNullException(); }
            if (!exeFile.Exists) { throw new FileNotFoundException(); }
            using (LoadedImage exeModule = new LoadedImage(exeFile)) {
                IDataDirectory debugDataDirectory = exeModule.NTHeader[DataDirectoryKind.DebuggingInformation];
                if (null != debugDataDirectory) {
                    IntPtr nativeDebugData = exeModule.MapRva(debugDataDirectory.RelativeVirtualAddress);
                    int debugDataDirectoryOffset = 0;
                    while (debugDataDirectoryOffset < debugDataDirectory.Size) {
                        IDebugDirectory candidate = new DebugDirectory(nativeDebugData,
                            ref debugDataDirectoryOffset);
                        if (DebugInformationType.Codeview != candidate.Type) { continue; }
                        // Based on undocumented CV_INFO_PDB70 structure.
                        IntPtr cvRawData = exeModule.MapRva(candidate.AddressOfRawData);
                        int rawDataOffset = 0;
                        uint cvSignature = ImageHelpers.ReadUint32(cvRawData, ref rawDataOffset);
                        if (Constants.RSDSSignature != cvSignature) { continue; }
                        id = new Guid(ImageHelpers.ReadBytes(cvRawData, 16, ref rawDataOffset));
                        age = ImageHelpers.ReadUint32(cvRawData, ref rawDataOffset);
                        return Marshal.PtrToStringAnsi(cvRawData + rawDataOffset);
                        // CV_INFO_PDB70, *PCV_INFO_PDB70;                    }
                    }
                }
                // Not found. Return default values.
                id = Guid.Empty;
                age = 0;
                return null;
            }
        }

        private string GetStringProperty(GetStringPropertyDelegate getter)
        {
            int bufferLength = Constants.OneKiloBytes;
            IntPtr at = IntPtr.Zero;
            string result;
            do {
                try {
                    at = Marshal.AllocCoTaskMem(bufferLength * sizeof(char));
                    lock (Globals.DebugHelpLock)  {
                        if (!getter(at,bufferLength)) {
                            throw new SymbolHandlingException();
                        }
                    }
                    result = Marshal.PtrToStringUni(at);
                    bufferLength += Constants.OneKiloBytes;
                    if (Constants.SixtyFourKiloBytes <= bufferLength) {
                        throw new SymbolHandlingException();
                    }
                }
                finally {
                    if (IntPtr.Zero != at) {
                        Marshal.FreeCoTaskMem(at);
                    }
                }
            }
            while (string.Empty == result);
            return result;
        }

        private void Initialize(ICollection<string> pathItems = null)
        {
            StringBuilder searchPathBuilder = null;
            if (null != pathItems) {
                searchPathBuilder = new StringBuilder();
            }
            int thisHandleValue;
            lock (Globals.DebugHelpLock) {
                _lastAllocatedHandle += IntPtr.Size;
                thisHandleValue = _lastAllocatedHandle;
            }
            base.SetHandle(new IntPtr(thisHandleValue));
            string searchPath = (null == searchPathBuilder) ? null : searchPathBuilder.ToString();
            if (!DbgHelp.SymInitialize(base.handle, searchPath, false)) {
                Trace.TraceError(
                    "Failed to create symbol handler for path '{0}'. Error : 0x{1:X8}",
                    searchPath ?? "<NULL>", Marshal.GetLastWin32Error());
                throw new SymbolHandlingException();
            }
            // Set default, reasonable search path.
            this.SearchPath = string.Format(
                @"srv*{0}\symbols*https://msdl.microsoft.com/download/symbols",
                Environment.GetEnvironmentVariable("SystemDrive") ?? "C:");
            // TODO : Also attempt to check the DBGHELP.DLL version.
        }

        public void LoadModule(string imageName, string moduleName, ulong at, uint size)
        {
            ulong loadedAt = Natives.DbgHelp.SymLoadModuleEx(base.handle, IntPtr.Zero,
                imageName, moduleName, at, size, IntPtr.Zero, 0);
            if ((0 == loadedAt) && (0 != Marshal.GetLastWin32Error())) {
                throw new SymbolHandlingException();
            }
            return;
        }

        protected override bool ReleaseHandle()
        {
            if (IntPtr.Zero == base.handle) { return true; }
            bool success;
            lock (Globals.DebugHelpLock) { success = DbgHelp.SymCleanup(handle); }
            if (success) {
                return true;
            }
            Trace.TraceError("Failed to cleanup a symbol handler. Error: {0}",
                Marshal.GetLastWin32Error());
            return false;
        }

        private void SetHomeDirectory(DbgHelp.DirectoryType kind, string newHome)
        {
            lock (Globals.DebugHelpLock) {
                if (IntPtr.Zero == Natives.DbgHelp.SymSetHomeDirectory(kind, newHome)) {
                    throw new SymbolHandlingException();
                }
            }
        }
        #endregion

        private static int _lastAllocatedHandle = 1;

        private delegate bool FindFileDelegate(IntPtr nativePathBuffer);
        private delegate bool GetStringPropertyDelegate(IntPtr at, int bufferLength);

        private class LoadedModule
        {
        }

        [Flags()]
        public enum Option: uint
        {
            /// <summary>Enables the use of symbols that are stored with absolute addresses.
            /// Most symbols are stored as RVAs from the base of the module. DbgHelp translates
            /// them to absolute addresses. There are symbols that are stored as an absolute
            /// address. These have very specialized purposes and are typically not used.
            /// DbgHelp 5.1 and earlier:  This value is not supported.</summary>
            AllowAbsoluteSymbols = 0x00000800,
            /// <summary>Enables the use of symbols that do not have an address. By default,
            /// DbgHelp filters out symbols that do not have an address.</summary>
            AllowZeroAddress = 0x01000000,
            /// <summary>Do not search the public symbols when searching for symbols by address,
            /// or when enumerating symbols, unless they were not found in the global symbols
            /// or within the current scope.This option has no effect with SYMOPT_PUBLICS_ONLY.
            /// DbgHelp 5.1 and earlier:  This value is not supported.</summary>
            AutoPublics = 0x00010000,
            /// <summary>All symbol searches are insensitive to case.</summary>
            CaseInsensitive = 0x00000001,
            /// <summary>Pass debug output through OutputDebugString or the
            /// SymRegisterCallbackProc64 callback function.</summary>
            Debug = 0x80000000,
            /// <summary>Symbols are not loaded until a reference is made requiring the symbols
            /// be loaded. This is the fastest, most efficient way to use the symbol handler.
            /// </summary>
            DeferredLoads = 0x00000004,
            /// <summary>Disables the auto-detection of symbol server stores in the symbol
            /// path, even without the "SRV*" designation, maintaining compatibility with
            /// previous behavior. DbgHelp 6.6 and earlier:  This value is not supported.</summary>
            DisableSymbolServerAutoDetection = 0x02000000,
            /// <summary>Do not load an unmatched.pdb file. Do not load export symbols if all
            /// else fails.</summary>
            ExactSymbols = 0x00000400,
            /// <summary>Do not display system dialog boxes when there is a media failure such
            /// as no media in a drive. Instead, the failure happens silently.</summary>
            FailCriticalErrors = 0x00000200,
            /// <summary>If there is both an uncompressed and a compressed file available,
            /// favor the compressed file. This option is good for slow connections.</summary>
            FavorCompressed = 0x00800000,
            /// <summary>Symbols are stored in the root directory of the default downstream
            /// store. DbgHelp 6.1 and earlier:  This value is not supported.</summary>
            FlatDirectory = 0x00400000,
            /// <summary>Ignore path information in the CodeView record of the image header
            /// when loading a .pdb file.</summary>
            IgnoreCodeViewRecord = 0x00000080,
            /// <summary>Ignore the image directory.DbgHelp 6.1 and earlier:  This value is
            /// not supported.</summary>
            IgnoreImageDirectory = 0x00200000,
            /// <summary>Do not use the path specified by _NT_SYMBOL_PATH if the user calls
            /// SymSetSearchPath without a valid path. DbgHelp 5.1:  This value is not
            /// supported.</summary>
            IgnorreNTSYMPATH = 0x00001000,
            /// <summary>When debugging on 64-bit Windows, include any 32-bit modules.</summary>
            Include32BitsModules = 0x00002000,
            /// <summary>Disable checks to ensure a file (.exe, .dbg., or.pdb) is the correct
            /// file.Instead, load the first file located.</summary>
            LoadAnything = 0x00000040,
            /// <summary>Loads line number information.</summary>
            LoadLines = 0x00000010,
            /// <summary>All C++ decorated symbols containing the symbol separator "::" are
            /// replaced by "__". This option exists for debuggers that cannot handle parsing
            /// real C++ symbol names.</summary>
            NoCpp = 0x00000008,
            /// <summary>Do not search the image for the symbol path when loading the symbols
            /// for a module if the module header cannot be read. DbgHelp 5.1:  This value is
            /// not supported.</summary>
            NoImageSearch = 0x00020000,
            /// <summary>Prevents prompting for validation from the symbol server.</summary>
            NoPrompts = 0x00080000,
            /// <summary>Do not search the publics table for symbols.This option should have
            /// little effect because there are copies of the public symbols in the globals
            /// table. DbgHelp 5.1:  This value is not supported.</summary>
            NoPublics = 0x00008000,
            /// <summary>Prevents symbols from being loaded when the caller examines symbols
            /// across multiple modules.Examine only the module whose symbols have already
            /// been loaded.</summary>
            NoUnqualifiedLoads = 0x00000100,
            /// <summary>Overwrite the downlevel store from the symbol store. DbgHelp 6.1 and
            /// earlier:  This value is not supported.</summary>
            Overwrite = 0x00100000,
            /// <summary>Do not use private symbols.The version of DbgHelp that shipped with
            /// earlier Windows release supported only public symbols; this option provides
            /// compatibility with this limitation. DbgHelp 5.1:  This value is not supported.
            /// </summary>
            PublicsOnly = 0x00004000,
            /// <summary>DbgHelp will not load any symbol server other than SymSrv. SymSrv
            /// will not use the downstream store specified in _NT_SYMBOL_PATH.After this
            /// flag has been set, it cannot be cleared. DbgHelp 6.0 and 6.1: This flag can
            /// be cleared. DbgHelp 5.1:  This value is not supported.</summary>
            Secure = 0x00040000,
            /// <summary>All symbols are presented in undecorated form. This option has no
            /// effect on global or local symbols because they are stored undecorated.This
            /// option applies only to public symbols.</summary>
            UndecorateNames = 0x00000002
        }
    }
}

// Pending functions
//SymAddSourceStream
//SymAddSymbol
//SymDeleteSymbol
//SymEnumerateModules64
//SymEnumLines
//SymEnumProcesses
//SymEnumSourceFiles
//SymEnumSourceLines
//SymEnumSymbols
//SymEnumSymbolsForAddr
//SymEnumTypes
//SymEnumTypesByName
//SymFindDebugInfoFile
//SymFindExecutableImage
//SymFindFileInPath
//SymFromAddr
//SymFromIndex
//SymFromName
//SymFromToken
//SymFunctionTableAccess64
//SymGetFileLineOffsets64
//SymGetLineFromAddr64
//SymGetLineFromName64
//SymGetLineNext64
//SymGetLinePrev64
//SymGetModuleBase64
//SymGetModuleInfo64
//SymGetOmaps
//SymGetScope
//SymGetSymbolFile
//SymGetTypeFromName
//SymGetTypeInfo
//SymGetTypeInfoEx
//SymLoadModule64
//SymLoadModuleEx
//SymMatchFileName
//SymMatchString
//SymNext
//SymPrev
//SymRefreshModuleList
//SymRegisterCallback64
//SymRegisterFunctionEntryCallback64
//SymSearch
//SymSetContext
//SymSetScopeFromAddr
//SymSetScopeFromIndex
//SymUnDName64
//SymUnloadModule64
