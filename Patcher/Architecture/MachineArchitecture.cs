namespace UniHacker
{
    public enum ArchitectureType : uint
    {
        UnKnown = 0,
        Windows = 1 << 7,
        MacOS = 1 << 6,
        Linux = 1 << 5,
        Windows_I386 = 0x1 | Windows,
        Windows_X86_64 = 0x2 | Windows,
        MacOS_I386 = 0x1 | MacOS,
        MacOS_X86_64 = 0x2 | MacOS,
        MacOS_ARM64 = 0x3 | MacOS,
        Linux_I386 = 0x1 | Linux,
        Linux_X86_64 = 0x2 | Linux,
    }

    internal class MachineArchitecture
    {
        public static ArchitectureType GetArchitectureType(string fileName)
        {
            try
            {
                var architectureType = ArchitectureType.UnKnown;

                architectureType = WindowsArchitecture.GetArchitectureType(fileName);
                if (architectureType != ArchitectureType.UnKnown)
                    return architectureType;

                architectureType = MacOSArchitecture.GetArchitectureType(fileName);
                if (architectureType != ArchitectureType.UnKnown)
                    return architectureType;

                architectureType = LinuxArchitecture.GetArchitectureType(fileName);
                if (architectureType != ArchitectureType.UnKnown)
                    return architectureType;
            }
            catch { }

            return ArchitectureType.UnKnown;
        }

        public static string GetArchitectureName(ArchitectureType type)
        {
            return type switch
            {
                ArchitectureType.Windows_I386 => "X86",
                ArchitectureType.Windows_X86_64 => "X64",
                ArchitectureType.MacOS_I386 => "X86",
                ArchitectureType.MacOS_X86_64 => "X64",
                ArchitectureType.MacOS_ARM64 => "ARM64",
                ArchitectureType.Linux_I386 => "X86",
                ArchitectureType.Linux_X86_64 => "X64",
                _ => string.Empty,
            };
        }
    }
}
