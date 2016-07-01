using System;
using System.Runtime.InteropServices;

namespace SymMngr.Natives
{
    internal static partial class DbgHelp
    {
        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError = true)]
        internal static extern bool SymCleanup(
            [In] IntPtr hProcess);

        [DllImport(DllName, CharSet = CharSet.Unicode, PreserveSig = true, SetLastError=true)]
        internal static extern bool SymInitialize(
            [In] IntPtr hProcess,
            [In] string UserSearchPath,
            [In] bool fInvadeProcess);

        private const string DllName = "DBGHELP.DLL";
    }
}
