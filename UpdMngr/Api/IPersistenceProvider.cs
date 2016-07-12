using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdMngr.Api
{
    /// <summary>An interface that is implemented by the back end class
    /// handling data persistence.</summary>
    public interface IPersistenceProvider
    {
        IServerIdentity GetServerIdentity(string defaultName = null);
    }
}
