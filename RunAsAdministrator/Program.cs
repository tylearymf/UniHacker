using System;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace RunAsAdministrator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (var path in args)
            {
                TrySetAccessControl(path);
                TrySetAccessControl(path + ".bak");
                TrySetAccessControl(Path.GetDirectoryName(path));
            }
        }

        static void TrySetAccessControl(string path)
        {
            if (File.Exists(path) || Directory.Exists(path))
            {
                Console.WriteLine();
                Console.Write($"Try set access permissions '{path}':");
                try
                {
                    var directoryInfo = new DirectoryInfo(path);
                    var directorySecurity = directoryInfo.GetAccessControl();
                    directorySecurity.AddAccessRule(new FileSystemAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), FileSystemRights.FullControl,
                                                    InheritanceFlags.ObjectInherit | InheritanceFlags.ContainerInherit, PropagationFlags.NoPropagateInherit, AccessControlType.Allow));
                    directoryInfo.SetAccessControl(directorySecurity);
                    Console.Write("Success");
                }
                catch
                {
                    Console.Write("Failed");
                }
            }
        }
    }
}
