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
            PatchStatus = MajorVersion >= 2 ? PatchStatus.Support : PatchStatus.NotSupport;

            var unityHubPath = RootPath;
            unityHubPath = Path.Combine(unityHubPath, "resources");
            var exportFolder = Path.Combine(unityHubPath, "app");
            var asarBackupPath = Path.Combine(unityHubPath, "app.asar.bak");
            if (Directory.Exists(exportFolder) || File.Exists(asarBackupPath))
                PatchStatus = PatchStatus.Patched;
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

            var patchResult = false;
            if (Version >= new Version("3.4.2"))
            {
                patchResult = await UnityHubV3_4_2.Patch(exportFolder);
            }
            else if (MajorVersion == 3)
            {
                patchResult = await UnityHubV3.Patch(exportFolder);
            }
            else if (MajorVersion == 2)
            {
                patchResult = UnityHubV2.Patch(exportFolder);
            }

            var licensingFilePath = Path.Combine(RootPath, "Frameworks/LicensingClient/Unity.Licensing.Client" + PlatformUtils.GetExtension());
            if (File.Exists(licensingFilePath))
                File.Move(licensingFilePath, licensingFilePath + ".bak");

            if (patchResult)
            {
                if (Directory.Exists(asarUnpackPath))
                {
                    var result = CopyDirectory(asarUnpackPath, exportFolder, true);
                    if (!result)
                        await MessageBox.Show(Language.GetString("Hub_copy_error1", asarUnpackPath, exportFolder));
                }

                if (File.Exists(asarPath) && !File.Exists(asarBackupPath))
                    File.Move(asarPath, asarBackupPath);

                if (PlatformUtils.IsOSX())
                    await PlatformUtils.MacOSRemoveQuarantine(Directory.GetParent(RootPath)!.FullName);
            }
            else
            {
                Directory.Delete(exportFolder, true);
            }

            return (patchResult, string.Empty);
        }

        public async override Task<(bool success, string errorMsg)> RemovePatch(Action<double> progress)
        {
            var unityHubPath = RootPath;
            unityHubPath = Path.Combine(unityHubPath, "resources");
            var exportFolder = Path.Combine(unityHubPath, "app");
            var asarPath = Path.Combine(unityHubPath, "app.asar");
            var asarBackupPath = Path.Combine(unityHubPath, "app.asar.bak");

            progress(0.2F);
            await Task.Delay(200);

            if (Directory.Exists(exportFolder))
                Directory.Delete(exportFolder, true);

            progress(0.7F);
            await Task.Delay(200);

            if (!File.Exists(asarPath) && File.Exists(asarBackupPath))
                File.Move(asarBackupPath, asarPath);

            progress(1F);
            await Task.Delay(200);

            return (true, string.Empty);
        }

        public static void ReplaceMethodBody(ref string scriptContent, string methodName, string newMethodContent)
        {
            scriptContent = Regex.Replace(scriptContent, @"(?<header>" + methodName + @"(?=\()(?:(?<open>\()|(?<-open>\))|[^\(\)])+?(?(open)(?!))\s*)(?=\{)(?:(?<open>\{)|(?<-open>\})|[^\{\}])+?(?(open)(?!))", evaluator =>
            {
                return $"{evaluator.Groups["header"].Value} {{ {newMethodContent} \t}}";
            }, RegexOptions.Singleline);
        }

        public static void ReplaceFullMethod(ref string scriptContent, string methodIdentifier, string newMethodContent)
        {
            scriptContent = Regex.Replace(scriptContent, methodIdentifier + @"(?=\{)(?:(?<open>\{)|(?<-open>\})|[^\{\}])+?(?(open)(?!))", evaluator =>
            {
                return newMethodContent;
            }, RegexOptions.Singleline);
        }

        public static bool CopyDirectory(string sourcePath, string destinationPath, bool overwrite)
        {
            var ret = true;
            try
            {
                if (Directory.Exists(sourcePath))
                {
                    if (!Directory.Exists(destinationPath))
                        Directory.CreateDirectory(destinationPath);

                    foreach (var filePath in Directory.GetFiles(sourcePath))
                    {
                        var file = new FileInfo(filePath);
                        file.CopyTo(Path.Combine(destinationPath, file.Name), overwrite);
                    }

                    foreach (var directoryPath in Directory.GetDirectories(sourcePath))
                    {
                        var directory = new DirectoryInfo(directoryPath);
                        if (!CopyDirectory(directoryPath, Path.Combine(destinationPath, directory.Name), overwrite))
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
