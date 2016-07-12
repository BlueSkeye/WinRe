using System;

namespace UpdMngr
{
    public class UpdateManagerException : ApplicationException
    {
        internal UpdateManagerException()
        {
            return;
        }

        internal UpdateManagerException(string message, params object[] args)
            : base(string.Format(message, args))
        {
            return;
        }
    }
}
