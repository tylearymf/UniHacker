using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.ReactiveUI;

namespace UniHacker
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args)
        {
            //Test();
            Language.Init();
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

        static void Test()
        {
#if DEBUG
            var versionName = PlatformUtils.TryGetVersionOfUnity("D:/Unity");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine($"Start Search Pattern.");

            var filePath = "D:/Unity";
            var version = "2018.1.15";
            var architecture = MachineArchitecture.GetArchitectureType(filePath);
            var patchInfo = UnityPatchInfos.FindPatchInfo(version, architecture);
            var fileBytes = File.ReadAllBytes(filePath);
            var darkIndexes = BoyerMooreSearcher.FindPattern(patchInfo.DarkPattern, fileBytes);
            var lightIndexes = BoyerMooreSearcher.FindPattern(patchInfo.LightPattern, fileBytes);

            stopwatch.Stop();
            Console.WriteLine($"Search Pattern Finish. {stopwatch.ElapsedMilliseconds}");

            if (darkIndexes.Count == patchInfo.DarkPattern.Count)
                Console.Beep();
            if (lightIndexes.Count == patchInfo.LightPattern.Count)
                Console.Beep();

            throw new Exception();
#endif
        }
    }
}
