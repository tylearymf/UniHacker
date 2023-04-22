using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace UniHacker
{
    internal class UnityPatchInfos
    {
        public static byte[] ToArray(params byte[] bytes)
        {
            return bytes;
        }

        public static byte?[] ToArray(string byteStr)
        {
            if (string.IsNullOrEmpty(byteStr))
                return Array.Empty<byte?>();

            return Regex.Split(byteStr, @"\s+").ToList().ConvertAll(x => x == "?" ? null : (byte?)Convert.ToByte(x, 16)).ToArray();
        }

        public static List<byte?[]> ToBytes(string byteStr)
        {
            return ToBytes(ToArray(byteStr));
        }

        public static List<byte?[]> ToBytes(params byte?[] bytes)
        {
            return new List<byte?[]>() { bytes };
        }

        public static List<byte?[]> ToBytes(params byte?[][] bytesArray)
        {
            var bytesList = new List<byte?[]>();
            foreach (var item in bytesArray)
                bytesList.Add(item);

            return bytesList;
        }

        static readonly List<UnityPatchInfo> WindowsPatches = new()
        {
            new()
            {
                // 3.x的跟其他版本的破解流程不一样，所以就不放进来了
                Version = "3.0",
            },
            new()
            {
                Version = "4.0",
                Architecture = ArchitectureType.Windows_I386,
                LightPattern = ToBytes(0x55, 0x8B, 0xEC, 0x83, 0xEC, 0x08, 0x53, 0x56, 0x8B, 0xF1, 0x80, 0x7E, 0x04),
                DarkPattern = ToBytes(0xB0, 0x01, 0xC3, 0x83, 0xEC, 0x08, 0x53, 0x56, 0x8B, 0xF1, 0x80, 0x7E, 0x04),
            },
            new()
            {
                Version = "5.0",
                Architecture = ArchitectureType.Windows_I386,
                LightPattern = ToBytes(0x55, 0x8B, 0xEC, 0x83, 0xEC, 0x08, 0x53, 0x56, 0x8B, 0xF1, 0x80, 0x7E),
                DarkPattern = ToBytes(0xB0, 0x01, 0xC3, 0x83, 0xEC, 0x08, 0x53, 0x56, 0x8B, 0xF1, 0x80, 0x7E),
            },
            new()
            {
                Version = "5.0",
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
                Version = "2017.2.0 ; 2017.2.1 ; 2017.2.2",
                LightPattern = ToBytes(0x88, 0x91, 0xD9, 0x03, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0x48, 0x89, 0x5C, 0x24, 0x10),
                DarkPattern = ToBytes(0x88, 0x91, 0xD9, 0x03, 0x00, 0x00, 0xC3, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xCC, 0xB0, 0x01, 0xC3, 0x90, 0x90),
            },
            new()
            {
                Version = "2017.2.3 ; 2017.2.4 ; 2017.2.5 ; 2017.3",
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
                Version = "2020.3.36",
                LightPattern = ToBytes(ToArray("0F 84 AB 00 00 00 C7 44 24 20"), ToArray("75 16 B8 02 00 00 00 E9 EA")),
                DarkPattern = ToBytes(ToArray("E9 AC 00 00 00 00 C7 44 24 20"), ToArray("EB 16 B8 02 00 00 00 E9 EA")),
            },
            new()
            {
                Version = "2020.3.45",
                LightPattern = ToBytes(ToArray("0F 84 A4 00 00 00 C7 44 24 20 22"), ToArray("75 16 B8 02 00 00 00 E9 EA")),
                DarkPattern = ToBytes(ToArray("0F 85 A4 00 00 00 C7 44 24 20 22"), ToArray("74 16 B8 02 00 00 00 E9 EA")),
            },
            new()
            {
                Version = "2021.1",
                LightPattern = ToBytes(0x75, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x33),
                DarkPattern = ToBytes(0xEB, 0x14, 0xB8, 0x02, 0x00, 0x00, 0x00, 0xE9, 0x33),
            },
            new()
            {
                Version = "2021.2 ; 2021.3.0 ; 2021.3.1",
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
                Version = "2021.3.4",
                LightPattern = ToBytes(ToArray("E9 C5 00 00 00 48 8D 0D 67 1E 40"), ToArray("75 14 B8 02 00 00 00 E9 66")),
                DarkPattern = ToBytes(ToArray("EB 3C 00 00 00 48 8D 0D 67 1E 40"), ToArray("74 14 B8 02 00 00 00 E9 66")),
            },
            new()
            {
                Version = "2021.3.5 ; 2021.3.6 ; 2021.3.11",
                LightPattern = ToBytes(ToArray("0F 84 9C 00 00 00 C7 44 24 20 24"), ToArray("75 14 B8 02 00 00 00 E9 66")),
                DarkPattern = ToBytes(ToArray("E9 9D 00 00 00 00 C7 44 24 20 24"), ToArray("74 14 B8 02 00 00 00 E9 66")),
            },
            new()
            {
                Version = "2021.3.19",
                LightPattern = ToBytes(ToArray("0F 84 A4 00 00 00 C7 44 24 20 25"), ToArray("75 14 B8 02 00 00 00 E9 66")),
                DarkPattern = ToBytes(ToArray("0F 85 A4 00 00 00 C7 44 24 20 25"), ToArray("74 14 B8 02 00 00 00 E9 66")),
            },
            new()
            {
                // beta
                Version = "2022.1.0",
                Flag = VersionFlag.Beta,
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
            // MacOS X86_X64
            // MacOS X86_X64
            // MacOS X86_X64
            new()
            {
                Version = "4.7.2",
                LightPattern = ToBytes("84 5C 02 00 00 48 8D B5 B0 FE FF FF 4C 89 F7"),
                DarkPattern = ToBytes("85 5C 02 00 00 48 8D B5 B0 FE FF FF 4C 89 F7"),
            },
            new()
            {
                Version = "5.6.6 ; 5.6.7",
                LightPattern = ToBytes("84 CC 00 00 00 49 8B 37 E8 40 63 00 00 BB 0C"),
                DarkPattern = ToBytes("85 CC 00 00 00 49 8B 37 E8 40 63 00 00 BB 0C"),
            },
            new()
            {
                Version = "2017.4.40",
                LightPattern = ToBytes("26 02 88 D8 48 83 C4 18 5B 41 5C 41 5D 41 5E"),
                DarkPattern = ToBytes("26 02 B0 01 48 83 C4 18 5B 41 5C 41 5D 41 5E"),
            },
            new()
            {
                Version = "2018.4.36",
                LightPattern = ToBytes("D0 00 88 D8 48 83 C4 18 5B 41 5C 41 5D 41 5E"),
                DarkPattern = ToBytes("D0 00 B0 01 48 83 C4 18 5B 41 5C 41 5D 41 5E"),
            },
            new()
            {
                Version = "2019.1.14",
                LightPattern = ToBytes(ToArray("8C FF 80 B8 D9 4B 00 00 00 74 74 48"), ToArray("2F 00 00 41 89 C4 48 8B 7D A0 48 85 FF 74")),
                DarkPattern = ToBytes(ToArray("8C FF 80 B8 D9 4B 00 00 00 75 74 48"), ToArray("2F 00 00 41 89 C4 48 8B 7D A0 48 85 FF 75")),
            },
            new()
            {
                Version = "2019.2.21",
                LightPattern = ToBytes(ToArray("90 FF 80 B8 D9 4B 00 00 00 74 4A"), ToArray("2F 00 00 41 89 C4 48 8B 7D A0 48 85 FF 74")),
                DarkPattern = ToBytes(ToArray("90 FF 80 B8 D9 4B 00 00 00 75 4A"), ToArray("2F 00 00 41 89 C4 48 8B 7D A0 48 85 FF 75")),
            },
            new()
            {
                Version = "2019.3.4",
                LightPattern = ToBytes(ToArray("02 73 FF 80 B8 69 4C 00 00 00 74 5D"), ToArray("FE BB 02 00 00 00 45 84 E4 0F 84 3E 02")),
                DarkPattern = ToBytes(ToArray("02 73 FF 80 B8 69 4C 00 00 00 EB 5D"), ToArray("FE BB 02 00 00 00 45 84 E4 0F 85 3E 02")),
            },
            new()
            {
                Version = "2019.3.15",
                LightPattern = ToBytes(ToArray("? FF 80 B8 69 4C 00 00 00 74 5D"), ToArray("BB 02 00 00 00 45 84 E4 0F 84 3E 02")),
                DarkPattern = ToBytes(ToArray("? FF 80 B8 69 4C 00 00 00 EB 5D"), ToArray("BB 02 00 00 00 45 84 E4 0F 85 3E 02")),
            },
            new()
            {
                Version = "2019.4.9",
                LightPattern = ToBytes(ToArray("05 6F FF 80 B8 69 4C 00 00 00 74 5D"), ToArray("FE BB 02 00 00 00 45 84 E4 0F 84 3E 02")),
                DarkPattern = ToBytes(ToArray("05 6F FF 80 B8 69 4C 00 00 00 EB 5D"), ToArray("FE BB 02 00 00 00 45 84 E4 0F 85 3E 02")),
            },
            new()
            {
                Version = "2019.4.10",
                LightPattern = ToBytes(ToArray("C3 6E FF 80 B8 69 4C 00 00 00 74 5D"), ToArray("FE BB 02 00 00 00 45 84 E4 0F 84 3E 02")),
                DarkPattern = ToBytes(ToArray("C3 6E FF 80 B8 69 4C 00 00 00 EB 5D"), ToArray("FE BB 02 00 00 00 45 84 E4 0F 85 3E 02")),
            },
            new()
            {
                Version = "2019.4.28",
                LightPattern = ToBytes(ToArray("6B FF 80 B8 69 4C 00 00 00 74 5D"), ToArray("BB 02 00 00 00 45 84 E4 0F 84 3E 02")),
                DarkPattern = ToBytes(ToArray("6B FF 80 B8 69 4C 00 00 00 EB 5D"), ToArray("BB 02 00 00 00 45 84 E4 0F 85 3E 02")),
            },
            new()
            {
                Version = "2019.4.40 ; 2019.4.39 ; 2019.4.38 ; 2019.4.37",
                LightPattern = ToBytes(ToArray("72 FF 80 B8 69 4C 00 00 00 74 5D"), ToArray("BB 02 00 00 00 45 84 FF 0F 84 2E")),
                DarkPattern = ToBytes(ToArray("72 FF 80 B8 69 4C 00 00 00 EB 5D"), ToArray("BB 02 00 00 00 45 84 FF 0F 85 2E")),
            },
            new()
            {
                Version = "2020.2.7 ; 2020.3.34 ; 2020.3.35",
                LightPattern = ToBytes(ToArray("FF 80 B8 91 58 00 00 00 74 5D 48 8D 35"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 84")),
                DarkPattern = ToBytes(ToArray("FF 80 B8 91 58 00 00 00 EB 5D 48 8D 35"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 85")),
            },
            new()
            {
                Version = "2020.3.46",
                LightPattern = ToBytes(ToArray("44 8A B0 A1 64 00 00 45 84 F6 0F 84"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 84 81")),
                DarkPattern = ToBytes(ToArray("44 8A B0 A1 64 00 00 45 84 F6 0F 85"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 85 81")),
            },
            new()
            {
                Version = "2021.1.7",
                LightPattern = ToBytes(ToArray("44 8A B0 A1 64 00 00 45 84 F6 74 66"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 84 7F")),
                DarkPattern = ToBytes(ToArray("44 8A B0 A1 64 00 00 45 84 F6 EB 66"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 85 7F")),
            },
            new()
            {
                Version = "2021.2.19",
                LightPattern = ToBytes(ToArray("44 8A B0 A1 64 00 00 45 84 F6 74 66"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 84 3E")),
                DarkPattern = ToBytes(ToArray("44 8A B0 A1 64 00 00 45 84 F6 EB 66"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 85 3E")),
            },
            new()
            {
                Version = "2021.3.6 ; 2021.3.11 ; 2021.3.13",
                LightPattern = ToBytes(ToArray("B0 A1 64 00 00 45 84 F6 74 66 48 8D 35"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 84 3E")),
                DarkPattern = ToBytes(ToArray("B0 A1 64 00 00 45 84 F6 EB 66 48 8D 35"), ToArray("00 41 BF 02 00 00 00 84 C0 0F 85 3E")),
            },
            new()
            {
                Version = "2021.3.19 ; 2021.3.20",
                LightPattern = ToBytes(ToArray("8A B0 A1 64 00 00 45 84 F6 0F 84 A6"), ToArray("2A 00 00 41 BF 02 00 00 00 84 C0 0F 84")),
                DarkPattern = ToBytes(ToArray("8A B0 A1 64 00 00 45 84 F6 E9 A7 00"), ToArray("2A 00 00 41 BF 02 00 00 00 84 C0 0F 85")),
            },
            new()
            {
                Version = "2022.1.2 ; 2022.1.18",
                LightPattern = ToBytes(ToArray("8A B0 A1 64 00 00 45 84 F6 0F 84 A6"), ToArray("2A 00 00 41 BF 02 00 00 00 84 C0 0F 84")),
                DarkPattern = ToBytes(ToArray("8A B0 A1 64 00 00 45 84 F6 E9 A7 00"), ToArray("2A 00 00 41 BF 02 00 00 00 84 C0 0F 85")),
            },

            // MacOS ARM64
            // MacOS ARM64
            // MacOS ARM64
            new()
            {
                Version = "2021.2.11",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 A1 CB 00 90 21 40 22 91 03"), ToArray("94 20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 A1 CB 00 90 21 40 22 91 03"), ToArray("94 20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B"))
            },
            new()
            {
                Version = "2021.3.1",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 21 CC 00 90 21 40 22 91 83"), ToArray("94 20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 21 CC 00 90 21 40 22 91 83"), ToArray("94 20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                Version = "2021.3.2",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 21 CC 00 B0 21 40 22 91 83"), ToArray("94 20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 21 CC 00 B0 21 40 22 91 83"), ToArray("94 1F 20 03 D5 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                Version = "2021.3.3",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 A1 CC 00 D0 21 40 23 91"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 A1 CC 00 D0 21 40 23 91"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                Version = "2021.3.4",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 A1 CC 00 F0 21 20 23 91"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 A1 CC 00 F0 21 20 23 91"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                Version = "2021.3.5",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 C1 CC 00 B0 21 A0 23 91"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 C1 CC 00 B0 21 A0 23 91"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                Version = "2021.3.6 ; 2021.3.7",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 81 D0 00 F0 21 20 25 91"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 81 D0 00 F0 21 20 25 91"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                Version = "2021.3.8",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 ? D0 00 ? 21 ? ? 91"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 ? D0 00 ? 21 ? ? 91"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                Version = "2021.3.10 ; 2021.3.11",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("F4 02 00 34 ? ? 00 ? 21 ? ? 91"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("17 00 00 14 ? ? 00 ? 21 ? ? 91"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                Version = "2021.3.12",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("14 68 68 38 F4 02 00 34 ? ? 00 ? 21 ? ? 91"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("14 68 68 38 17 00 00 14 ? ? 00 ? 21 ? ? 91"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                Version = "2021.3.19 ; 2021.3.20",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("14 68 68 38 14 05 00 34 41 D3 00 90 21 60"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("14 68 68 38 28 00 00 14 41 D3 00 90 21 60"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                Version = "2021.3.21",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("14 68 68 38 14 05 00 34"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("14 68 68 38 28 00 00 14"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
            new()
            {
                Version = "2022.1.2",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("14 05 00 34 21 CE 00 D0 21 00 37"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13")),
                DarkPattern = ToBytes(ToArray("28 00 00 14 21 CE 00 D0 21 00 37"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13")),
            },
            new()
            {
                Version = "2022.1.18",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("14 05 00 34 21 ? 00 ? 21 ? ?"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13")),
                DarkPattern = ToBytes(ToArray("28 00 00 14 21 ? 00 ? 21 ? ?"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13")),
            },
            new()
            {
                Version = "2022.1.21",
                Architecture = ArchitectureType.MacOS_ARM64,
                LightPattern = ToBytes(ToArray("14 68 68 38 14 05 00 34 41 ? 00 ? 21 ? ? 91"), ToArray("20 06 00 36 E1 E3 01 91 E0 03 13 AA 2F 0B")),
                DarkPattern = ToBytes(ToArray("14 68 68 38 28 00 00 14 41 ? 00 ? 21 ? ? 91"), ToArray("20 06 00 37 E1 E3 01 91 E0 03 13 AA 2F 0B")),
            },
        };

        static readonly List<UnityPatchInfo> LinuxPatches = new()
        {
            new()
            {
                Version = "2017.4.40",
                LightPattern = ToBytes(ToArray("3C 7A 46 FF E9 78 FF FF FF 90 66 0F 1F 44 00 00 41 57 41 56 41 55")),
                DarkPattern = ToBytes(ToArray("3C 7A 46 FF E9 78 FF FF FF 90 66 0F 1F 44 00 00 B8 01 00 00 00 C3")),
            },
            new()
            {
                Version = "2018.4.36",
                LightPattern = ToBytes(ToArray("24 4F E9 BA FE FF FF 90 0F 1F 84 00 00 00 00 00 41 57 41 56 41 55")),
                DarkPattern = ToBytes(ToArray("24 4F E9 BA FE FF FF 90 0F 1F 84 00 00 00 00 00 B8 01 00 00 00 C3")),
            },
            new()
            {
                Version = "2019.4.40",
                LightPattern = ToBytes(ToArray("74 60 48 8D 35 ? ? ? 06 48 8D 0D"), ToArray("84 F6 01 00 00 48 8D 74 24 30")),
                DarkPattern = ToBytes(ToArray("EB 60 48 8D 35 ? ? ? 06 48 8D 0D"), ToArray("85 F6 01 00 00 48 8D 74 24 30")),
            },
            new()
            {
                Version = "2020.3.38",
                LightPattern = ToBytes(ToArray("74 60 48 8B 35 ? ? ? 01 48 8D 0D"), ToArray("84 78 01 00 00 48 8D 74 24 78")),
                DarkPattern = ToBytes(ToArray("EB 60 48 8D 35 ? ? ? 01 48 8D 0D"), ToArray("85 78 01 00 00 48 8D 74 24 78")),
            },
            new()
            {
                Version = "2020.3.46",
                LightPattern = ToBytes(ToArray("00 40 84 ED 0F 84 A7 00 00 00 48 8B"), ToArray("0F 84 78 01 00 00 48 8D 74 24 78")),
                DarkPattern = ToBytes(ToArray("00 40 85 ED 0F 84 A7 00 00 00 48 8B"), ToArray("0F 85 78 01 00 00 48 8D 74 24 78")),
            },
            new()
            {
                Version = "2021.3.7",
                LightPattern = ToBytes(ToArray("74 67 48 8B 35 ? ? ? 01 48 8D 0D"), ToArray("84 36 01 00 00 48 8D B4 24 A8")),
                DarkPattern = ToBytes(ToArray("EB 67 48 8B 35 ? ? ? 01 48 8D 0D"), ToArray("85 36 01 00 00 48 8D B4 24 A8")),
            },
            new()
            {
                Version = "2021.3.19",
                LightPattern = ToBytes(ToArray("0F 84 A7 00 00 00 48 8B 35 48 39 2D"), ToArray("84 36 01 00 00 48 8D B4 24 A8")),
                DarkPattern = ToBytes(ToArray("E9 A8 00 00 00 00 48 8B 35 48 39 2D"), ToArray("85 36 01 00 00 48 8D B4 24 A8")),
            },
            new()
            {
                Version = "2021.3.20",
                LightPattern = ToBytes(ToArray("00 40 84 ED 0F 84 A7 00 00 00 48 8B"), ToArray("84 36 01 00 00 48 8D B4 24 A8")),
                DarkPattern = ToBytes(ToArray("00 40 84 ED E9 A8 00 00 00 00 48 8B"), ToArray("85 36 01 00 00 48 8D B4 24 A8")),
            },
            new()
            {
                Version = "2022.1.15",
                LightPattern = ToBytes(ToArray("0F 84 A7 00 00 00 48 8B 35 D8 CE 27"), ToArray("84 36 01 00 00 48 8D B4 24 A8")),
                DarkPattern = ToBytes(ToArray("E9 A8 00 00 00 00 48 8B 35 D8 CE 27"), ToArray("85 36 01 00 00 48 8D B4 24 A8")),
            },
            new()
            {
                Version = "2022.1.23",
                LightPattern = ToBytes(ToArray("0F 84 A7 00 00 00 48 8B 35 D0 14 29 01"), ToArray("84 C0 0F 84 36 01 00 00 48 8D B4 24 A8")),
                DarkPattern = ToBytes(ToArray("E9 A8 00 00 00 00 48 8B 35 D0 14 29 01"), ToArray("84 C0 0F 85 36 01 00 00 48 8D B4 24 A8")),
            },
        };

        static UnityPatchInfos()
        {
            ProcessInfos(ref WindowsPatches, ArchitectureType.Windows);
            ProcessInfos(ref MacOSPatches, ArchitectureType.MacOS);
            ProcessInfos(ref LinuxPatches, ArchitectureType.Linux);
        }

        static void ProcessInfos(ref List<UnityPatchInfo> patchInfos, ArchitectureType type)
        {
            foreach (var item in patchInfos)
            {
                if (!item.Architecture.HasValue)
                    switch (type)
                    {
                        case ArchitectureType.Windows:
                            item.Architecture = ArchitectureType.Windows_X86_64;
                            break;
                        case ArchitectureType.MacOS:
                            item.Architecture = ArchitectureType.MacOS_X86_64;
                            break;
                        case ArchitectureType.Linux:
                            item.Architecture = ArchitectureType.Linux_X86_64;
                            break;
                    }
            }

            var tempInfoDic = new Dictionary<string, UnityPatchInfo>();
            foreach (var patchInfo in patchInfos)
            {
                var versions = patchInfo.Version.Split(";");
                foreach (var item in versions)
                {
                    var version = item.Trim();
                    var key = $"{version} - {patchInfo.Architecture}";
                    if (tempInfoDic.ContainsKey(key))
                        throw new ArgumentException($"{type} 中存在多个相同版本补丁：{key}");

                    tempInfoDic.Add(key, new()
                    {
                        Version = version,
                        Flag = patchInfo.Flag,
                        DarkPattern = patchInfo.DarkPattern,
                        LightPattern = patchInfo.LightPattern,
                        Architecture = patchInfo.Architecture,
                    });
                }
            }

            patchInfos = tempInfoDic.Values.ToList();
            patchInfos.ForEach(x => x.Init());
            patchInfos.Sort((x, y) => x.VersionID.CompareTo(y.VersionID));
        }

        public static UnityPatchInfo? FindPatchInfo(string version, ArchitectureType architectureType)
        {
            var match = Regex.Match(version, @"(?<version>\d+(\.\d+)?(\.\d+)?)");
            if (!match.Success)
                return null;

            var newVersion = match.Groups["version"].Value;
            return FindApproximateVersion(architectureType, newVersion, PlatformUtils.GetVersionFlag(version));
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

        public static UnityPatchInfo? FindApproximateVersion(ArchitectureType type, string v, VersionFlag flag)
        {
            var pathInfos = GetPatchInfos(type);
            var target = PlatformUtils.ConvertToVersionID(v, flag);
            var major = PlatformUtils.GetMajorRevision(v, flag);

            if (pathInfos != null)
            {
                UnityPatchInfo? last = null;
                for (int i = pathInfos.Count - 1; i >= 0; i--)
                {
                    var info = pathInfos[i];
                    if (info.Architecture != type)
                        continue;

                    if (target >= info.VersionID)
                    {
                        if (PlatformUtils.IsMajorEquals(info.VersionID, major))
                            return info;
                        else if (last != null && PlatformUtils.IsMajorEquals(last.VersionID, major))
                            return last;
                        else
                            return null;
                    }

                    last = info;
                }
            }

            return null;
        }
    }

#pragma warning disable CS8618
    internal class UnityPatchInfo
    {
        public string Version { get; set; }

        public long VersionID { get; set; }

        public VersionFlag Flag { get; set; }

        public ArchitectureType? Architecture { get; set; }

        public List<byte?[]> DarkPattern { get; set; }

        public List<byte?[]> LightPattern { get; set; }

        public bool IsValid()
        {
            return Architecture != ArchitectureType.UnKnown && DarkPattern != null && LightPattern != null;
        }

        public void Init()
        {
            VersionID = PlatformUtils.ConvertToVersionID(Version, Flag);
        }

        public override string ToString()
        {
            return $"{Version} - {Architecture}";
        }
    }

    internal enum VersionFlag
    {
        None = 0,
        Beta = 1,
    }
#pragma warning restore CS8618
}
