using System;
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
            var filePath = "D:/Unity";
            var version = "2022.1.3";
            var architecture = MachineArchitecture.GetArchitectureType(filePath);
            var patchInfo = UnityPatchInfos.FindPatchInfo(version, architecture);
            var fileBytes = File.ReadAllBytes(filePath);
            var darkIndexes = BoyerMooreSearcher.FindPattern(patchInfo.DarkPattern, fileBytes);
            var lightIndexes = BoyerMooreSearcher.FindPattern(patchInfo.LightPattern, fileBytes);

            if (darkIndexes.Count == patchInfo.DarkPattern.Count)
                Console.Beep();
            if (lightIndexes.Count == patchInfo.LightPattern.Count)
                Console.Beep();

            throw new Exception();
#endif
        }
    }
}
