using System;
using System.Collections.Generic;
using System.Diagnostics;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using MessageBox.Avalonia.Enums;

namespace UniHacker.Views
{
    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { private set; get; }

        string? filePath;
        Patcher? patcher;

        public MainWindow()
        {
            Instance = this;
            Opened += MainWindow_Opened;

            InitializeComponent();
            InitView();
        }

        async void MainWindow_Opened(object? sender, EventArgs e)
        {
            var isAdministrator = PlatformUtils.IsAdministrator;
            if (!isAdministrator)
            {
                _ = await MessageBox.Show(Language.GetString("Non_Administrator"));
                Close();
            }
        }

        void InitView()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var version = FileVersionInfo.GetVersionInfo(assembly.Location);
            Title = $"UniHacker - Unity3D & UnityHub Patcher by tylearymf v{version.ProductVersion}";

            var controls = new List<TemplatedControl>()
            {
                FilePath, SelectBtn, Version, Status, RichText, PatchBtn, ProgressBar,
            };
            var fontFamily = PlatformUtils.FontFamily;
            controls.ForEach(x => x.FontFamily = fontFamily);

            SelectBtn.Click += SelectBtn_Click;
            PatchBtn.Click += PatchBtn_Click;
            RevertBtn.Click += RevertBtn_Click;
        }

        void UpdateFilePath(string filePath)
        {
            this.filePath = filePath;
            patcher = PatchManager.GetPatcher(filePath, PlatformUtils.GetPlatformType());
            var status = patcher?.PatchStatus ?? PatchStatus.Unknown;

            FilePath.Text = filePath;
            Version.Text = patcher?.FileVersion ?? string.Empty;
            Status.Text = Language.GetString(status.ToString());
            PatchBtn.IsEnabled = status == PatchStatus.Support;
            RevertBtn.IsEnabled = status == PatchStatus.Patched;

            if (patcher != null)
            {
                var architectureName = MachineArchitecture.GetArchitectureName(patcher.ArchitectureType);
                if (!string.IsNullOrEmpty(architectureName))
                    architectureName = $"({architectureName})";

                Version.Text = $"{patcher.FileVersion} {architectureName}";
            }
            else
            {
                Version.Text = string.Empty;
            }
        }

        async void PatchBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (patcher == null)
                return;

            try
            {
                PatchBtn.IsEnabled = false;

                (bool success, string errorMsg) = await patcher.ApplyPatch(progress => ProgressBar.Value = (int)(progress * 100));
                var msg = Language.GetString(success ? "Patch_success" : "Patch_fail");
                _ = await MessageBox.Show(string.Format("{0}\n\n{1}", msg, errorMsg));

                UpdateFilePath(filePath!);
            }
            catch (Exception ex)
            {
                PatchBtn.IsEnabled = true;
                _ = await MessageBox.Show(ex.ToString());
            }
        }

        async void RevertBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (patcher == null)
                return;

            var result = await MessageBox.Show(Language.GetString("Revert_Desc"), ButtonEnum.YesNo);
            if (result == ButtonResult.Yes)
            {
                try
                {
                    RevertBtn.IsEnabled = false;

                    (bool success, string errorMsg) = await patcher.RemovePatch(progress => ProgressBar.Value = (int)(progress * 100));
                    var msg = Language.GetString(success ? "Revert_success" : "Revert_fail");
                    _ = await MessageBox.Show(string.Format("{0}\n\n{1}", msg, errorMsg));

                    UpdateFilePath(filePath!);
                }
                catch (Exception ex)
                {
                    RevertBtn.IsEnabled = true;
                    _ = await MessageBox.Show(ex.ToString());
                }
            }
        }

        async void SelectBtn_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (!PlatformUtils.IsLinux())
            {
                dialog.Filters = new List<FileDialogFilter>()
                {
                    new FileDialogFilter()
                    {
                        Name = "Unity | UnityHub", Extensions = new (){ PlatformUtils.GetExtension(false) }
                    }
                };
            }

            var results = await dialog.ShowAsync(this);
            if (results?.Length > 0)
                UpdateFilePath(results[0]);
        }
    }
}
