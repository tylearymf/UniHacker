using System.Threading.Tasks;
using Avalonia.Controls;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using MessageBoxAvalonia = MessageBox.Avalonia;

namespace UniHacker
{
    internal class MessageBox
    {
        static readonly MessageBoxStandardParams s_Params = new()
        {
            WindowIcon = new WindowIcon(PlatformUtils.IconStream),
            FontFamily = PlatformUtils.FontFamily,
            ButtonDefinitions = ButtonEnum.Ok,
            ShowInCenter = true,
            CanResize = false,
            MinWidth = 400,
            MinHeight = 160,
        };

        public static Task<ButtonResult> Show(string message)
        {
            var mainView = Views.MainWindow.Instance;
            s_Params.ContentMessage = message;
            var messageView = MessageBoxAvalonia.MessageBoxManager.GetMessageBoxStandardWindow(s_Params);

            if (mainView?.IsVisible ?? false)
                return messageView.ShowDialog(mainView);
            else
                return messageView.Show();
        }
    }
}
