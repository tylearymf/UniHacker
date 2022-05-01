using System.IO;
using System.Threading.Tasks;

namespace asardotnetasync
{
    internal static class Utils
    {

        public static void Write(this byte[] bytes, string destination)
        {
            var dirPath = Path.GetDirectoryName(destination);
            dirPath.CreateDir();
            File.WriteAllBytes(destination, bytes);
        }

        public static void CreateDir(this string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        public static async Task<int> WriteToFile(byte[] bytes, string destination)
        {
            Path.GetDirectoryName(destination).CreateDir();
            using (var fs = File.Create(destination))
            {
                await fs.WriteAsync(bytes, 0, bytes.Length);
                return 1;
            }
        }

    }
}
