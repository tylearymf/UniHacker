using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using asardotnetasync;

namespace UniHacker
{
    internal class UnityHubPatcher : Patcher
    {
        public UnityHubPatcher(string filePath) : base(filePath)
        {
            PatchStatus = PatchStatus.NotSupport;

            var fileVersion = FileVersion;
            if (fileVersion.StartsWith("2."))
            {
                PatchStatus = PatchStatus.Support;
            }
            else if (fileVersion.StartsWith("3."))
            {
                PatchStatus = PatchStatus.Support;
            }
        }

        public async override Task<(bool success, string errorMsg)> ApplyPatch(Action<double> progress)
        {
            var unityHubPath = RootPath;
            unityHubPath = Path.Combine(unityHubPath, "resources");
            var exportFolder = Path.Combine(unityHubPath, "app");
            var asarPath = Path.Combine(unityHubPath, "app.asar");
            var asarBackupPath = Path.Combine(unityHubPath, "app.asar.bak");
            var asarUnpackPath = Path.Combine(unityHubPath, "app.asar.unpacked");

            if (Directory.Exists(exportFolder))
                Directory.Delete(exportFolder, true);
            Directory.CreateDirectory(exportFolder);

            if (!File.Exists(asarPath) && File.Exists(asarBackupPath))
                File.Move(asarBackupPath, asarPath);

            var archive = new AsarArchive(asarPath);
            var extractor = new AsarExtractor();

            extractor.FileExtracted += (sender, e) => progress(e.Progress);
            await extractor.ExtractAll(archive, exportFolder);
            await Task.Delay(200);

            // Newtonsoft 7.0以上的版本有BUG
            var jsonVersion = typeof(Newtonsoft.Json.JsonConvert).Assembly.GetName().Version?.Major;
            if (jsonVersion > 7)
            {
                if (File.Exists(Path.Combine(exportFolder, "filespackage.json")))
                    File.Move(Path.Combine(exportFolder, "filespackage.json"), Path.Combine(exportFolder, "package.json"));
            }

            var fileVersion = FileVersion;
            var patchResult = false;

            if (fileVersion.StartsWith("2."))
            {
                patchResult = UnityHubV2.Patch(exportFolder);
            }
            else if (fileVersion.StartsWith("3."))
            {
                patchResult = UnityHubV3.Patch(exportFolder);
            }

            var licensingFilePath = Path.Combine(RootPath, "Frameworks/LicensingClient/Unity.Licensing.Client" + PlatformUtils.GetExtension());
            if (File.Exists(licensingFilePath))
                File.Move(licensingFilePath, licensingFilePath + ".bak");

            if (patchResult)
            {
                if (Directory.Exists(asarUnpackPath))
                    CopyDirectory(asarUnpackPath, exportFolder, true);

                if (File.Exists(asarPath) && !File.Exists(asarBackupPath))
                    File.Move(asarPath, asarBackupPath);
                if (PlatformUtils.IsOSX())
                {
                    await PlatformUtils.MacOSRemoveQuarantine(Directory.GetParent(RootPath)!.FullName);
                }
            }
            else
            {
                Directory.Delete(exportFolder, true);
            }

            return (patchResult, string.Empty);
        }


        public static void ReplaceMethod(ref string scriptContent, string methodIdentifier, string newMethodContent)
        {
            scriptContent = Regex.Replace(scriptContent, methodIdentifier + @"(?=\{)(?:(?<open>\{)|(?<-open>\})|[^\{\}])+?(?(open)(?!))", evaluator =>
            {
                return newMethodContent;
            }, RegexOptions.Singleline);
        }

        public static void ReplaceMehthodBody(ref string scriptContent, string body, string regex)
        {
            scriptContent = Regex.Replace(scriptContent, regex, evaluator =>
            {
                return evaluator.Value.Replace(evaluator.Groups["body"].Value, "\n" + body + "\n");
            }, RegexOptions.Singleline);
        }

        public static bool CopyDirectory(string sourcePath, string destinationPath, bool overwrite)
        {
            var ret = true;
            try
            {
                sourcePath = sourcePath.EndsWith(@"\") ? sourcePath : sourcePath + @"\";
                destinationPath = destinationPath.EndsWith(@"\") ? destinationPath : destinationPath + @"\";

                if (Directory.Exists(sourcePath))
                {
                    if (!Directory.Exists(destinationPath))
                        Directory.CreateDirectory(destinationPath);

                    foreach (var filePath in Directory.GetFiles(sourcePath))
                    {
                        var file = new FileInfo(filePath);
                        file.CopyTo(destinationPath + file.Name, overwrite);
                    }
                    foreach (var directoryPath in Directory.GetDirectories(sourcePath))
                    {
                        var directory = new DirectoryInfo(directoryPath);
                        if (!CopyDirectory(directoryPath, destinationPath + directory.Name, overwrite))
                            ret = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ret = false;
                Console.WriteLine(ex);
            }

            return ret;
        }
    }
}
