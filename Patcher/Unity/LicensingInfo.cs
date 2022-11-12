using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace UniHacker
{
    internal class LicensingInfo
    {
        internal static int s_MajorVersion;
        internal static int s_MinorVersion;

        static async Task<string> GetLicensePath(int majorVersion, int minorVersion)
        {
            var commonAppData = string.Empty;
            switch (PlatformUtils.GetPlatformType())
            {
                case PlatformType.Windows:
                    commonAppData = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
                    break;
                case PlatformType.MacOS:
                    commonAppData = "/Library/Application Support";
                    break;
                case PlatformType.Linux:
                    {
#if DOCKER_ENV
                        commonAppData = "/root/.local/share/unity3d";
#else
                        var userName = await PlatformUtils.GetLinuxUserName();
                        commonAppData = $"/home/{userName}/.local/share/unity3d";
#endif

                        if (!Directory.Exists(commonAppData))
                            Directory.CreateDirectory(commonAppData);
                    }
                    break;
            }

            var unityLicensingPath = Path.Combine(commonAppData, "Unity");
            if (!Directory.Exists(unityLicensingPath))
                Directory.CreateDirectory(unityLicensingPath);

            string? ulfFilePath;
            if (majorVersion == 4)
                ulfFilePath = Path.Combine(unityLicensingPath, "Unity_v4.x.ulf");
            else if (majorVersion == 5)
                ulfFilePath = Path.Combine(unityLicensingPath, "Unity_v5.x.ulf");
            else
                ulfFilePath = Path.Combine(unityLicensingPath, "Unity_lic.ulf");

            return ulfFilePath;
        }

        public static async void TryGenerate(int majorVersion, int minorVersion)
        {
            s_MajorVersion = majorVersion;
            s_MinorVersion = minorVersion;

            var ulfFilePath = await GetLicensePath(majorVersion, minorVersion);
            if (File.Exists(ulfFilePath))
                File.Delete(ulfFilePath);

            var serializer = new XmlSerializer(typeof(LicensingXml));
            var ns = new XmlSerializerNamespaces();
            ns.Add(string.Empty, string.Empty);

            var info = new LicensingXml();
            info.License.SetVersion(majorVersion);
            var settings = new XmlWriterSettings() { Indent = true, OmitXmlDeclaration = true, Encoding = new UTF8Encoding(false), NewLineChars = "\n" };

            using (var fs = File.Open(ulfFilePath, FileMode.OpenOrCreate, FileAccess.Write))
            using (var xmlWriter = XmlWriter.Create(fs, settings))
            {
                serializer.Serialize(xmlWriter, info, ns);
            }

            var xmlDocument = new XmlDocument()
            {
                PreserveWhitespace = true
            };
            xmlDocument.Load(ulfFilePath);
            var signedXml = new SignedXml(xmlDocument)
            {
                SigningKey = new RSACryptoServiceProvider(),
            };
            signedXml.SignedInfo.SignatureMethod = SignedXml.XmlDsigRSASHA1Url;
            var reference = new Reference()
            {
                Uri = "#" + info.License.id,
                DigestMethod = SignedXml.XmlDsigSHA1Url,
            };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            signedXml.AddReference(reference);
            signedXml.ComputeSignature();
            using (var writer = XmlWriter.Create(ulfFilePath, settings))
            {
                var document = new XmlDocument
                {
                    InnerXml = xmlDocument.InnerXml
                };
                var documentElement = document.DocumentElement;
                if (documentElement != null)
                    documentElement.AppendChild(document.ImportNode(signedXml.GetXml(), true));

                document.Save(writer);
            }

            var contents = File.ReadAllText(ulfFilePath).Replace(" />", "/>");
            contents = contents.Replace("<MachineBindings/>", "<MachineBindings></MachineBindings>");
            File.WriteAllText(ulfFilePath, contents);
        }

        public static async void TryRemove(int majorVersion, int minorVersion)
        {
            var ulfFilePath = await GetLicensePath(majorVersion, minorVersion);
            if (File.Exists(ulfFilePath))
                File.Delete(ulfFilePath);
        }
    }

    [XmlRoot("root", Namespace = "", IsNullable = false)]
    public class LicensingXml
    {
        public License License = new();
    }

    public class License
    {
        [XmlAttribute]
        public string id;

        public XmlValue AlwaysOnline = new();
        public XmlValue ClientProvidedVersion = new();
        public XmlValue DeveloperData = new();

        [XmlArray]
        [XmlArrayItem("Feature")]
        public XmlValue[] Features;

        public XmlValue InitialActivationDate = new();
        public XmlValue LicenseVersion = new();
        public XmlEmpty MachineBindings = new();
        public XmlValue MachineID = new();
        public XmlValue SerialHash = new();
        public XmlValue SerialMasked = new();
        public XmlValue StartDate = new();
        public XmlValue StopDate = new();
        public XmlValue UpdateDate = new();

        public License()
        {
            id = "Terms";
            AlwaysOnline.Value = "false";

            var features = new List<XmlValue>();
            var removeFeatures = new HashSet<int>() { 7, 8, 16, 22, 27, 37, 38 };
            for (int i = 0; i < 40; i++)
            {
                if (removeFeatures.Contains(i))
                    continue;

                features.Add(new XmlValue(i));
            }
            features.Add(new XmlValue((int)FeatureID.PS5));
            Features = features.ToArray();

            GenerateSerialNumber();

            InitialActivationDate.Value = DateTime.Now.AddDays(-1).ToString("s", CultureInfo.InvariantCulture);
            UpdateDate.Value = DateTime.Now.AddYears(10).ToString("s", CultureInfo.InvariantCulture);
        }

        public void SetVersion(int majorVersion)
        {
            LicenseVersion.Value = majorVersion switch
            {
                4 => "4.x",
                5 => "5.x",
                _ => "6.x",
            };
        }

        public void GenerateSerialNumber()
        {
            var radom = new Random();
            var serials = new string[]
            {
                    "U3",
                    RamdomString(radom),
                    RamdomString(radom),
                    RamdomString(radom),
                    RamdomString(radom),
                    RamdomString(radom),
            };

            var serialKey = string.Join("-", serials);
            var bytes = new List<byte>();
            bytes.AddRange(new byte[] { 1, 0, 0, 0 });
            bytes.AddRange(Encoding.ASCII.GetBytes(serialKey));
            DeveloperData.Value = Convert.ToBase64String(bytes.ToArray());
            SerialMasked.Value = serialKey.Remove(serialKey.Length - 4, 4) + "XXXX";
        }

        static string RamdomString(Random random)
        {
            var text = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return text[random.Next(0, 36)].ToString() +
                   text[random.Next(0, 36)].ToString() +
                   text[random.Next(0, 36)].ToString() +
                   text[random.Next(0, 36)].ToString();
        }
    }

    public class XmlValue
    {
        [XmlAttribute]
        public string Value = "";

        public XmlValue() { }
        public XmlValue(string value) => Value = value;
        public XmlValue(int value) => Value = value.ToString();
    }

    public class XmlEmpty
    {
    }
}
