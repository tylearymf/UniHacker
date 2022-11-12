using System;
using System.Threading.Tasks;
#if !DOCKER_ENV
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBoxAvalonia = MessageBox.Avalonia;
#endif

namespace UniHacker
{
    internal class MessageBox
    {
#if DOCKER_ENV
        public static Task Show(string message)
        {
            Console.WriteLine($"[UniHacker] {message}");
            return Task.CompletedTask;
        }
#else
        static readonly MessageBoxStandardParams s_Params = new()
        {
            WindowIcon = new WindowIcon(PlatformUtils.IconStream),
            FontFamily = PlatformUtils.FontFamily,
            ShowInCenter = true,
            CanResize = false,
            MinWidth = 400,
            MinHeight = 160,
        };

        public static Task<ButtonResult> Show(string message, ButtonEnum definitions = ButtonEnum.Ok)
        {
            var @params = s_Params;
            @params.ContentMessage = message;
            @params.ButtonDefinitions = definitions;

            var mainView = Views.MainWindow.Instance;
            var messageView = MessageBoxAvalonia.MessageBoxManager.GetMessageBoxStandardWindow(@params);

            if (mainView?.IsVisible ?? false)
                return messageView.ShowDialog(mainView);
            else
                return messageView.Show();
        }
#endif
    }
}
