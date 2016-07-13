using System;
using System.IO;
using System.Text;

namespace UpdMngr
{
    internal static class Extensions
    {
        internal static DirectoryInfo EnsureSubDirectory(this DirectoryInfo parent, string name)
        {
            if (null == parent) { throw new ArgumentNullException(); }
            if (!parent.Exists) {
                throw new InvalidOperationException();
            }
            DirectoryInfo result = new DirectoryInfo(Path.Combine(parent.FullName, name));
            if (!result.Exists) {
                result.Create();
                result.Refresh();
            }
            return result;
        }

        internal static string HexadecimalString(this byte[] data)
        {
            StringBuilder builder = new StringBuilder();
            foreach(byte scannedByte in data) {
                builder.AppendFormat("{0:X2}", scannedByte);
            }
            return builder.ToString();
        }
    }
}
