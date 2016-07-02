using System;
using System.Runtime.InteropServices;
using System.Text;

namespace SymMngr.Natives
{
    internal static partial class DbgHelp
    {
        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool SymCleanup(
            [In] IntPtr hProcess);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern IntPtr SymGetHomeDirectory(
            [In] DirectoryType type,
            [In] IntPtr dir,
            [In] int size);

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
        internal static extern IntPtr SymSetHomeDirectory(
            [In] DirectoryType type,
            [In] string dir);

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
    }
}
