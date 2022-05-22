using System;
using System.IO;

namespace UniHacker
{
    internal class PatchManager
    {
        public static Patcher? GetPatcher(string filePath)
        {
            Func<string, bool> existAction = PlatformUtils.GetPlatformType() == PlatformType.MacOS ? Directory.Exists : File.Exists;
            if (!existAction(filePath))
            {
                MessageBox.Show(Language.GetString("NotExist", filePath));
                return null;
            }

            var fileName = Path.GetFileName(filePath);
            if (fileName == "Unity" + PlatformUtils.GetExtension())
                return new UnityPatcher(filePath);
            else if (fileName == "Unity Hub" + PlatformUtils.GetExtension())
                return new UnityHubPatcher(filePath);
            else
                return new DefaultPatcher(filePath);
        }
    }
}
