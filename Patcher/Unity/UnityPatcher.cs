
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace UniHacker
{
#pragma warning disable CS8618
    internal class UnityPatcher : Patcher
    {
        readonly byte[] fileBytes;
        readonly List<int> patchIndexes;
        readonly UnityPatchInfo? patchInfo;

        public UnityPatcher(string filePath) : base(filePath)
        {
            PatchStatus = PatchStatus.NotSupport;

            patchInfo = UnityPatchInfos.FindPatchInfo(FileVersion, ArchitectureType);
            if (patchInfo?.IsValid() ?? false)
            {
                fileBytes = File.ReadAllBytes(FilePath);

                var darkIndexes = BoyerMooreSearcher.FindPattern(patchInfo.DarkPattern, fileBytes);
                var lightIndexes = BoyerMooreSearcher.FindPattern(patchInfo.LightPattern, fileBytes);

                if (darkIndexes.Count == patchInfo.DarkPattern.Count)
                {
                    patchIndexes = darkIndexes;
                    PatchStatus = PatchStatus.Patched;
                }
                else if (lightIndexes.Count == patchInfo.LightPattern.Count)
                {
                    patchIndexes = lightIndexes;
                    PatchStatus = PatchStatus.Support;
                }

                if (PatchStatus == PatchStatus.NotSupport)
                {
                    var isSpecial = Regex.IsMatch(FileVersion, @"\d+\.\d+.\d+f\d+c\d+(.*)?") ||
                                    File.Exists(Path.Combine(RootPath, "hasp_rt.exe")) ||
                                    File.Exists(Path.Combine(RootPath, "hasp_update.exe")) ||
                                    File.Exists(Path.Combine(RootPath, "unity-sl.v2c")) ||
                                    File.ReadAllBytes(filePath)?.Length <= 50 * 1024 * 1024;
                    if (isSpecial)
                        PatchStatus = PatchStatus.Special;
                }
            }
        }

        public override async Task<(bool success, string errorMsg)> ApplyPatch(Action<double> progress)
        {
            if (patchInfo == null || patchIndexes?.Count == 0)
                return (false, string.Empty);

            // 创建备份
            var bakPath = FilePath + ".bak";
            if (File.Exists(bakPath))
                File.Delete(bakPath);
            await File.WriteAllBytesAsync(bakPath, fileBytes);

            // 修改 bak 权限
            //if (PlatformUtils.IsOSX() || PlatformUtils.IsLinux())
            //{
            //    _ = new UnixFileInfo(bakPath)
            //    {
            //        // rwxr-xr-x
            //        FileAccessPermissions = (FileAccessPermissions)0B111101101
            //    };
            //}

            // 写入文件流
            for (var i = 0; i < patchInfo.DarkPattern.Count; i++)
            {
                var bytes = patchInfo.DarkPattern[i];
                for (int j = 0; j < bytes.Length; j++)
                {
                    fileBytes[patchIndexes![i] + j] = bytes[j];
                }
            }

            using (var sw = File.OpenWrite(FilePath))
                sw.Write(fileBytes, 0, fileBytes.Length);

            var licensingFilePath = Path.Combine(RootPath, "Data/Resources/Licensing/Client/Unity.Licensing.Client" + PlatformUtils.GetExtension());
            if (File.Exists(licensingFilePath))
                File.Move(licensingFilePath, licensingFilePath + ".bak");

            //给 arm64 binary 自签名并放行
            if (PlatformUtils.IsOSX() && patchInfo.Architecture == ArchitectureType.MacOS_ARM64)
            {
                //自签名
                try
                {
                    var startInfo = new ProcessStartInfo("codesign", $"--force --deep --sign - {FilePath}")
                    {
                        RedirectStandardError = true
                    };
                    var process = Process.Start(startInfo);
                    await process!.WaitForExitAsync();
                    if (process.ExitCode != 0)
                    {
                        return (false, $"Codesign failed. {await process.StandardError.ReadToEndAsync()}");
                    }
                }
                catch (Win32Exception)
                {
                    return (false, "Cannot locate codesign");
                }
                //放行quarantine
                if (!await PlatformUtils.MacOSRemoveQuarantine(FilePath))
                {
                    return (false, "Set quarantine attribute failed.");
                }
            }

            // 创建许可证
            LicensingInfo.TryGenerate(MajorVersion, MinorVersion);

            return (true, string.Empty);
        }
    }
#pragma warning restore CS8618
}
