using System.IO;

namespace UniHacker
{
    internal class PatcherManager
    {
        public static Patcher GetPatcher(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            switch (fileName)
            {
                case "Unity.exe":
                    return new UnityPatcher(filePath);
                case "Unity Hub.exe":
                    return new UnityHubPatcher(filePath);
            }

            return new DefaultPatcher(filePath);
        }
    }
}
