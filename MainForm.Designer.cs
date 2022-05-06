using System.Windows.Forms;

namespace UniHacker
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.textBox_filePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_fileVer = new System.Windows.Forms.TextBox();
            this.textBox_fileStatus = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei UI", 15F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(325, 91);
            this.button1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 137);
            this.button1.TabIndex = 0;
            this.button1.Text = "PATCH";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.progressBar1.Location = new System.Drawing.Point(8, 231);
            this.progressBar1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(449, 6);
            this.progressBar1.TabIndex = 1;
            // 
            // textBox_filePath
            // 
            this.textBox_filePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_filePath.Location = new System.Drawing.Point(8, 28);
            this.textBox_filePath.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.textBox_filePath.Name = "textBox_filePath";
            this.textBox_filePath.ReadOnly = true;
            this.textBox_filePath.Size = new System.Drawing.Size(374, 21);
            this.textBox_filePath.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "Drag Unity.exe or Unity Hub.exe to the window";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 62);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Version:";
            // 
            // textBox_fileVer
            // 
            this.textBox_fileVer.Location = new System.Drawing.Point(69, 60);
            this.textBox_fileVer.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.textBox_fileVer.Name = "textBox_fileVer";
            this.textBox_fileVer.ReadOnly = true;
            this.textBox_fileVer.Size = new System.Drawing.Size(102, 21);
            this.textBox_fileVer.TabIndex = 5;
            this.textBox_fileVer.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // textBox_fileStatus
            // 
            this.textBox_fileStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_fileStatus.Location = new System.Drawing.Point(325, 60);
            this.textBox_fileStatus.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.textBox_fileStatus.Name = "textBox_fileStatus";
            this.textBox_fileStatus.ReadOnly = true;
            this.textBox_fileStatus.Size = new System.Drawing.Size(136, 21);
            this.textBox_fileStatus.TabIndex = 7;
            this.textBox_fileStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(264, 63);
            this.label3.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "Status:";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.richTextBox1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.richTextBox1.Location = new System.Drawing.Point(8, 91);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox1.Size = new System.Drawing.Size(316, 140);
            this.richTextBox1.TabIndex = 13;
            this.richTextBox1.Text = "Patch all versions of Unity and UnityHub.  \n \nIf patch fails, please submit unity" +
    ".exe binary file or Unity Hub version number on Github issue.  \nhttps://www.gith" +
    "ub.com/tylearymf/UniHacker";
            this.richTextBox1.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.richTextBox1_LinkClicked);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(386, 26);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Select";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // MainForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 240);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.textBox_fileStatus);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_fileVer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_filePath);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.Text = "UniHacker";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.MainForm_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button button1;
        private ProgressBar progressBar1;
        private TextBox textBox1;
        private Label label1;
        private Label label2;
        private TextBox textBox_fileVer;
        private TextBox textBox_fileStatus;
        private Label label3;
        private RichTextBox richTextBox1;
        private TextBox textBox_filePath;
        private Button button2;
    }
}