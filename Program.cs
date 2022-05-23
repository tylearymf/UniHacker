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
            var patchInfo = UnityPatchInfos.FindPatchInfo("2019.4.39f1", ArchitectureType.MacOS_X86_64);
            var fileBytes = File.ReadAllBytes("D:/Unity");
            var darkIndexes = BoyerMooreSearcher.FindPattern(patchInfo.DarkPattern, fileBytes);
            var lightIndexes = BoyerMooreSearcher.FindPattern(patchInfo.LightPattern, fileBytes);
            Console.WriteLine("darkIndexes:" + darkIndexes.Count);
            Console.WriteLine("lightIndexes:" + lightIndexes.Count);
#endif
        }
    }
}
