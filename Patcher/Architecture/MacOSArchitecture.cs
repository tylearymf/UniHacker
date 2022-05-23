using System.IO;

namespace UniHacker
{
    internal class MacOSArchitecture
    {
        const uint Header = 0xFEEDFACF;

        //https://developer.apple.com/documentation/foundation/1495005-mach-o_architecture
        const uint I386 = 0x00000007;
        const uint X86_64 = 0x01000007;
        const uint ARM64 = 0x0100000C;

        public static ArchitectureType GetArchitectureType(string fileName)
        {
            try
            {
                using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                using var reader = new BinaryReader(fs);
                var header = reader.ReadUInt32();

                if (header != Header)
                    return ArchitectureType.UnKnown;

                var cpuType = reader.ReadUInt32();
                if (cpuType == I386)
                    return ArchitectureType.MacOS_I386;
                else if (cpuType == X86_64)
                    return ArchitectureType.MacOS_X86_64;
                else if (cpuType == ARM64)
                    return ArchitectureType.MacOS_ARM64;
            }
            catch { }

            return ArchitectureType.UnKnown;
        }
    }
}
