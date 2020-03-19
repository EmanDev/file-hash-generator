using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.ComponentModel;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace HashGenerator
{
    public partial class Form1 : MaterialForm
    {
        public Form1()
        {
            InitializeComponent();
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.EnforceBackcolorOnAllComponents = true;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.Indigo500, Primary.Indigo700, Primary.Indigo100, Accent.Pink200, TextShade.WHITE);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Text Documents (*.txt)|*.txt|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            toolTip1.ShowAlways = true;
            toolTip1.SetToolTip(materialButton1, "Browse the file you want to calcuate the hash.");
            toolTip1.SetToolTip(materialButton8, "Calculate the selected file with the selected hashes.");
            toolTip1.SetToolTip(materialButton2, "Export calculated hashes into a file.");
            toolTip1.SetToolTip(materialButton3, "Copy calculated MD5 to clipboard.");
            toolTip1.SetToolTip(materialButton4, "Copy calculated SHA-1 to clipboard.");
            toolTip1.SetToolTip(materialButton5, "Copy calculated SHA-256 to clipboard.");
            toolTip1.SetToolTip(materialButton6, "Copy calculated SHA-384 to clipboard.");
            toolTip1.SetToolTip(materialButton7, "Copy calculated SHA-512 to clipboard.");

            // Disable bugged tools here (add //fixed for fixed bugs)
            materialCheckbox5.Enabled = true; //fixed
        }

        private void MaterialButton1_Click(object sender, EventArgs e)
        {
            materialTextBox1.Text = "";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                materialTextBox1.Text = openFileDialog1.FileName;
            }
        }

        private void MaterialButton8_Click(object sender, EventArgs e)
        {
            materialTextBox2.Text = "";
            materialTextBox3.Text = "";
            materialTextBox4.Text = "";
            materialTextBox5.Text = "";
            materialTextBox6.Text = "";
            if (materialTextBox1.Text == "")
            {
                MessageBox.Show("Error: No file selected, please select a file to calculate.", "Browse File Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                long length = new System.IO.FileInfo(materialTextBox1.Text).Length;
                if (length == 0)
                {
                    MessageBox.Show("Error: Selected file is empty, please select another file.", "Browse File Path", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (materialCheckbox1.Checked == true)
                    {
                        backgroundWorker1.RunWorkerAsync(materialTextBox1.Text);
                    }
                    else
                    {
                        materialTextBox2.Text = "MD5 not selected.";
                    }
                    if (materialCheckbox2.Checked == true)
                    {
                        backgroundWorker2.RunWorkerAsync(materialTextBox1.Text);
                    }
                    else
                    {
                        materialTextBox3.Text = "SHA-1 not selected.";
                    }
                    if (materialCheckbox3.Checked == true)
                    {
                        backgroundWorker3.RunWorkerAsync(materialTextBox1.Text);
                    }
                    else
                    {
                        materialTextBox4.Text = "SHA-256 not selected.";
                    }
                    if (materialCheckbox4.Checked == true)
                    {
                        backgroundWorker4.RunWorkerAsync(materialTextBox1.Text);
                    }
                    else
                    {
                        materialTextBox5.Text = "SHA-384 not selected.";
                    }
                    if (materialCheckbox5.Checked == true)
                    {
                        backgroundWorker5.RunWorkerAsync(materialTextBox1.Text);
                    }
                    else
                    {
                        if (materialCheckbox5.Enabled == false)
                        {
                            materialTextBox6.Text = "SHA-512 is disabled.";
                        }
                        else
                        {
                            materialTextBox6.Text = "SHA-512 not selected.";
                        }
                    }
                }
            }
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
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

        private static string MakeHashStringSHA1(byte[] hashBytesSHA1)
        {
            StringBuilder hash = new StringBuilder(40);
            foreach (byte b in hashBytesSHA1)
                hash.Append(b.ToString("X2").ToUpper());
            return hash.ToString();
        }

        private static string MakeHashStringSHA256(byte[] hashBytesSHA256)
        {
            StringBuilder hash = new StringBuilder(64);
            foreach (byte b in hashBytesSHA256)
                hash.Append(b.ToString("X2").ToUpper());
            return hash.ToString();
        }

        private static string MakeHashStringSHA512(byte[] hashbyesSHA512)
        {
            StringBuilder hash = new StringBuilder(0);
            foreach (byte b in hashbyesSHA512)
                hash.Append(b.ToString("X2").ToUpper());
            return hash.ToString();
        }

        private static string MakeHashStringSHA384(byte[] hashbytesSHA384)
        {
            StringBuilder hash = new StringBuilder(96);
            foreach (byte b in hashbytesSHA384)
                hash.Append(b.ToString("X2").ToUpper());
            return hash.ToString();
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            materialProgressBar1.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            materialTextBox2.Text = e.Result.ToString();
            backgroundWorker1.CancelAsync();
        }

        private void MaterialButton3_Click(object sender, EventArgs e)
        {
            if (materialTextBox2.Text == "")
            {
                MessageBox.Show("Error: No hash available to copy.", "Copy Hash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Clipboard.SetText(materialTextBox2.Text);
            }
        }

        private void MaterialButton2_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                saveFileDialog1.ShowDialog();
                string exportedPath = saveFileDialog1.FileName;
                using (StreamWriter writeHash = new StreamWriter(exportedPath))
                {
                    writeHash.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    writeHash.WriteLine("Calculated hashes of " + "'" + openFileDialog1.SafeFileName + "'");
                    writeHash.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                    if (materialTextBox2.Text == "")
                    {
                        writeHash.Write("");
                    }
                    else
                    {
                        writeHash.WriteLine("MD5: " + materialTextBox2.Text);
                    }
                    if (materialTextBox3.Text == "")
                    {
                        writeHash.Write("");
                    }
                    else
                    {
                        writeHash.WriteLine("SHA-1: " + materialTextBox3.Text);
                    }
                    if (materialTextBox4.Text == "")
                    {
                        writeHash.Write("");
                    }
                    else
                    {
                        writeHash.WriteLine("SHA-256: " + materialTextBox4.Text);
                    }
                    if (materialTextBox5.Text == "")
                    {
                        writeHash.Write("");
                    }
                    else
                    {
                        writeHash.WriteLine("SHA-384: " + materialTextBox5.Text);
                    }
                    if (materialTextBox6.Text == "")
                    {
                        writeHash.Write("");
                    }
                    else
                    {
                        writeHash.WriteLine("SHA-512: " + materialTextBox6.Text);
                    }
                    writeHash.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------------------------");
                }
                MessageBox.Show("Hashes exported at " + exportedPath, "Exported Hash", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No hashes exported", "Exported Hash", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BackgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            string file_path = e.Argument.ToString();
            byte[] buffer;
            int bytesRead;
            long size;
            long totalBytesRead = 0;
            using (Stream file = File.OpenRead(file_path))
            {
                size = file.Length;
                using (HashAlgorithm hasher = SHA1.Create())
                {
                    do
                    {
                        buffer = new byte[4096];
                        bytesRead = file.Read(buffer, 0, buffer.Length);
                        totalBytesRead += bytesRead;
                        hasher.TransformBlock(buffer, 0, bytesRead, null, 0);
                        backgroundWorker2.ReportProgress((int)((double)totalBytesRead / size * 100));
                    }
                    while (bytesRead != 0);
                    hasher.TransformFinalBlock(buffer, 0, 0);
                    e.Result = MakeHashStringSHA1(hasher.Hash);
                }
            }
        }

        private void BackgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            materialProgressBar2.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            materialTextBox3.Text = e.Result.ToString();
            backgroundWorker2.CancelAsync();
        }

        private void BackgroundWorker3_DoWork(object sender, DoWorkEventArgs e)
        {
            string PathFile = e.Argument.ToString();
            byte[] buffer;
            int bytesRead;
            long size;
            long totalbytesRead = 0;
            using (Stream file = File.OpenRead(PathFile))
            {
                size = file.Length;
                using (HashAlgorithm hasher = SHA256.Create())
                {
                    do
                    {
                        buffer = new byte[4096];
                        bytesRead = file.Read(buffer, 0, buffer.Length);
                        totalbytesRead += bytesRead;
                        hasher.TransformBlock(buffer, 0, bytesRead, null, 0);
                        backgroundWorker3.ReportProgress((int)((double)totalbytesRead / size * 100));
                    }
                    while (bytesRead != 0);
                    hasher.TransformFinalBlock(buffer, 0, 0);
                    e.Result = MakeHashStringSHA256(hasher.Hash);
                }
            }
        }

        private void BackgroundWorker3_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            materialProgressBar3.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            materialTextBox4.Text = e.Result.ToString();
            backgroundWorker3.CancelAsync();
        }

        private void BackgroundWorker4_DoWork(object sender, DoWorkEventArgs e)
        {
            string Path_File = e.Argument.ToString();
            byte[] buffer;
            int bytesRead;
            long size;
            long totalbytesRead = 0;
            using (Stream file = File.OpenRead(Path_File))
            {
                size = file.Length;
                using (HashAlgorithm hasher = SHA384.Create())
                {
                    do
                    {
                        buffer = new byte[4096];
                        bytesRead = file.Read(buffer, 0, buffer.Length);
                        totalbytesRead += bytesRead;
                        hasher.TransformBlock(buffer, 0, bytesRead, null, 0);
                        backgroundWorker4.ReportProgress((int)((double)totalbytesRead / size * 100));
                    }
                    while (bytesRead != 0);
                    hasher.TransformFinalBlock(buffer, 0, 0);
                    e.Result = MakeHashStringSHA384(hasher.Hash);
                }
            }
        }

        private void BackgroundWorker4_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            materialProgressBar4.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            materialTextBox5.Text = e.Result.ToString();
            backgroundWorker4.CancelAsync();
        }

        private void MaterialButton7_Click(object sender, EventArgs e)
        {
            if (materialTextBox6.Text == "")
            {
                MessageBox.Show("Error: No hash available to copy.", "Copy Hash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Clipboard.SetText(materialTextBox6.Text);
            }
        }

        private void MaterialButton4_Click(object sender, EventArgs e)
        {
            if (materialTextBox3.Text == "")
            {
                MessageBox.Show("Error: No hash available to copy.", "Copy Hash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Clipboard.SetText(materialTextBox3.Text);
            }
        }

        private void MaterialButton5_Click(object sender, EventArgs e)
        {
            if (materialTextBox4.Text == "")
            {
                MessageBox.Show("Error: No hash available to copy.", "Copy Hash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Clipboard.SetText(materialTextBox4.Text);
            }
        }

        private void MaterialButton6_Click(object sender, EventArgs e)
        {
            if (materialTextBox5.Text == "")
            {
                MessageBox.Show("Error: No hash available to copy.", "Copy Hash", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Clipboard.SetText(materialTextBox5.Text);
            }
        }

        private void BackgroundWorker5_DoWork(object sender, DoWorkEventArgs e)
        {
            string sha512path = e.Argument.ToString();

            byte[] buffer;
            int BytesRead;
            long size;
            long totalBytesRead = 0;
            using (Stream file = File.OpenRead(sha512path))
            {
                size = file.Length;
                using (HashAlgorithm hasher = SHA512.Create())
                {
                    do
                    {
                        buffer = new byte[4096];
                        BytesRead = file.Read(buffer, 0, buffer.Length);
                        totalBytesRead += BytesRead;
                        hasher.TransformBlock(buffer, 0, BytesRead, null, 0);
                        backgroundWorker5.ReportProgress((int)((double)totalBytesRead / size * 100));
                    }
                    while (BytesRead != 0);
                    hasher.TransformFinalBlock(buffer, 0, 0);
                    e.Result = MakeHashStringSHA512(hasher.Hash);
                }
            }
        }

        private void BackgroundWorker5_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            materialProgressBar5.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            materialTextBox6.Text = e.Result.ToString();
            backgroundWorker5.CancelAsync();
        }

        private void MaterialButton9_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }
    }
}
