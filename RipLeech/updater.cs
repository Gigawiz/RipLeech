using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Diagnostics;
using System.IO;

namespace RipLeech
{
    public partial class updater : Form
    {
        public updater()
        {
            InitializeComponent();
        }

        private void updater_Load(object sender, EventArgs e)
        {
            /*if ((RipLeech.Properties.Settings.Default.flashnotinstalled == true) && (RipLeech.Properties.Settings.Default.updateavail == false))
            {
                MessageBox.Show("Updating Flash!");
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                Uri update = new Uri("http://get.adobe.com/flashplayer/download/?installer=Flash_Player_11_for_Internet_Explorer&os=Windows%207&browser_type=Gecko&browser_dist=Firefox&a=McAfee_Security_Scan_Plus");
                client.DownloadFileAsync(update, "update.exe");
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
            }*/
                MessageBox.Show("Updating Program!");
                WebClient client = new WebClient();
                client.Encoding = System.Text.Encoding.UTF8;
                Uri update = new Uri("https://dl.dropbox.com/u/22054429/RipLeech/updater.exe");
                client.DownloadFileAsync(update, "updater.exe");
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Process.Start("updater.exe");
            Environment.Exit(0);
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            label1.Text = "Downloading Update Please Wait..." + e.ProgressPercentage.ToString() + "%";
            progressBar1.Value = e.ProgressPercentage;
        }
    }
}