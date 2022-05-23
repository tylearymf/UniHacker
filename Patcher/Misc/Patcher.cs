using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace UniHacker
{
    internal abstract class Patcher
    {
        public string RootPath { protected set; get; }

        public string FilePath { protected set; get; }

        public string FileVersion { protected set; get; }

        public ArchitectureType ArchitectureType { protected set; get; }

        public PatchStatus PatchStatus { protected set; get; }

        public int MajorVersion { protected set; get; }

        public int MinorVersion { protected set; get; }


        public Patcher(string filePath)
        {
            var sourceFilePath = filePath;
            var realFilePath = filePath;
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            RootPath = Path.GetDirectoryName(sourceFilePath) ?? string.Empty;

            if (PlatformUtils.GetPlatformType() == PlatformType.MacOS)
            {
                var rootPath = Path.Combine(filePath, "Contents");
                realFilePath = Path.Combine(rootPath, $"MacOS/{fileName}");
                RootPath = rootPath;
            }

            FilePath = realFilePath;
            ArchitectureType = MachineArchitecture.GetArchitectureType(realFilePath);
            (FileVersion, MajorVersion, MinorVersion) = PlatformUtils.GetFileVersionInfo(filePath, ArchitectureType);
            PatchStatus = PatchStatus.Unknown;
            if (this is not DefaultPatcher)
            {
                var processes = Process.GetProcessesByName(fileName);
                if (processes.Length > 0)
                    MessageBox.Show(Language.GetString("Process_occupy", fileName));
            }
        }

        public virtual async Task<(bool success, string errorMsg)> ApplyPatch(Action<double> progress)
        {
            await Task.Yield();
            throw new NotImplementedException();
        }

        public virtual async Task<(bool success, string errorMsg)> RemovePatch()
        {
            await Task.Yield();
            throw new NotImplementedException();
        }
    }
}
