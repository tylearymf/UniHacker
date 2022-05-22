
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Mono.Unix;

namespace UniHacker
{
#pragma warning disable CS8618
    internal class UnityPatcher : Patcher
    {
        readonly byte[] fileBytes;
        readonly List<int> patchIndexes;
        readonly UnityPatchInfo patchInfo;

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
                    if (Regex.IsMatch(FileVersion, @"\d+\.\d+.\d+f\d+c\d+(.*)?"))
                        PatchStatus = PatchStatus.Special;
                }
            }
        }

        public override async Task<(bool success, string errorMsg)> ApplyPatch(Action<double> progress)
        {
            if (patchInfo != null && patchIndexes?.Count > 0)
            {
                // 创建备份
                var bakPath = FilePath + ".bak";
                if (File.Exists(bakPath))
                    File.Delete(bakPath);
                await File.WriteAllBytesAsync(bakPath, fileBytes);

                if (PlatformUtils.IsOSX())
                {
                    // chmod +x
                    _ = new UnixFileInfo(bakPath)
                    {
                        FileAccessPermissions = FileAccessPermissions.UserExecute |
                                                         FileAccessPermissions.OtherExecute |
                                                         FileAccessPermissions.GroupExecute
                    };
                }
                else if (PlatformUtils.IsLinux())
                {
                    _ = new UnixFileInfo(bakPath)
                    {
                        FileAccessPermissions = FileAccessPermissions.UserReadWriteExecute |
                                                         FileAccessPermissions.OtherRead |
                                                         FileAccessPermissions.OtherExecute |
                                                         FileAccessPermissions.OtherRead |
                                                         FileAccessPermissions.OtherExecute
                    };
                }

                // 写入文件流
                for (var i = 0; i < patchInfo.DarkPattern.Count; i++)
                {
                    var bytes = patchInfo.DarkPattern[i];
                    for (int j = 0; j < bytes.Length; j++)
                    {
                        fileBytes[patchIndexes[i] + j] = bytes[j];
                    }
                }

                using (var sw = File.OpenWrite(FilePath))
                    sw.Write(fileBytes, 0, fileBytes.Length);

                if (PlatformUtils.IsWindows())
                {
                    var licensingFilePath = Path.Combine(RootPath, @"Data\Resources\Licensing\Client\Unity.Licensing.Client" + PlatformUtils.GetExtension());
                    if (File.Exists(licensingFilePath))
                        File.Move(licensingFilePath, licensingFilePath + ".bak");
                }

                // 创建许可证
                LicensingInfo.TryGenerate(MajorVersion, MinorVersion);

                return (true, string.Empty);
            }

            return (false, string.Empty);
        }
    }
#pragma warning restore CS8618
}
