namespace UniHacker
{
    public enum ArchitectureType
    {
        UnKnown = 0,
        Windows_I386 = 0x1,
        Windows_X86_64 = 0x2,
        Mac_I386 = 0x3,
        Mac_X86_64 = 0x4,
        Mac_ARM64 = 0x5,
        Linux_I386 = 0x6,
        Linux_X86_64 = 0x7,
    }

    internal class MachineArchitecture
    {
        public static ArchitectureType GetArchitectureType(string fileName)
        {
            try
            {
                switch (PlatformUtils.GetPlatformType())
                {
                    case PlatformType.Windows:
                        return WindowsArchitecture.GetArchitectureType(fileName);
                    case PlatformType.MacOS:
                        return MacOSArchitecture.GetArchitectureType(fileName);
                    case PlatformType.Linux:
                        return LinuxArchitecture.GetArchitectureType(fileName);
                }
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
                ArchitectureType.Mac_I386 => "X86",
                ArchitectureType.Mac_X86_64 => "X64",
                ArchitectureType.Mac_ARM64 => "ARM64",
                _ => string.Empty,
            };
        }
    }
}
