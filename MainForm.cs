using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace UniHacker
{
    public partial class MainForm : Form
    {
        Patcher patcher;

        public MainForm()
        {
            InitializeComponent();

            InitComponents();
        }

        void InitComponents()
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var version = FileVersionInfo.GetVersionInfo(assembly.Location);
            Text = $"UniHacker - Unity3D & UnityHub Patcher by tylearymf v{version.FileVersion}";

            label1.Text = Language.GetString("tips");
            label2.Text = Language.GetString("version");
            label3.Text = Language.GetString("status");
            richTextBox1.Text = Language.GetString("description");
            button1.Text = Language.GetString("patch_btn");
            button2.Text = Language.GetString("select");

            var len = richTextBox1.Text.Length;
            var url = "https://www.github.com/tylearymf/UniHacker";
            richTextBox1.Text += "\n" + url;
            richTextBox1.SelectionStart = len - 1;
            richTextBox1.SelectionLength = url.Length;
            richTextBox1.SelectionFont = new System.Drawing.Font("Gabriola", 15);
        }

        void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.LinkText) { UseShellExecute = true });
        }

        void MainForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            var filePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            UpdateFilePath(filePath);
        }

        void UpdateFilePath(string filePath)
        {
            patcher = PatcherManager.GetPatcher(filePath);
            var status = patcher?.PatchStatus ?? PatchStatus.Unknown;

            textBox_filePath.Text = filePath;
            textBox_fileVer.Text = patcher?.FileVersion ?? string.Empty;
            textBox_fileStatus.Text = Language.GetString(status.ToString());
            button1.Enabled = status == PatchStatus.Support;
        }

        async void button1_Click(object sender, EventArgs e)
        {
            if (patcher == null)
                return;

            button1.Enabled = false;

            try
            {
                (bool success, string errorMsg) = await patcher.ApplyPatch(progress => progressBar1.Value = (int)(progress * 100));
                var msg = Language.GetString(success ? "patch_success" : "patch_fail");
                MessageBox.Show(string.Format("{0}\n\n{1}", msg, errorMsg));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        void button2_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Unity.exe ‖ Unity Hub.exe|*.exe";
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    UpdateFilePath(dialog.FileName);
                }
            }
        }
    }
}
