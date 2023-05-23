using System;
using System.Diagnostics;
using System.IO;
using Avalonia;
using Avalonia.Media;
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
                .With(new FontManagerOptions
                 {
                    DefaultFamilyName = "avares://MyAssembly/MyAssets#MyCustomFont"
                 })
                .UseReactiveUI();

        static void Test()
        {
#if DEBUG
            //var patchInfo1 = UnityPatchInfos.FindPatchInfo("2022.1.16", ArchitectureType.Linux);
            //var patchInfo2 = UnityPatchInfos.FindPatchInfo("2022.1.14", ArchitectureType.Linux);

            var versionName = PlatformUtils.TryGetVersionOfUnity("D:/Unity");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Debug.WriteLine($"Start Search Pattern.");

            var filePath = "D:/Unity";
            //var version = "2021.3.20";
            var architecture = MachineArchitecture.GetArchitectureType(filePath);
            var patchInfo = UnityPatchInfos.FindPatchInfo(versionName, architecture);
            var fileBytes = File.ReadAllBytes(filePath);
            var darkIndexes = BoyerMooreSearcher.FindPattern(patchInfo.DarkPattern, fileBytes);
            var lightIndexes = BoyerMooreSearcher.FindPattern(patchInfo.LightPattern, fileBytes);

            stopwatch.Stop();
            Debug.WriteLine($"Search Pattern Finish. {stopwatch.ElapsedMilliseconds}");

            if (darkIndexes.Count == patchInfo.DarkPattern.Count)
                Console.Beep();
            if (lightIndexes.Count == patchInfo.LightPattern.Count)
                Console.Beep();

            throw new Exception();
#endif
        }
    }
}
