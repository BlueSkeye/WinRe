using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdMngr.Api
{
    public interface IUpdateDescriptor
    {
        object Container { get; }
        bool IsReadOnly { get; }
        string UpdateBlob { get; set; }

        void AddFile(byte[] digest, string updateUrl);
    }
}
