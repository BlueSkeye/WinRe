using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using SymMngr.Natives;

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

        public override bool IsInvalid
        {
            get { return (IntPtr.Zero == base.handle); }
        }

        public string SearchPath
        {
            get
            {
                lock (_globalLock) {
                    // See ymGetSearchPath 
                }
            }
            set
            {
                lock (_globalLock) {
                    // See SymSetSearchPath
                }
            }
        }

        private void Initialize(ICollection<string> pathItems = null)
        {
            StringBuilder searchPathBuilder = null;
            if (null != pathItems) {
                searchPathBuilder = new StringBuilder();
            }
            int thisHandleValue;
            lock (_globalLock) {
                _lastAllocatedHandle += IntPtr.Size;
                thisHandleValue = _lastAllocatedHandle;
            }
            base.SetHandle(new IntPtr(thisHandleValue));
            string searchPath = (null == searchPathBuilder) ? null : searchPathBuilder.ToString();
            bool success = DbgHelp.SymInitialize(base.handle, searchPath, false);
            if (success) {
                return;
            }
            Trace.TraceError(
                "Failed to create symbol handler for path '{0}'. Error : 0x{1:X8}",
                searchPath ?? "<NULL>", Marshal.GetLastWin32Error());
            throw new SymbolHandlingException();
        }

        protected override bool ReleaseHandle()
        {
            if (IntPtr.Zero == base.handle) { return true; }
            bool success;
            lock (_globalLock) { success = DbgHelp.SymCleanup(handle); }
            if (success) {
                return true;
            }
            Trace.TraceError("Failed to cleanup a symbol handler. Error: {0}",
                Marshal.GetLastWin32Error());
            return false;
        }

        private static object _globalLock = new object();
        private static int _lastAllocatedHandle = 1;

        private class LoadedModule
        {
        }
    }
}
