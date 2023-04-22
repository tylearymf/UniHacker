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

        public int BuildVersion { protected set; get; }

        public Version Version { protected set; get; }


        public Patcher(string filePath)
        {
            var fileName = Path.GetFileNameWithoutExtension(filePath);
            var (rootPath, realFilePath) = PlatformUtils.GetRealFilePath(filePath);

            FilePath = realFilePath;
            RootPath = rootPath;
            ArchitectureType = MachineArchitecture.GetArchitectureType(realFilePath);
            (FileVersion, MajorVersion, MinorVersion, BuildVersion) = PlatformUtils.GetFileVersionInfo(filePath, ArchitectureType);
            Version = new Version(MajorVersion, MinorVersion, BuildVersion);
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

        public virtual async Task<(bool success, string errorMsg)> RemovePatch(Action<double> progress)
        {
            await Task.Yield();
            throw new NotImplementedException();
        }
    }
}
