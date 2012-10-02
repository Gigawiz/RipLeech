using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using IWshRuntimeLibrary;


namespace RipLeech_Updater
{
    public partial class Form1 : Form
    {
        string installfolder = @"C:\Program Files\NiCoding\RipLeech\";
        string filedling = null;
        int count = 0;
        int done = 0;
        double totalBytes = 0;
        double bytesIn = 0;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            getnews();
        }
        private void getnews()
        {
            string newsurl = "https://dl.dropbox.com/u/22054429/RipLeech/update_news.txt";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(newsurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                textBox1.Text = sr.ReadToEnd();
                if (System.IO.Directory.Exists(installfolder + @"\Temp") || System.IO.Directory.Exists(installfolder + @"\temp"))
                {
                    List<String> MyMusicFiles = Directory.GetFiles(installfolder + @"\Temp", "*.*", SearchOption.TopDirectoryOnly).ToList();
                    foreach (string file in MyMusicFiles)
                    {
                        FileInfo mFile = new FileInfo(file);
                        if (new FileInfo(installfolder + "\\" + mFile.Name).Exists == false)//to remove name collusion
                        {
                            mFile.MoveTo(installfolder + "\\" + mFile.Name);
                        }
                        else
                        {
                            System.IO.File.Delete(installfolder + "\\" + mFile.Name);
                            mFile.MoveTo(installfolder + "\\" + mFile.Name);
                        }
                    }
                    System.IO.Directory.Delete(installfolder + @"\Temp", true);
                    end(false);
                }
                else
                {
                    dlfiles();
                }
            }
            catch
            {
                textBox1.Text = "Unable to get update news at this time.";
                if (System.IO.Directory.Exists(installfolder + @"\Temp") || System.IO.Directory.Exists(installfolder + @"\temp"))
                {
                    List<String> MyMusicFiles = Directory.GetFiles(installfolder + @"\Temp", "*.*", SearchOption.TopDirectoryOnly).ToList();
                    foreach (string file in MyMusicFiles)
                    {
                        FileInfo mFile = new FileInfo(file);
                        if (new FileInfo(installfolder + "\\" + mFile.Name).Exists == false)//to remove name collusion
                        {
                            mFile.MoveTo(installfolder + "\\" + mFile.Name);
                        }
                        else
                        {
                            System.IO.File.Delete(installfolder + "\\" + mFile.Name);
                            mFile.MoveTo(installfolder + "\\" + mFile.Name);
                        }
                    }
                    System.IO.Directory.Delete(installfolder + @"\Temp", true);
                    end(false);
                }
                else
                {
                    dlfiles();
                }
            }
        }
        private Queue<string> _downloadUrls = new Queue<string>();
        private void dlfiles()
        {
            string updateurl = null;
            if (System.IO.File.Exists(installfolder + "beta.lock"))
            {
                updateurl = "https://dl.dropbox.com/u/22054429/RipLeech/beta_filelist.txt";
            }
            else if (System.IO.File.Exists(installfolder + "update.lock"))
            {
                updateurl = "https://dl.dropbox.com/u/22054429/RipLeech/update_filelist.txt";
            }
            else
            {
                updateurl = "https://dl.dropbox.com/u/22054429/RipLeech/filelist.txt";
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
            WebResponse response = request.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
            string test = sr.ReadToEnd();
            string[] parts = test.Split('\r');
            IEnumerable<string> urls = parts;
            foreach (string url in urls)
            {
                Match match = Regex.Match(url, @"([A-Za-z0-9\-]+)\.([A-Za-z0-9\-]+)$",
                RegexOptions.IgnoreCase);

                // Here we check the Match instance.
                if (match.Success)
                {
                    _downloadUrls.Enqueue(url);
                    count++;
                }
            }
            DownloadFile();
        }
        private void end(bool chrome)
        {
            if (System.IO.File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\RipLeech.ink"))
            {
                System.IO.File.Delete(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\RipLeech.ink");
            }
            if (chrome == true)
            {
                if (MessageBox.Show("Do you want the TubeRip Chrome Plugin?", "Chrome Addon", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    string dir2 = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\RipLeech.crx";
                    System.IO.File.WriteAllBytes(dir2, RipLeech_Updater.Properties.Resources.ripleech);
                    MessageBox.Show("A file called 'RipLeech.crx' has been created on your desktop. Open chrome, go to tools --> settings --> extensions and drag the file named 'RipLeech.crx' from your desktop into the window.");
                }
            }
            #region shortcut installer
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\RipLeech.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "RipLeech";
            shortcut.TargetPath = installfolder;
            shortcut.TargetPath = installfolder + "RipLeech.exe";
            shortcut.Save();
            #endregion
            Process.Start(installfolder + "RipLeech.exe");
            Environment.Exit(0);
        }

        private void DownloadFile()
        {
            if (_downloadUrls.Any())
            {
                WebClient client = new WebClient();
                client.DownloadProgressChanged += client_DownloadProgressChanged;
                client.DownloadFileCompleted += client_DownloadFileCompleted;

                var url = _downloadUrls.Dequeue();
                string FileName = url.Substring(url.LastIndexOf("/") + 1,
                            (url.Length - url.LastIndexOf("/") - 1));
                string filepath = GetParentUriString(new Uri(url));
                string uril2 = filepath.Replace("https://dl.dropbox.com/u/22054429/RipLeech/", installfolder).Replace("/", @"\");
                if (!String.IsNullOrEmpty(uril2))
                {
                    if (!Directory.Exists(uril2))
                    {
                        Directory.CreateDirectory(uril2);
                    }
                }
                filedling = FileName;
                    client.DownloadFileAsync(new Uri(url), uril2 + FileName);
                return;
            }
            done = count;
            // End of the download
            end(true);
        }
        static string GetParentUriString(Uri uri)
        {
            return uri.AbsoluteUri.Remove(uri.AbsoluteUri.Length - uri.Segments.Last().Length);
        }
        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // handle error scenario
                throw e.Error;
            }
            if (e.Cancelled)
            {
                // handle cancelled scenario
            }
            done++;
            DownloadFile();
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label2.Text = e.ProgressPercentage.ToString() + "%";
            bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytescheck = double.Parse(e.TotalBytesToReceive.ToString());
            string type = null;
            if (totalBytescheck >= 1024)
            {
                double totalBytescheck2 = totalBytescheck /1024;
                if (totalBytescheck2 <= 1024)
                {
                    totalBytes = double.Parse(e.TotalBytesToReceive.ToString()) /1024;
                    type = "Kb";
                }
                else
                {
                    totalBytes = double.Parse(e.TotalBytesToReceive.ToString()) / 1024 / 1024;
                    type = "Mb";
                }
            }
            else
            {
                totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                type = "Bytes";
            }
            string stripped = totalBytes.ToString().Split('.')[0];

            label5.Text = filedling + " (" + stripped + " "+ type + ")";
            double percentage = ((double)done / (double)count) * (double)100;
            progressBar2.Value = int.Parse(Math.Truncate(percentage).ToString());
            label4.Text = progressBar2.Value.ToString() +"%";
            label6.Text =  done + " / " + count + " Files Downloaded";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                webClient1.CancelAsync();
                Environment.Exit(0);
            }
            catch
            {
                Environment.Exit(0);
            }
        }
    }
}
