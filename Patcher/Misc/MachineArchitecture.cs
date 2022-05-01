using System.IO;

namespace UniHacker
{
    public enum ArchitectureType
    {
        UnKnown = 0,
        AMD64 = 0x8664,
        I386 = 0x14C,
    }

    internal class MachineArchitecture
    {
        public static ArchitectureType GetArchitectureType(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (var reader = new BinaryReader(fs))
            {
                // 从开头跳到PE头的长度计算位置
                fs.Seek(60, SeekOrigin.Begin);

                // 从开头跳到PE头
                var offset = reader.ReadInt32();
                fs.Seek(offset, SeekOrigin.Begin);

                var peHead = reader.ReadUInt32();
                return peHead != 0x4550 ? ArchitectureType.UnKnown : (ArchitectureType)reader.ReadUInt16();
            }
        }
    }
}
