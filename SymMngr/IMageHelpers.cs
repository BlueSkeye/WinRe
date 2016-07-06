using System;
using System.Runtime.InteropServices;

namespace SymMngr
{
    internal static class ImageHelpers
    {
        private static void Align(IntPtr baseAddress, int alignment, ref int offset)
        {
            baseAddress += offset;
            long raw = baseAddress.ToInt64();
            long delta = raw % alignment;
            if (0 != delta) {
                offset += (int)(alignment - delta);
            }
            return;
        }

        internal static byte ReadByte(IntPtr at, ref int offset)
        {
            try {
                // Useless for a byte.
                Align(at, sizeof(byte), ref offset);
                return Marshal.ReadByte(at, offset);
            }
            finally { offset += sizeof(byte); }
        }

        internal static ushort ReadUint16(IntPtr at, ref int offset)
        {
            try {
                Align(at, sizeof(ushort), ref offset);
                return (ushort)Marshal.ReadInt16(at, offset);
            }
            finally { offset += sizeof(ushort); }
        }

        internal static uint ReadUint32(IntPtr at, ref int offset)
        {
            try {
                Align(at, sizeof(uint), ref offset);
                return (uint)Marshal.ReadInt32(at, offset);
            }
            finally { offset += sizeof(uint); }
        }

        internal static ulong ReadUint64(IntPtr at, ref int offset)
        {
            try {
                Align(at, sizeof(ulong), ref offset);
                return (ulong)Marshal.ReadInt64(at, offset);
            }
            finally { offset += sizeof(ulong); }
        }
    }
}
