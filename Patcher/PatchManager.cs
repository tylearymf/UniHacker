using System;
using System.IO;

namespace UniHacker
{
    internal class PatchManager
    {
        public static Patcher? GetPatcher(string filePath, PlatformType platformType)
        {
            Func<string, bool> existAction = platformType == PlatformType.MacOS ? Directory.Exists : File.Exists;
            if (!existAction(filePath))
            {
                var message = string.Empty;
#if DOCKER_ENV
                message = $"The file does not exist. '{filePath}'";
#else
                message = Language.GetString("NotExist", filePath);
#endif
                MessageBox.Show(message);
                return null;
            }

            var fileName = Path.GetFileName(filePath);
            if (fileName == "Unity" + PlatformUtils.GetExtension())
                return new UnityPatcher(filePath);
            else if (fileName.Replace(" ", "").Contains("unityhub", StringComparison.OrdinalIgnoreCase))
                return new UnityHubPatcher(filePath);
            else
                return new DefaultPatcher(filePath);
        }
    }
}
