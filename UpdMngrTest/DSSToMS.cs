using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using UpdMngr;

namespace UpdMngrTest
{
    [TestClass]
    public class DSSToMS
    {
        [TestMethod]
        public void RetrieveAndStoreCookie()
        {
            using (DownstreamUpdateServer dss =
                new DownstreamUpdateServer(ServerIdentityManager.Instance)) {
            }
        }

        private class ServerIdentityManager : IServerIdentityManager
        {
            private ServerIdentityManager()
            {
                FileStream input = null;
                bool retrievedFromFile = false;
                try {
                    try { input = File.Open(ServerIdFile.FullName, FileMode.Open, FileAccess.Read); }
                    catch { }
                    if (null != input) {
                        using (StreamReader reader = new StreamReader(input, Encoding.UTF8)) {
                            if (   (null != (ServerName = reader.ReadLine()))
                                && (null != (ServerId = reader.ReadLine())))
                            {
                                retrievedFromFile = true;
                                try { new AuthCookie(reader); }
                                catch { }
                            }
                        }
                    }
                }
                finally { if (null != input) { input.Close(); } }
                if (retrievedFromFile) { return; }
                ServerName = "garamond.contoso.com";
                ServerId = Guid.NewGuid().ToString();
                using (FileStream output = File.Open(ServerIdFile.FullName, FileMode.OpenOrCreate, FileAccess.Write)) {
                    using (StreamWriter writer = new StreamWriter(output)) {
                        WriteServerNameAndId(writer);
                    }
                }
                return;
            }

            public IReadOnlyAuthorizationCookie AuthorizationCookie
            {
                get { return AuthCookie.Instance; }
            }

            public static IServerIdentityManager Instance
            {
                get { return _instance; }
            }

            public string ServerId { get; internal set; }

            private static FileInfo ServerIdFile
            {
                get
                {
                    return new FileInfo(
                        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                            "updsrvid.txt"));
                }
            }

            public string ServerName { get; internal set; }

            public void RegisterAuthorizationCookieData(string pluginId, byte[] data)
            {
                new AuthCookie(pluginId, data, WriteServerNameAndId);
                return;
            }

            private void WriteServerNameAndId(StreamWriter into)
            {
                into.WriteLine(ServerName);
                into.WriteLine(ServerId);
            }

            private static ServerIdentityManager _instance = new ServerIdentityManager();

            private class AuthCookie : IReadOnlyAuthorizationCookie
            {
                internal delegate void HeaderWriterDelegate(StreamWriter into);

                static AuthCookie()
                {
                    FileInfo serverIdFilePath = ServerIdFile;
                    if (File.Exists(serverIdFilePath.FullName)) {
                        Instance = new AuthCookie();
                        bool success = false;
                        try {
                            using (FileStream input = File.Open(serverIdFilePath.FullName, FileMode.Open, FileAccess.Read)) {
                                using (StreamReader reader = new StreamReader(input, Encoding.UTF8)) {
                                    if (!string.IsNullOrEmpty(Instance.PlugInId = reader.ReadLine())) {
                                        string rawData = reader.ReadLine();
                                        if (   !string.IsNullOrEmpty(rawData)
                                            && (0 == (rawData.Length % 2)))
                                        {
                                            rawData = rawData.ToUpper();
                                            List<byte> bytes = new List<byte>();
                                            while (true) {
                                                string item = rawData.Substring(0, 2);
                                                byte rawByte;
                                                byte.TryParse(item, NumberStyles.AllowHexSpecifier, null, out rawByte);
                                                bytes.Add(rawByte);
                                                if (2 == rawData.Length) { break; }
                                                rawData = rawData.Substring(2);
                                            }
                                            success = true;
                                            Instance.CookieData = bytes.ToArray();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e) { }
                        if (!success) { Instance = null; }
                    }
                }

                private AuthCookie()
                {
                    return;
                }

                internal AuthCookie(StreamReader from)
                {
                    if (string.IsNullOrEmpty(this.PlugInId = from.ReadLine())) {
                        throw new ApplicationException();
                    }
                    string rawData = from.ReadLine();
                    if (string.IsNullOrEmpty(rawData)) {
                        throw new ApplicationException();
                    }
                    if (0 != (rawData.Length % 2)) {
                        throw new ApplicationException();
                    }
                    rawData = rawData.ToUpper();
                    List<byte> bytes = new List<byte>();
                    while (true) {
                        string item = rawData.Substring(0, 2);
                        byte rawByte;
                        if (!byte.TryParse(item, NumberStyles.AllowHexSpecifier, null, out rawByte)) {
                            throw new ApplicationException();
                        }
                        bytes.Add(rawByte);
                        if (2 == rawData.Length) { break; }
                        rawData = rawData.Substring(2);
                    }
                    this.CookieData = bytes.ToArray();
                    Instance = this;
                }

                internal AuthCookie(string id, byte[] data, HeaderWriterDelegate headerWriter)
                {
                    if (string.IsNullOrEmpty(id)) { throw new ArgumentNullException(); }
                    if ((null == data) || (0 == data.Length)) { throw new ArgumentNullException(); }
                    PlugInId = id;
                    CookieData = data;
                    Instance = this;
                    using (FileStream output = File.Open(ServerIdFile.FullName, FileMode.OpenOrCreate, FileAccess.Write)) {
                        using (StreamWriter writer = new StreamWriter(output, Encoding.UTF8)) {
                            headerWriter(writer);
                            writer.WriteLine(id);
                            foreach(byte item in data) {
                                writer.Write(string.Format("{0:X2}", item));
                            }
                            writer.WriteLine();
                        }
                    }
                }

                public byte[] CookieData { get; private set; }

                public string PlugInId { get; private set; }

                internal static AuthCookie Instance { get; private set; }
            }
        }
    }
}
