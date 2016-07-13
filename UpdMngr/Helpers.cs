using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UpdMngr
{
    internal static class Helpers
    {
        internal static bool AreEquals(byte[] x, byte[] y)
        {
            if (null == x) { return false; }
            if (null == y) { return false; }
            int length = x.Length;
            if (y.Length != x.Length) { return false; }
            for(int index = 0; index < length; index++) {
                if (x[index] != y[index]) { return false; }
            }
            return true;
        }
    }
}
