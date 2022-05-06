using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            FilePath = filePath;
            RootPath = Path.GetDirectoryName(filePath);
            ArchitectureType = MachineArchitecture.GetArchitectureType(filePath);

            var info = FileVersionInfo.GetVersionInfo(filePath);
            FileVersion = !string.IsNullOrEmpty(info.ProductVersion) ? info.ProductVersion.Split('_')[0] : Language.GetString(PatchStatus.Unknown.ToString());
            MajorVersion = info.ProductMajorPart;
            MinorVersion = info.ProductMinorPart;

            PatchStatus = PatchStatus.Unknown;

            if (!(this is DefaultPatcher))
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var processes = Process.GetProcessesByName(fileName);
                if (processes.Length > 0)
                    MessageBox.Show(Language.GetString("process_occupy", fileName));
            }
        }

        public virtual async Task<(bool success, string errorMsg)> ApplyPatch(Action<double> progress)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<(bool success, string errorMsg)> RemovePatch()
        {
            throw new NotImplementedException();
        }
    }
}
