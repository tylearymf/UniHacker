using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Platform;

namespace UniHacker
{
    internal class PlatformUtils
    {
        public const string FontFamily = "Microsoft YaHei,Simsun,苹方-简,宋体-简";

        static Stream? s_IconStream;
        public static Stream IconStream
        {
            get
            {
                if (s_IconStream == null)
                {
#pragma warning disable CS8602
                    var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                    s_IconStream = assets.Open(new Uri("avares://UniHacker/Assets/avalonia-logo.ico"));
#pragma warning restore CS8602
                }

                return s_IconStream;
            }
        }

        public static bool IsAdministrator =>
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
            new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator) :
            Mono.Unix.Native.Syscall.geteuid() == 0;

        public static string GetExtension(bool dot = true)
        {
            var extension = string.Empty;
            switch (GetPlatformType())
            {
                case PlatformType.Windows:
                    extension = "exe";
                    break;
                case PlatformType.MacOS:
                    extension = "app";
                    break;
                case PlatformType.Linux:
                    return string.Empty;
            }

            return (dot ? "." : "") + extension;
        }

        public static PlatformType GetPlatformTypeByArch(ArchitectureType type)
        {
            if ((type & ArchitectureType.Windows) != 0)
                return PlatformType.Windows;
            else if ((type & ArchitectureType.MacOS) != 0)
                return PlatformType.MacOS;
            else if ((type & ArchitectureType.Linux) != 0)
                return PlatformType.Linux;
            else
                return PlatformType.Unknown;
        }

        public static PlatformType GetPlatformType()
        {
            if (IsWindows())
                return PlatformType.Windows;
            else if (IsOSX())
                return PlatformType.MacOS;
            else if (IsLinux())
                return PlatformType.Linux;

            return PlatformType.Unknown;
        }

        public static bool IsWindows()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        public static bool IsOSX()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        public static bool IsLinux()
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        public static (string rootPath, string filePath) GetRealFilePath(string filePath)
        {
            var realFilePath = filePath;
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var rootPath = Path.GetDirectoryName(filePath) ?? string.Empty;

            switch (GetPlatformType())
            {
                case PlatformType.MacOS:
                    rootPath = Path.Combine(filePath, "Contents");
                    realFilePath = Path.Combine(rootPath, $"MacOS/{fileName}");
                    break;
                case PlatformType.Linux:
                    if (fileName == "unityhub")
                        realFilePath = Path.Combine(rootPath, "unityhub-bin");
                    break;
            }

            return (rootPath, realFilePath);
        }

        public static (string fileVersion, int majorVersion, int minorVersion) GetFileVersionInfo(string filePath, ArchitectureType architectureType)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var rootPath = Path.GetDirectoryName(filePath);
            var fileVersion = string.Empty;
            var majorVersion = 0;
            var minorVersion = 0;

            switch (GetPlatformTypeByArch(architectureType))
            {
                case PlatformType.Windows:
                    var info = FileVersionInfo.GetVersionInfo(filePath);
                    fileVersion = !string.IsNullOrEmpty(info.ProductVersion) ? info.ProductVersion.Split('_')[0] : Language.GetString(PatchStatus.Unknown.ToString());
                    majorVersion = info.ProductMajorPart;
                    minorVersion = info.ProductMinorPart;
                    return (fileVersion, majorVersion, minorVersion);
                case PlatformType.MacOS:
                    rootPath = Path.Combine(filePath, "Contents");
                    var plistFile = Path.Combine(rootPath, $"Info.plist");
                    var content = File.ReadAllText(plistFile);
                    var match = Regex.Match(content, "<key>CFBundleVersion</key>.*?<string>(?<version>.*?)</string>", RegexOptions.Singleline);
                    if (match.Success)
                    {
                        fileVersion = match.Groups["version"].Value;
                        var versions = fileVersion.Split('.');
                        _ = int.TryParse(versions[0], out majorVersion);
                        _ = int.TryParse(versions[1], out minorVersion);
                        return (fileVersion, majorVersion, minorVersion);
                    }
                    break;
                case PlatformType.Linux:
                    if (fileName.Contains("unityhub", StringComparison.OrdinalIgnoreCase))
                    {
                        var hubPath = Path.GetDirectoryName(rootPath);
                        var infoPath = Path.Combine(hubPath!, "info");
                        if (File.Exists(infoPath))
                        {
                            var infoContent = File.ReadAllText(infoPath);
                            var infoMatch = Regex.Match(infoContent, @"version\"":\s*\""(?<version>.*?)\""", RegexOptions.Singleline);
                            if (infoMatch.Success)
                            {
                                fileVersion = infoMatch.Groups["version"].Value;
                                var versions = fileVersion.Split('.');
                                _ = int.TryParse(versions[0], out majorVersion);
                                _ = int.TryParse(versions[1], out minorVersion);
                                return (fileVersion, majorVersion, minorVersion);
                            }
                        }
                    }
                    else
                    {
#pragma warning disable CS8604
                        var cacheFolder = new DirectoryInfo(Path.Combine(rootPath, "Data/Resources/PackageManager/ProjectTemplates/libcache"));
#pragma warning restore CS8604
                        if (cacheFolder.Exists)
                        {
                            var childFolders = cacheFolder.GetDirectories();
                            foreach (var child in childFolders)
                            {
                                var infoPath = Path.Combine(child.FullName, "Bee/bee_backend.info");
                                if (File.Exists(infoPath))
                                {
                                    var infoContent = File.ReadAllText(infoPath);
                                    var infoMatch = Regex.Match(infoContent, @"UnityVersion\"":\s*\""(?<version>.*?)\""", RegexOptions.Singleline);
                                    if (infoMatch.Success)
                                    {
                                        fileVersion = infoMatch.Groups["version"].Value;
                                        var versions = fileVersion.Split('.');
                                        _ = int.TryParse(versions[0], out majorVersion);
                                        _ = int.TryParse(versions[1], out minorVersion);
                                        return (fileVersion, majorVersion, minorVersion);
                                    }
                                }
                            }
                        }
                    }
                    break;
            }

            return (fileVersion, majorVersion, minorVersion);
        }

        public static async Task<bool> MacOSRemoveQuarantine(string appPath)
        {
            try
            {
                var attrStartInfo = new ProcessStartInfo("xattr", $"-rds com.apple.quarantine \"{appPath}\"");
                var attrProcess = Process.Start(attrStartInfo);
                await attrProcess!.WaitForExitAsync();
                return attrProcess.ExitCode == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    internal enum PlatformType
    {
        Unknown = 0,
        Windows,
        MacOS,
        Linux,
    }
}
