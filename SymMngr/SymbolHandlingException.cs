using System;

namespace SymMngr
{
    public class SymbolHandlingException : ApplicationException
    {
        internal SymbolHandlingException()
        {
            return;
        }

        internal SymbolHandlingException(string message)
            : base(message)
        {
            return;
        }
    }
}
