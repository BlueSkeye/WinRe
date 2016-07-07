using System;
using System.Runtime.InteropServices;

namespace SymMngr.Natives
{
    internal static partial class DbgHelp
    {
        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern int GetTimestampForLoadedLibrary(
            [In] IntPtr ImageBase);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern object ImageDirectoryEntryToDataEx(
            [In] IntPtr Base,
            [In] bool MappedAsImage,
            [In] ushort DirectoryEntry,
            [Out] out uint Size,
            [In] IntPtr /* PIMAGE_SECTION_HEADER */ FoundHeader);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr /* PIMAGE_NT_HEADERS */ ImageNtHeader(
            [In] IntPtr ImageBase);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr /* PIMAGE_SECTION_HEADER */ ImageRvaToSection(
            [In] IntPtr /* PIMAGE_NT_HEADERS */ NtHeaders,
            [In] IntPtr Base,
            [In] uint Rva);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr ImageRvaToVa(
            [In] IntPtr /* PIMAGE_NT_HEADERS */ NtHeaders,
            [In] IntPtr Base,
            [In] uint Rva,
            [In, Out] ref IntPtr /* PIMAGE_SECTION_HEADER */ LastRvaSection);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool SymCleanup(
            [In] IntPtr hProcess);

        internal delegate bool SymFindFileInPathProcDelegate(string filename, IntPtr context);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool SymFindFileInPath(
            [In] IntPtr hProcess,
            [In] string SearchPath,
            [In] string FileName,
            [In] IntPtr id,
            [In] uint two,
            [In] uint three,
            [In] uint flags,
            [In] IntPtr FilePath,
            [In] SymFindFileInPathProcDelegate callback,
            [In] IntPtr context);
        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool SymFindFileInPath(
            [In] IntPtr hProcess,
            [In] string SearchPath,
            [In] string FileName,
            [In] Guid id,
            [In] uint two,
            [In] uint three,
            [In] uint flags,
            [In] IntPtr FilePath,
            [In] SymFindFileInPathProcDelegate callback,
            [In] IntPtr context);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr SymGetHomeDirectory(
            [In] DirectoryType type,
            [In] IntPtr dir,
            [In] int size);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern SymbolHandler.Option SymGetOptions();

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool SymGetSearchPath(
            [In] IntPtr hProcess,
            [In] IntPtr SearchPath,
            [In] int SearchPathLength);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError=true)]
        internal static extern bool SymInitialize(
            [In] IntPtr hProcess,
            [In] string UserSearchPath,
            [In] bool fInvadeProcess);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern ulong SymLoadModuleEx(
            [In] IntPtr hProcess,
            [In] IntPtr hFile,
            [In] string ImageName,
            [In] string ModuleName,
            [In] ulong BaseOfDll,
            [In] uint DllSize,
            [In] IntPtr /* PMODLOAD_DATA */ Data,
            [In] ModuleLoadFlags Flags);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr SymSetHomeDirectory(
            [In] DirectoryType type,
            [In] string dir);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern SymbolHandler.Option SymSetOptions(
            [In] SymbolHandler.Option SymOptions);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool SymSetSearchPath(
            [In] IntPtr hProcess,
            [In] string SearchPath);

        private const string DllName = "DBGHELP.DLL";

        internal enum DirectoryType
        {
            Base = 0,
            Symbols = 1,
            Source = 2,
        }

        [Flags()]
        internal enum ModuleLoadFlags
        {
            /// <summary>Loads the module but not the symbols for the module.</summary>
            NoSymbols = 0x04,
            /// <summary>Creates a virtual module named ModuleName at the address specified
            /// in BaseOfDll.To add symbols to this module, call the SymAddSymbol function.
            /// </summary>
            Virtual = 0x01,
        }
    }
}
