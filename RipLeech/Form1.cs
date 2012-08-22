using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Microsoft.Win32;
using System.IO;
using System.Diagnostics;

namespace RipLeech
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value == 100)
            {
                timer1.Stop();
                getupdates();
            }
            else
            {
                progressBar1.Value += 1;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool isconnected = checknetconnection();
            if (isconnected == false)
            {
                MessageBox.Show("You must be connected to the internet to use RipLeech!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            else
            {
                timer1.Start();
            }
        }
        private bool checknetconnection()
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable() == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        private void checkflash()
        {
            int? flashVersion = GetFlashPlayerVersion();
            if (flashVersion.HasValue == true && flashVersion > 6)
            {
                //MessageBox.Show("Your Adobe Flash Version is : " + flashVersion.ToString());
                RipLeech.Properties.Settings.Default.flashnotinstalled = false;
                RipLeech.Properties.Settings.Default.Save();
                getupdates();
            }
            else
            {
                RipLeech.Properties.Settings.Default.flashnotinstalled = true;
                RipLeech.Properties.Settings.Default.Save();
                getupdates();
            }
        }
        private void getupdates()
        {
            try
            {
                string updateurl = null;
                if (RipLeech.Properties.Settings.Default.betaupdates == true)
                {
                    updateurl = "https://dl.dropbox.com/u/22054429/RipLeech/RipLeech_beta_version.txt";
                    if (!File.Exists("beta.lock"))
                    {
                        File.Create("beta.lock");
                    }
                }
                else
                {
                    updateurl = "https://dl.dropbox.com/u/22054429/RipLeech/RipLeech_version.txt";
                }
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string update = sr.ReadToEnd();
                int build = Convert.ToInt32(update);
                int thisbuild = 11;
                if (build > thisbuild)
                {
                    label2.Visible = true;
                    var result = MessageBox.Show("There is an update available for RipLeech! Would you like to download it now?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        if (File.Exists("updater.exe"))
                        {
                            Process.Start("updater.exe");
                            Environment.Exit(0);
                        }
                        else
                        {
                            updater updater = new updater();
                            updater.Show();
                            this.Dispose(false);
                        }
                    }
                    else
                    {
                        Menu home = new Menu();
                        home.Show();
                        this.Dispose(false);
                    }
                }
                else
                {
                    label2.Visible = false;
                    if (RipLeech.Properties.Settings.Default.flashnotinstalled == true)
                    {
                        /*updater flupdate = new updater();
                        flupdate.Show();*/
                    }
                    else
                    {
                        Menu home = new Menu();
                        home.Show();
                        this.Dispose(false);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Unable to connect to update server! RipLeech will check for updates at next launch!");
                label2.Visible = false;
                Menu home = new Menu();
                home.Show();
                this.Dispose(false);
            }
        }
        internal static int? GetFlashPlayerVersion()
        {
            using (RegistryKey rk = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Macromedia\FlashPlayer"))
            {
                if (rk != null)
                {
                    string version = rk.GetValue("CurrentVersion") as string;
                    if (string.IsNullOrEmpty(version) == false)
                    {
                        int idx = version.IndexOf(",");
                        if (idx > 0)
                        {
                            int value;
                            if (int.TryParse(version.Substring(0, idx), out value) == true)
                            {
                                return value;
                            }
                        }
                    }
                }
            }
            return null;
        }
    }
}
