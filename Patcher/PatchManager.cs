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
                MessageBox.Show(Language.GetString("NotExist", filePath));
                return null;
            }

            var fileName = Path.GetFileName(filePath);
            if (fileName == "Unity" + PlatformUtils.GetExtension())
                return new UnityPatcher(filePath);
            else if (string.Compare(fileName.Replace(" ", ""), "UnityHub" + PlatformUtils.GetExtension(), true) == 0)
                return new UnityHubPatcher(filePath);
            else
                return new DefaultPatcher(filePath);
        }
    }
}
