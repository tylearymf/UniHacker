using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniHacker
{
    internal class PermissionSet
    {
        public static bool TrySetAccess(params string[] paths)
        {
            var process = new Process();
            process.StartInfo.FileName = $"RunAsAdministrator.exe";
            process.StartInfo.Arguments = string.Join(" ", paths.ToList().ConvertAll(x => $"\"{x}\""));
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();
            return true;
        }
    }
}
