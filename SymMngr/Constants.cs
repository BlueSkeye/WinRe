using System;

namespace SymMngr
{
    public static class Constants
    {
        public static readonly DateTime Epoch = new DateTime(1970, 1, 1);
        public const int MaxPath = 260;
        public const int OneKiloBytes = 1024;
        public const uint RSDSSignature = 0x53445352;
        public const int SixtyFourKiloBytes = 64 * OneKiloBytes;
    }
}
