using System;

namespace SymMngr
{
    public class Globals
    {
        static Globals()
        {
            DebugHelpLock = new object();
        }

        /// <summary>Used to enforce the single threading requirements from the
        /// DBGHELP.DLL library.</summary>
        internal static object DebugHelpLock { get; private set; }
    }
}
