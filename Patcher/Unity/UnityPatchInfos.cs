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
                return Array.Empty<byte>();

            return byteStr.Split(' ').ToList().ConvertAll(x => Convert.ToByte(x, 16)).ToArray();
        }

        public static List<byte[]> ToBytes(string byteStr)
        {
            return ToBytes(ToArray(byteStr));
        }

        public static List<byte[]> ToBytes(params byte[] bytes)
        {
            return new List<byte[]>() { bytes };
        }

        public static List<byte[]> ToBytes(params byte[][] bytesArray)
        {
            var bytesList = new List<byte[]>();
            foreach (var item in bytesArray)
                bytesList.Add(item);

            return bytesList;
        }

        static readonly List<UnityPatchInfo> WindowsPatches = new()
        {
            new()
            {
                // 3.x的跟其他版本的破解流程不一样，所以就不放进来了
                Version = "3.",
            },
            new()
            {
                Version = "4.",
                Architecture = ArchitectureType.Windows_I386,
                LightPattern = ToBytes(0x55, 0x8B, 0xEC, 0x83, 0xEC, 0x08, 0x53, 0x56, 0x8B, 0xF1, 0x80, 0x7E, 0x04),
                DarkPattern = ToBytes(0xB0, 0x01, 0xC3, 0x83, 0xEC, 0x08, 0x53, 0x56, 0x8B, 0xF1, 0x80, 0x7E, 0x04),
            },
            new()
            {
                Version = "5.",
                Architecture = ArchitectureType.Windows_I386,
                LightPattern = ToBytes(0x55, 0x8B, 0xEC, 0x83, 0xEC, 0x08, 0x53, 0x56, 0x8B, 0xF1, 0x80, 0x7E),
                DarkPattern = ToBytes(0xB0, 0x01, 0xC3, 0x83, 0xEC, 0x08, 0x53, 0x56, 0x8B, 0xF1, 0x80, 0x7E),
            },
            new()
            {
                Version = "5.",
                LightPattern = ToBytes(0x40, 0x57, 0x48, 0x83, 0xEC, 0x30, 0x80, 0x79, 0x08),
                DarkPattern = ToBytes(0xB0, 0x01, 0xC3, 0x90, 0x90, 0x90, 0x80, 0x79, 0x08),
            },
            new()
            {
                Version = "2017.1",
                LightPattern = ToBytes(0x40, 0x57, 0x48, 0x83, 0xEC, 0x30, 0x80, 0x79, 0x08),
                DarkPattern = ToBytes(0xB0, 0x01, 0xC3, 0x90, 0x90, 0x90, 0x80, 0x79, 0x08),
            },
            new()
            {
                Version = "2017.2",
                LightPattern = ToBytes(0x88, 0x91, 0xD9, 0x03, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x48, 0x89, 0x5C, 0x24, 0x10),
                DarkPattern = ToBytes(0x88, 0x91, 0xD9, 0x03, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xB0, 0x01, 0xC3, 0x90, 0x90),
            },
            new()
            {
                Version = "2017.3",
                LightPattern = ToBytes(0x88, 0x91, 0x01, 0x04, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x48, 0x89, 0x5C, 0x24, 0x10),
                DarkPattern = ToBytes(0x88, 0x91, 0x01, 0x04, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xB0, 0x01, 0xC3, 0x90, 0x90),
            },
            new()
            {
                Version = "2017.4",
                LightPattern = ToBytes(0x88, 0x91, 0x29, 0x04, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x48, 0x89, 0x5C, 0x24, 0x10),
                DarkPattern = ToBytes(0x88, 0x91, 0x29, 0x04, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xB0, 0x01, 0xC3, 0x90, 0x90),
            },
            new()
            {
                Version = "2018.1",
                LightPattern = ToBytes(0x74, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x6E),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x6E),
            },
            new()
            {
                Version = "2018.2",
                LightPattern = ToBytes(0x74, 0x0A, 0xBB, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x3E),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xBB, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x3E),
            },
            new()
            {
                Version = "2018.3",
                LightPattern = ToBytes(0x74, 0x0A, 0xBB, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x9B),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xBB, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x9B),
            },
            new()
            {
                Version = "2018.4",
                LightPattern = ToBytes(0x74, 0x0A, 0xBB, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x9B),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xBB, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x9B),
            },
            new()
            {
                Version = "2019.1",
                LightPattern = ToBytes(0x74, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x34),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x34),
            },
            new()
            {
                Version = "2019.2",
                LightPattern = ToBytes(0x74, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x34),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x34),
            },
            new()
            {
                Version = "2019.3",
                LightPattern = ToBytes(0x75, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xF3),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xF3),
            },
            new()
            {
                Version = "2019.4",
                LightPattern = ToBytes(0x75, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xF3),
                DarkPattern = ToBytes(0xEB, 0x0A, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xF3),
            },
            new()
            {
                Version = "2020.1",
                LightPattern = ToBytes(0x75, 0x11, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x38),
                DarkPattern = ToBytes(0xEB, 0x11, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x38),
            },
            new()
            {
                Version = "2020.2",
                LightPattern = ToBytes(0x75, 0x16, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xEA),
                DarkPattern = ToBytes(0xEB, 0x16, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xEA),
            },
            new()
            {
                Version = "2020.3",
                LightPattern = ToBytes(0x75, 0x16, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xEA),
                DarkPattern = ToBytes(0xEB, 0x16, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0xEA),
            },
            new()
            {
                Version = "2020.3.34",
                LightPattern = ToBytes(ToArray("74 28 48 8D 0D 9D 50 88 01"), ToArray("75 16 B8 02 00 00 00 E9 EA")),
                DarkPattern = ToBytes(ToArray("EB 28 48 8D 0D 9D 50 88 01"), ToArray("EB 16 B8 02 00 00 00 E9 EA")),
            },
            new()
            {
                Version = "2021.1",
                LightPattern = ToBytes(0x75, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x33),
                DarkPattern = ToBytes(0xEB, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x33),
            },
            new()
            {
                Version = "2021.2",
                LightPattern = ToBytes(0x75, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x66),
                DarkPattern = ToBytes(0xEB, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x66),
            },
            new()
            {
                Version = "2021.3.2",
                LightPattern = ToBytes(ToArray("74 19 48 8D 0D C0 82 3F 01"), ToArray("75 14 B8 02 00 00 00 E9 66")),
                DarkPattern = ToBytes(ToArray("EB 19 48 8D 0D C0 82 3F 01"), ToArray("74 14 B8 02 00 00 00 E9 66")),
            },
            new()
            {
                Version = "2021.3.3",
                LightPattern = ToBytes(ToArray("0F 84 9C 00 00 00 C7 44 24 20 24"), ToArray("75 14 B8 02 00 00 00 E9 66")),
                DarkPattern = ToBytes(ToArray("E9 9D 00 00 00 00 C7 44 24 20 24"), ToArray("74 14 B8 02 00 00 00 E9 66")),
            },
            new()
            {
                // 2021.3.4
                Version = "2021.3",
                LightPattern = ToBytes(ToArray("E9 C5 00 00 00 48 8D 0D 67 1E 40"), ToArray("75 14 B8 02 00 00 00 E9 66")),
                DarkPattern = ToBytes(ToArray("EB 3C 00 00 00 48 8D 0D 67 1E 40"), ToArray("74 14 B8 02 00 00 00 E9 66")),
            },
            new()
            {
                Version = "2022.1.0b",
                LightPattern = ToBytes(ToArray("75 14 B8 02 00 00 00 E9 66 04 00"), ToArray("0F 84 A4 00 00 00 C7 44 24 20 25 00")),
                DarkPattern = ToBytes(ToArray("74 14 B8 02 00 00 00 E9 66 04 00"), ToArray("E9 A5 00 00 00 00 C7 44 24 20 25 00")),
            },
            new()
            {
                Version = "2022.1",
                LightPattern = ToBytes(ToArray("0F 84 A4 00 00 00 C7 44 24 20 25"), ToArray("75 14 B8 02 00 00 00 E9 66")),
                DarkPattern = ToBytes(ToArray("E9 A5 00 00 00 00 C7 44 24 20 25"), ToArray("74 14 B8 02 00 00 00 E9 66")),
            },
        };

        static readonly List<UnityPatchInfo> MacOSPatches = new()
        {
            // X86_X64
            new()
            {
                // 4.7.2
                Version = "4",
                LightPattern = ToBytes("84 5C 02 00 00 48 8D B5 B0 FE FF FF 4C 89 F7"),
                DarkPattern = ToBytes("85 5C 02 00 00 48 8D B5 B0 FE FF FF 4C 89 F7"),
            },
            new()
            {
                // 5.6.6 5.6.7
                Version = "5",
                LightPattern = ToBytes("84 CC 00 00 00 49 8B 37 E8 40 63 00 00 BB 0C"),
                DarkPattern = ToBytes("85 CC 00 00 00 49 8B 37 E8 40 63 00 00 BB 0C"),
            },
            new()
            {
                // 2017.4.39 2017.4.40
                Version = "2017",
                LightPattern = ToBytes("00 00 41 88 C6 48 8B BD 20 FE FF FF 48 85 FF 74"),
                DarkPattern = ToBytes("00 00 41 88 C6 48 8B BD 20 FE FF FF 48 85 FF 75"),
            },
            new()
            {
                // 2018.4.35 2018.4.36
                Version = "2018",
                LightPattern = ToBytes(ToArray("08 FF 80 B8 D9 4B 00 00 00 74 4A 48 8D 35"), ToArray("00 41 88 C4 48 8B BD 20 FE FF FF 48 85 FF 74 16")),
                DarkPattern = ToBytes(ToArray("08 FF 80 B8 D9 4B 00 00 00 EB 4A 48 8D 35"), ToArray("00 41 88 C4 48 8B BD 20 FE FF FF 48 85 FF 75 16")),
            },
            new()
            {
                // 2019.4.28
                Version = "2019",
                LightPattern = ToBytes(ToArray("6B FF 80 B8 69 4C 00 00 00 74 5D"), ToArray("BB 02 00 00 00 45 84 E4 0F 84 3E 02")),
                DarkPattern = ToBytes(ToArray("6B FF 80 B8 69 4C 00 00 00 EB 5D"), ToArray("BB 02 00 00 00 45 84 E4 0F 85 3E 02")),
            },
            new()
            {
                // 2019.4.39、2019.4.38、2019.4.37
                Version = "2019.4.3",
                LightPattern = ToBytes(ToArray("72 FF 80 B8 69 4C 00 00 00 74 5D"), ToArray("BB 02 00 00 00 45 84 FF 0F 84 2E")),
                DarkPattern = ToBytes(ToArray("72 FF 80 B8 69 4C 00 00 00 EB 5D"), ToArray("BB 02 00 00 00 45 84 FF 0F 85 2E")),
            },
            new()
            {
                // 2020.3.34 2020.3.35
                Version = "2020",
                LightPattern = ToBytes(ToArray("FF 80 B8 91 58 00 00 00 74 5D 48 8D 35"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 84 81")),
                DarkPattern = ToBytes(ToArray("FF 80 B8 91 58 00 00 00 EB 5D 48 8D 35"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 85 81")),
            },
            new()
            {
                // 2021.2.19
                Version = "2021",
                LightPattern = ToBytes(ToArray("44 8A B0 A1 64 00 00 45 84 F6 74 66"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 84 3E")),
                DarkPattern = ToBytes(ToArray("44 8A B0 A1 64 00 00 45 84 F6 EB 66"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 85 3E")),
            },
            new()
            {
                // 2022.1.2
                Version = "2022",
                LightPattern = ToBytes(ToArray("8A B0 A1 64 00 00 45 84 F6 0F 84 A6"), ToArray("2A 00 00 41 BF 02 00 00 00 84 C0 0F 84")),
                DarkPattern = ToBytes(ToArray("8A B0 A1 64 00 00 45 84 F6 E9 A7 00"), ToArray("2A 00 00 41 BF 02 00 00 00 84 C0 0F 85")),
            },

            // ARM64
            new()
            {
                // 2021.2.11(m1)
                Version = "2021.2.11",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 A1 CB 00 90 21 40 22 91 03"), ToArray("94 20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 A1 CB 00 90 21 40 22 91 03"), ToArray("94 20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B"))
            },
            new()
            {
                // 2021.3.1(m1)
                Version = "2021.3.1",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 21 CC 00 90 21 40 22 91 83"), ToArray("94 20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes( ToArray("17 00 00 14 21 CC 00 90 21 40 22 91 83"), ToArray("94 20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                // 2021.3.2(m1)
                Version = "2021.3.2",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 21 CC 00 B0 21 40 22 91 83"), ToArray("94 20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 21 CC 00 B0 21 40 22 91 83"), ToArray("94 1F 20 03 D5 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                // 2021.3.3(m1)
                Version = "2021.3.3",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 A1 CC 00 D0 21 40 23 91"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 A1 CC 00 D0 21 40 23 91"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                // 2021.3.4(m1)
                Version = "2021.3.4",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 A1 CC 00 F0 21 20 23 91"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 A1 CC 00 F0 21 20 23 91"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                // 2021.3.5(m1)
                Version = "2021.3.5",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 C1 CC 00 B0 21 A0 23 91"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 C1 CC 00 B0 21 A0 23 91"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                // 2022.1.2(m1)
                Version = "2022",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("14 05 00 34 21 CE 00 D0 21 00 37"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13")),
                DarkPattern = ToBytes(ToArray("28 00 00 14 21 CE 00 D0 21 00 37"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13")),
            },
        };

        static readonly List<UnityPatchInfo> LinuxPatches = new()
        {
            new()
            {
                Version = "2021.3.0",
                LightPattern = ToBytes(ToArray("74 67 48 8B 35 9C 82 23 01 48 8D 0D"), ToArray("84 36 01 00 00 48 8D B4 24 A8")),
                DarkPattern = ToBytes(ToArray("EB 67 48 8B 35 9C 82 23 01 48 8D 0D"), ToArray("85 36 01 00 00 48 8D B4 24 A8")),
            },
        };

        static UnityPatchInfos()
        {
            var patchInfos = WindowsPatches;
            patchInfos.AddRange(new MultiVersionPatchInfo()
            {
                Versions = new List<string> { "2021.3.0", "2021.3.1" },
                LightPattern = ToBytes(0x75, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x66),
                DarkPattern = ToBytes(0xEB, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x66),
            }.ToArray());


            // set default architecture
            foreach (var item in WindowsPatches)
                if (!item.Architecture.HasValue)
                    item.Architecture = ArchitectureType.Windows_X86_64;
            foreach (var item in MacOSPatches)
                if (!item.Architecture.HasValue)
                    item.Architecture = ArchitectureType.MacOS_X86_64;
            foreach (var item in LinuxPatches)
                if (!item.Architecture.HasValue)
                    item.Architecture = ArchitectureType.Linux_X86_64;
        }


        public static UnityPatchInfo? FindPatchInfo(string version, ArchitectureType architectureType)
        {
            var pathInfos = GetPatchInfos(architectureType);
            var infos = pathInfos?.FindAll(x => version.StartsWith(x.Version) && x.Architecture == architectureType);
            return infos?.OrderByDescending(x => x.Version.Length).FirstOrDefault();
        }

        public static List<UnityPatchInfo>? GetPatchInfos(ArchitectureType type)
        {
            return PlatformUtils.GetPlatformTypeByArch(type) switch
            {
                PlatformType.Windows => WindowsPatches,
                PlatformType.MacOS => MacOSPatches,
                PlatformType.Linux => LinuxPatches,
                _ => null,
            };
        }
    }

#pragma warning disable CS8618
    internal class UnityPatchInfo
    {
        public string Version { get; set; }

        public ArchitectureType? Architecture { get; set; }

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

        public ArchitectureType Architecture { get; set; } = ArchitectureType.Windows_X86_64;

        public List<byte[]> DarkPattern { get; set; }

        public List<byte[]> LightPattern { get; set; }


        public List<UnityPatchInfo> ToArray()
        {
            var infos = new List<UnityPatchInfo>(Versions.Count);
            foreach (var ver in Versions)
            {
                infos.Add(new()
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
#pragma warning restore CS8618
}
