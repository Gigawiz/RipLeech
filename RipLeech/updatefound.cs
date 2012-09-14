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
    public partial class updatefound : Form
    {
        public updatefound()
        {
            InitializeComponent();
        }

        private void updatefound_Load(object sender, EventArgs e)
        {
            label4.Text = RipLeech.Properties.Settings.Default.buildvers;
            try
            {
                string updateurl = "http://nicoding.com/api.php?app=ripleech&update=check";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string pluginsavail = sr.ReadToEnd();
                label5.Text = pluginsavail;
            }
            catch
            {
                label5.Text = "3.5A-2";
            }
            if (RipLeech.Properties.Settings.Default.theme == "steamthemes")
            {
                this.BackgroundImage = RipLeech.Properties.Resources.bg1;
            }
            else if (RipLeech.Properties.Settings.Default.theme == "nicoding")
            {
                this.BackgroundImage = RipLeech.Properties.Resources.bg3;
            }
            else
            {
                this.BackgroundImage = RipLeech.Properties.Resources.bg;
            }
            try
            {
                string updateurl = "https://dl.dropbox.com/u/22054429/RipLeech/changelog.txt";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                textBox1.Text = sr.ReadToEnd();
            }
            catch
            {
                textBox1.Text = "Unable to get changelog at this time.";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            restart();
        }
        void restart()
        {
            try
            {
                string updateurl = "https://dl.dropbox.com/u/22054429/RipLeech/build.txt";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string type = sr.ReadToEnd();
                if (type != "1")
                {
                    if (File.Exists(@"C:\Program Files\NiCoding\RipLeech\updater.exe"))
                    {
                        Process.Start(@"C:\Program Files\NiCoding\RipLeech\updater.exe"); // to start new instance of application
                        Environment.Exit(0); //to turn off current app
                    }
                    else
                    {
                        updater upd = new updater();
                        upd.Show();
                        this.Close();
                    }
                }
                else
                {
                    updater upd = new updater();
                    upd.Show();
                    this.Close();
                }
            }
            catch
            {
                textBox1.Text = "Unable to get changelog at this time.";
            }
        }
    }
}
