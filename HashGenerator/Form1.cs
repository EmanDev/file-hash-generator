using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using MaterialSkin;
using MaterialSkin.Controls;

namespace HashGenerator
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink200, TextShade.WHITE);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(materialButton1, "Browse the file you want to calcuate the hash.");
            toolTip1.SetToolTip(materialButton8, "Calculate the selected file with the selected hashes.");
            toolTip1.SetToolTip(materialButton2, "Export calculated hashes into a text file.");
            toolTip1.SetToolTip(materialButton3, "Copy calculated MD5 to clipboard.");
            toolTip1.SetToolTip(materialButton4, "Copy calculated SHA-1 to clipboard.");
            toolTip1.SetToolTip(materialButton5, "Copy calculated SHA-256 to clipboard.");
            toolTip1.SetToolTip(materialButton6, "Copy calculated SHA-384 to clipboard.");
            toolTip1.SetToolTip(materialButton7, "Copy calculated SHA-512 to clipboard.");
        }

        private void materialButton1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                materialTextBox1.Text = openFileDialog1.FileName;
            }
        }

        private void materialButton8_Click(object sender, EventArgs e)
        {
            if (materialTextBox1.Text == "")
            {
                MaterialMessageBox.Show("Please select a file to calculate.", "Browse File Path");
            }
            else
            {
                if (materialCheckbox1.Checked == true)
                {
                    backgroundWorker1.RunWorkerAsync(materialTextBox1.Text);
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            string filepath = e.Argument.ToString();

            byte[] buffer;
            int BytesRead;
            long size;
            long totalBytesRead = 0;

            using (Stream file = File.OpenRead(filepath))
            {
                size = file.Length;
                using (HashAlgorithm hasher = MD5.Create())
                {
                    do
                    {
                        buffer = new byte[4096];
                        BytesRead = file.Read(buffer, 0, buffer.Length);
                        totalBytesRead += BytesRead;
                        hasher.TransformBlock(buffer, 0, BytesRead, null, 0);
                        backgroundWorker1.ReportProgress((int)((double)totalBytesRead / size * 100));
                    }
                    while (BytesRead != 0);
                    hasher.TransformFinalBlock(buffer, 0, 0);
                    e.Result = MakeHashString(hasher.Hash);
                }
            }

        }

        private static string MakeHashString(byte[] hashBytes)
        {
            StringBuilder hash = new StringBuilder(32);
            foreach (byte b in hashBytes)
                hash.Append(b.ToString("X2").ToUpper());
            return hash.ToString();
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            materialProgressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            materialTextBox2.Text = e.Result.ToString();
        }

        private void materialButton3_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(materialTextBox2.Text);
        }
    }
}
