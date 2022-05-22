using System.IO;

namespace UniHacker
{
    internal class WindowsArchitecture
    {
        const uint X86_64 = 0x8664;
        const uint I386 = 0x014C;

        public static ArchitectureType GetArchitectureType(string fileName)
        {
            try
            {
                using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                using var reader = new BinaryReader(fs);
                // 从开头跳到PE头的长度计算位置
                fs.Seek(60, SeekOrigin.Begin);

                // 从开头跳到PE头
                var offset = reader.ReadInt32();
                fs.Seek(offset, SeekOrigin.Begin);

                var peHead = reader.ReadUInt32();
                if (peHead != 0x4550)
                    return ArchitectureType.UnKnown;

                var value = reader.ReadUInt16();
                if (value == X86_64)
                    return ArchitectureType.Windows_X86_64;
                else if (value == I386)
                    return ArchitectureType.Windows_I386;
            }
            catch { }

            return ArchitectureType.UnKnown;
        }
    }
}
