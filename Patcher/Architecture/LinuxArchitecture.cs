using System.IO;

namespace UniHacker
{
    internal class LinuxArchitecture
    {
        const uint Header = 0x464C457F;

        public static ArchitectureType GetArchitectureType(string fileName)
        {
            try
            {
                using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                using var reader = new BinaryReader(fs);
                var header = reader.ReadUInt32();
                if (header != Header)
                    return ArchitectureType.UnKnown;

                var architecture = reader.ReadByte();
                if (architecture == 0x1)
                    return ArchitectureType.Linux_I386;
                else if (architecture == 0x2)
                    return ArchitectureType.Linux_X86_64;
            }
            catch { }

            return ArchitectureType.UnKnown;
        }
    }
}
