using System;
using System.Collections.Generic;
using System.Linq;

namespace UniHacker
{
    internal class UnityPatchInfos
    {
        public static byte[] ToArray(params byte[] bytes)
        {
            return bytes;
        }

        public static byte[] ToArray(string byteStr)
        {
            if (string.IsNullOrEmpty(byteStr))
                return new byte[0];

            return byteStr.Split(' ').ToList().ConvertAll(x => Convert.ToByte(x, 16)).ToArray();
        }

        public static List<byte[]> ToBytes(params byte[] bytes)
        {
            return new List<byte[]>() { bytes };
        }

        public static List<byte[]> ToBytes(byte[] bytes1, byte[] bytes2)
        {
            return new List<byte[]>() { bytes1, bytes2 };
        }

        static readonly List<UnityPatchInfo> WindowsPatches = new List<UnityPatchInfo>
        {
            new UnityPatchInfo
            {
                // 3.x的跟其他版本的破解流程不一样，所以就不放进来了
                Version = "3.",
            },
            new UnityPatchInfo
            {
                Version = "4.",
                Architecture = ArchitectureType.I386,
                LightPattern = ToBytes(0x55, 0x8B, 0xEC, 0x83, 0xEC, 0x08, 0x53, 0x56, 0x8B, 0xF1, 0x80, 0x7E, 0x04),
                DarkPattern = ToBytes(0xB0, 0x01, 0xC3, 0x83, 0xEC, 0x08, 0x53, 0x56, 0x8B, 0xF1, 0x80, 0x7E, 0x04),
            },
            new UnityPatchInfo
            {
                Version = "5.",
                Architecture = ArchitectureType.I386,
                LightPattern = ToBytes(0x55, 0x8B, 0xEC, 0x83, 0xEC, 0x08, 0x53, 0x56, 0x8B, 0xF1, 0x80, 0x7E),
                DarkPattern = ToBytes(0xB0, 0x01, 0xC3, 0x83, 0xEC, 0x08, 0x53, 0x56, 0x8B, 0xF1, 0x80, 0x7E),
            },
            new UnityPatchInfo
            {
                Version = "5.",
                LightPattern = ToBytes(0x40, 0x57, 0x48, 0x83, 0xEC, 0x30, 0x80, 0x79, 0x08),
                DarkPattern = ToBytes(0xB0, 0x01, 0xC3, 0x90, 0x90, 0x90, 0x80, 0x79, 0x08),
            },
            new UnityPatchInfo
            {
                Version = "2017.1",
                LightPattern = ToBytes(0x40, 0x57, 0x48, 0x83, 0xEC, 0x30, 0x80, 0x79, 0x08),
                DarkPattern = ToBytes(0xB0, 0x01, 0xC3, 0x90, 0x90, 0x90, 0x80, 0x79, 0x08),
            },
            new UnityPatchInfo
            {
                Version = "2017.2",
                LightPattern = ToBytes(0x88, 0x91, 0xD9, 0x03, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x48, 0x89, 0x5C, 0x24, 0x10),
                DarkPattern = ToBytes(0x88, 0x91, 0xD9, 0x03, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xB0, 0x01, 0xC3, 0x90, 0x90),
            },
            new UnityPatchInfo
            {
                Version = "2017.3",
                LightPattern = ToBytes(0x88, 0x91, 0x01, 0x04, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x48, 0x89, 0x5C, 0x24, 0x10),
                DarkPattern = ToBytes(0x88, 0x91, 0x01, 0x04, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xB0, 0x01, 0xC3, 0x90, 0x90),
            },
            new UnityPatchInfo
            {
                Version = "2017.4",
                LightPattern = ToBytes(0x88, 0x91, 0x29, 0x04, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x48, 0x89, 0x5C, 0x24, 0x10),
                DarkPattern = ToBytes(0x88, 0x91, 0x29, 0x04, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xB0, 0x01, 0xC3, 0x90, 0x90),
            },
            new UnityPatchInfo
            {
                Version = "2018.1",
                LightPattern = ToBytes(0x74, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x6E),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x6E),
            },
            new UnityPatchInfo
            {
                Version = "2018.2",
                LightPattern = ToBytes(0x74, 0x0A, 0xBB, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x3E),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xBB, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x3E),
            },
            new UnityPatchInfo
            {
                Version = "2018.3",
                LightPattern = ToBytes(0x74, 0x0A, 0xBB, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x9B),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xBB, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x9B),
            },
            new UnityPatchInfo
            {
                Version = "2018.4",
                LightPattern = ToBytes(0x74, 0x0A, 0xBB, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x9B),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xBB, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x9B),
            },
            new UnityPatchInfo
            {
                Version = "2019.1",
                LightPattern = ToBytes(0x74, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x34),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x34),
            },
            new UnityPatchInfo
            {
                Version = "2019.2",
                LightPattern = ToBytes(0x74, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x34),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x34),
            },
            new UnityPatchInfo
            {
                Version = "2019.3",
                LightPattern = ToBytes(0x75, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xF3),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xF3),
            },
            new UnityPatchInfo
            {
                Version = "2019.4",
                LightPattern = ToBytes(0x75, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xF3),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xF3),
            },
            new UnityPatchInfo
            {
                Version = "2020.1",
                LightPattern = ToBytes(0x75, 0x11, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x38),
                DarkPattern = ToBytes(0xEB, 0x11, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x38),
            },
            new UnityPatchInfo
            {
                Version = "2020.2",
                LightPattern = ToBytes(0x75, 0x16, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xEA),
                DarkPattern = ToBytes(0xEB, 0x16, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xEA),
            },
            new UnityPatchInfo
            {
                Version = "2020.3",
                LightPattern = ToBytes(0x75, 0x16, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xEA),
                DarkPattern = ToBytes(0xEB, 0x16, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xEA),
            },
            new UnityPatchInfo
            {
                Version = "2021.1",
                LightPattern = ToBytes(0x75, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x33),
                DarkPattern = ToBytes(0xEB, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x33),
            },
            new UnityPatchInfo
            {
                Version = "2021.2",
                LightPattern = ToBytes(0x75, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x66),
                DarkPattern = ToBytes(0xEB, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x66),
            },
            new UnityPatchInfo
            {
                Version = "2021.3",
                LightPattern = ToBytes(ToArray("74 19 48 8D 0D C0 82 3F 01"), ToArray("75 14 B8 02 00 00 00 E9 66")),
                DarkPattern = ToBytes(ToArray("EB 19 48 8D 0D C0 82 3F 01"), ToArray("74 14 B8 02 00 00 00 E9 66")),
            },
            new UnityPatchInfo
            {
                Version = "2022.1.0b",
                LightPattern = ToBytes(ToArray("75 14 B8 02 00 00 00 E9 66 04 00"), ToArray("0F 84 A4 00 00 00 C7 44 24 20 25 00")),
                DarkPattern = ToBytes(ToArray("74 14 B8 02 00 00 00 E9 66 04 00"), ToArray("E9 A5 00 00 00 00 C7 44 24 20 25 00")),
            }
        };

        static UnityPatchInfos()
        {
            var patchInfos = WindowsPatches;
            patchInfos.AddRange(new MultiVersionPatchInfo()
            {
                Versions = new List<string> { "2021.3.0f1", "2021.3.1f1" },
                LightPattern = ToBytes(0x75, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x66),
                DarkPattern = ToBytes(0xEB, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x66),
            }.ToArray());
        }


        public static UnityPatchInfo FindPatchInfo(string version, ArchitectureType architectureType)
        {
            var pathInfos = GetPatchInfos();
            var infos = pathInfos.FindAll(x => version.StartsWith(x.Version) && x.Architecture == architectureType);
            return infos.OrderByDescending(x => x.Version).FirstOrDefault();
        }

        public static List<UnityPatchInfo> GetPatchInfos()
        {
            return WindowsPatches;
        }
    }

    internal class UnityPatchInfo
    {
        public string Version { get; set; }

        public ArchitectureType Architecture { get; set; } = ArchitectureType.AMD64;

        public List<byte[]> DarkPattern { get; set; }

        public List<byte[]> LightPattern { get; set; }


        public bool IsValid()
        {
            return Architecture != ArchitectureType.UnKnown && DarkPattern != null && LightPattern != null;
        }
    }

    internal class MultiVersionPatchInfo
    {
        public List<string> Versions { set; get; }

        public ArchitectureType Architecture { get; set; } = ArchitectureType.AMD64;

        public List<byte[]> DarkPattern { get; set; }

        public List<byte[]> LightPattern { get; set; }


        public List<UnityPatchInfo> ToArray()
        {
            var infos = new List<UnityPatchInfo>(Versions.Count);
            foreach (var ver in Versions)
            {
                infos.Add(new UnityPatchInfo()
                {
                    Version = ver,
                    Architecture = Architecture,
                    LightPattern = LightPattern,
                    DarkPattern = DarkPattern,
                });
            }

            return infos;
        }
    }
}
