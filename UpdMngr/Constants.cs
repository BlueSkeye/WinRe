using System;

namespace UpdMngr
{
    public static class Constants
    {
        internal const string MicrosoftServerName = "www.update.microsoft.com";
        internal const string ProtocolVersion = "1.8";
        internal const string UserAgentValue =
            "Microsoft WSUS Server 3.2.7600.226 ProtocolVersion: " + ProtocolVersion;

        public static class Classifiers
        {
            public static readonly Guid Application = new Guid("5C9376AB-8CE6-464A-B136-22113DD69801");
            public static readonly Guid Connectors = new Guid("434DE588-ED14-48F5-8EED-A15E09A991F6");
            public static readonly Guid CriticalUpdates = new Guid("E6CF1350-C01B-414D-A61F-263D14D133B4");
            public static readonly Guid DefinitionUpdates = new Guid("E0789628-CE08-4437-BE74-2495B842F43B");
            public static readonly Guid DeveloperKits = new Guid("E140075D-8433-45C3-AD87-E72345B36078");
            public static readonly Guid FeaturePacks = new Guid("B54E7D24-7ADD-428F-8B75-90A396FA584F");
            public static readonly Guid Guidance = new Guid("9511D615-35B2-47BB-927F-F73D8E9260BB");
            public static readonly Guid SecurityUpdates = new Guid("0FA1201D-4330-4FA8-8AE9-B877473B6441");
            public static readonly Guid ServicePacks = new Guid("68C5B0A3-D1A6-4553-AE49-01D3A7827828");
            public static readonly Guid Tools = new Guid("B4832BD8-E735-4761-8DAF-37F882276DAB");
            public static readonly Guid UpdateRollups = new Guid("28BC880E-0592-4CBF-8F95-C79B17911D5F");
            public static readonly Guid Updates = new Guid("CD5FFD1E-E932-4E3A-BF74-18BF0B1BBD83");
        }
    }
}
