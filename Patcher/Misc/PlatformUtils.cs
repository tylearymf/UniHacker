using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
#if !DOCKER_ENV
using Avalonia;
using Avalonia.Platform;
#endif

namespace UniHacker
{
    internal class PlatformUtils
    {
        public const string FontFamily = "Microsoft YaHei,Simsun,苹方-简,宋体-简";

#if !DOCKER_ENV
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
#endif

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
                    if (fileName.Contains("unityhub"))
                        realFilePath = Path.Combine(rootPath, "unityhub-bin");
                    break;
            }

            return (rootPath, realFilePath);
        }

        public static (string fileVersion, int majorVersion, int minorVersion, int buildVersion) GetFileVersionInfo(string filePath, ArchitectureType architectureType)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var rootPath = Path.GetDirectoryName(filePath);
            var fileVersion = string.Empty;
            var majorVersion = 0;
            var minorVersion = 0;
            var buildVersion = 0;

            switch (GetPlatformTypeByArch(architectureType))
            {
                case PlatformType.Windows:
                    var info = FileVersionInfo.GetVersionInfo(filePath);
                    fileVersion = !string.IsNullOrEmpty(info.ProductVersion) ? info.ProductVersion.Split('_')[0] : Language.GetString(PatchStatus.Unknown.ToString());
                    majorVersion = info.ProductMajorPart;
                    minorVersion = info.ProductMinorPart;
                    buildVersion = info.ProductBuildPart;
                    return (fileVersion, majorVersion, minorVersion, buildVersion);
                case PlatformType.MacOS:
                    rootPath = Path.Combine(filePath, "Contents");
                    var plistFile = Path.Combine(rootPath, $"Info.plist");
                    var content = File.ReadAllText(plistFile);
                    var match = Regex.Match(content, "<key>CFBundleVersion</key>.*?<string>(?<version>.*?)</string>", RegexOptions.Singleline);
                    if (match.Success)
                    {
                        fileVersion = match.Groups["version"].Value;
                        ParseVersionStr(fileVersion, ref majorVersion, ref minorVersion, ref buildVersion);
                        return (fileVersion, majorVersion, minorVersion, buildVersion);
                    }
                    break;
                case PlatformType.Linux:
                    if (fileName.Contains("unityhub", StringComparison.OrdinalIgnoreCase))
                    {
                        var asarPath = Path.Combine(rootPath!, "resources/app.asar");
                        var asarBakPath = Path.Combine(rootPath!, "resources/app.asar.bak");
                        if (File.Exists(asarPath) || File.Exists(asarBakPath))
                        {
                            var asarContent = string.Empty;

                            if (File.Exists(asarPath))
                                asarContent = File.ReadAllText(asarPath);
                            else
                                asarContent = File.ReadAllText(asarBakPath);

                            var infoMatch = Regex.Match(asarContent, @"""name"":\s""unityhub"",.*?""version"":\s""(?<version>.*?)"",", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                            if (infoMatch.Success)
                            {
                                fileVersion = infoMatch.Groups["version"].Value;
                                ParseVersionStr(fileVersion, ref majorVersion, ref minorVersion, ref buildVersion);
                                return (fileVersion, majorVersion, minorVersion, buildVersion);
                            }
                            else
                            {
                                MessageBox.Show(Language.GetString("Hub_patch_error2"));
                            }
                        }
                        else
                        {
                            MessageBox.Show(Language.GetString("Hub_patch_error1"));
                        }
                    }
                    else
                    {
                        fileVersion = TryGetVersionOfUnity(filePath);
                        if (!string.IsNullOrEmpty(fileVersion))
                        {
                            ParseVersionStr(fileVersion, ref majorVersion, ref minorVersion, ref buildVersion);
                            return (fileVersion, majorVersion, minorVersion, buildVersion);
                        }
                        else
                        {
                            MessageBox.Show(Language.GetString("Unity_patch_error1"));
                        }
                    }
                    break;
            }

            return (fileVersion, majorVersion, minorVersion, buildVersion);
        }

        public static void ParseVersionStr(string version, ref int major, ref int minor, ref int build)
        {
            if (string.IsNullOrEmpty(version))
                return;

            var splits = version.Split('.');
            if (splits.Length > 0)
                int.TryParse(splits[0], out major);
            if (splits.Length > 1)
                int.TryParse(splits[1], out minor);
            if (splits.Length > 2)
                int.TryParse(splits[2], out build);
        }

        public static async Task<bool> MacOSRemoveQuarantine(string appPath)
        {
            try
            {
                // xattr -> /usr/bin/xattr
                // https://github.com/tylearymf/UniHacker/issues/173
                var attrStartInfo = new ProcessStartInfo("/usr/bin/xattr", $"-rds com.apple.quarantine \"{appPath}\"");
                var attrProcess = Process.Start(attrStartInfo);
                await attrProcess!.WaitForExitAsync();
                return attrProcess.ExitCode == 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string TryGetVersionOfUnity(string filePath)
        {
            var architectureType = MachineArchitecture.GetArchitectureType(filePath);
            switch (GetPlatformTypeByArch(architectureType))
            {
                case PlatformType.Windows:
                    var info = FileVersionInfo.GetVersionInfo(filePath);
                    var version = info?.ProductVersion;
                    if (!string.IsNullOrEmpty(version))
                        return version;
                    break;
            }

            var maxLength = 40;
            var fileBytes = File.ReadAllBytes(filePath);

            var regex1 = new Regex(@"\d+\.\d\.\d+[fb]\d_\w+", RegexOptions.Compiled | RegexOptions.Singleline);
            var regex2 = new Regex(@"\d+\.\d\.\d+[fb]\d\.git\.\w+", RegexOptions.Compiled | RegexOptions.Singleline);

            var counter = 0;
            var stringBytes = new List<byte>(maxLength);

            void Clear()
            {
                counter = 0;
                stringBytes.Clear();
            }

            for (int i = 0; i < fileBytes.Length; i++)
            {
                if (++counter >= maxLength)
                {
                    Clear();
                    continue;
                }

                stringBytes.Add(fileBytes[i]);
                if (fileBytes[i] == 0 && stringBytes.Count > 1)
                {
                    stringBytes.RemoveAt(stringBytes.Count - 1);
                    var versionName = Encoding.UTF8.GetString(stringBytes.ToArray());

                    if (regex1.IsMatch(versionName) || regex2.IsMatch(versionName))
                        return versionName;

                    Clear();
                }
            }

            return string.Empty;
        }

        public static async Task<string> GetLinuxUserName()
        {
            try
            {
                var startInfo = new ProcessStartInfo("/bin/bash", "-c \"getent passwd | grep \"/home\" | grep -v \"nologin\"\"")
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };
                var process = Process.Start(startInfo);
                await process!.WaitForExitAsync();
                if (process.ExitCode != 0)
                {
                    throw new Exception($"error get linux user name. errorCode:{process.ExitCode}. errorMsg:{process.StandardError.ReadToEnd()}");
                }
                else
                {
                    var output = await process.StandardOutput.ReadToEndAsync();
                    var names = output.Split(':');
                    var userName = names[0];

                    startInfo = new ProcessStartInfo("/bin/bash", $"-c id \"{userName}\"")
                    {
                        RedirectStandardError = true
                    };
                    process = Process.Start(startInfo);
                    await process!.WaitForExitAsync();
                    if (process.ExitCode != 0)
                    {
                        throw new Exception($"error linux user name. errorCode:{process.ExitCode}. errorMsg:{process.StandardError.ReadToEnd()}");
                    }
                    else
                    {
                        return userName;
                    }
                }
            }
            catch (Exception ex)
            {
                await MessageBox.Show(ex.ToString());
            }

            return string.Empty;
        }

        // flag{1} + major{4} + minor{3} + build{3}
        // flag = none : 0 2021 003 019
        // flag = beta : 1 2021 003 019
        const int FlagWidth = 1;
        const int MajorWidth = 4;
        const int MinorWidth = 3;
        const int BuildWidth = 3;
        const int VersionWidth = MajorWidth + MinorWidth + BuildWidth;
        const int TotalVersionWidth = FlagWidth + VersionWidth;

        public static long ConvertToVersionID(string version, VersionFlag flag = VersionFlag.None)
        {
            var splits = version.Split('.').ToList();
            while (splits.Count > 3)
                splits.RemoveAt(splits.Count - 1);

            while (splits.Count < 3)
                splits.Add(string.Empty);

            var str = $"{splits[0].PadLeft(MajorWidth, '0')}{splits[1].PadLeft(MinorWidth, '0')}{splits[2].PadLeft(BuildWidth, '0')}";
            if (long.TryParse(str, out var id))
                id += (int)flag * (long)Math.Pow(10, TotalVersionWidth);

            return id;
        }

        public static bool IsMajorEquals(long v1, long v2)
        {
            var str1 = v1.ToString().PadLeft(TotalVersionWidth, '0');
            var str2 = v2.ToString().PadLeft(TotalVersionWidth, '0');

            str1 = str1.Substring(0, FlagWidth + MajorWidth);
            str2 = str2.Substring(0, FlagWidth + MajorWidth);
            return str1 == str2;
        }

        public static long GetMajorRevision(string version, VersionFlag flag = VersionFlag.None)
        {
            var splits = version.Split('.');
            return ConvertToVersionID(splits.Length > 0 ? splits[0] : string.Empty);
        }

        public static VersionFlag GetVersionFlag(string version)
        {
            if (Regex.IsMatch(version, @"^\d+(\.\d+)?(\.\d+)?b", RegexOptions.Multiline))
                return VersionFlag.Beta;

            return VersionFlag.None;
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
