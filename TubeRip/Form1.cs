﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading;

namespace TubeRip
{
    public partial class Form1 : Form
    {
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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool isconnected = checknetconnection();
            if (isconnected == false)
            {
                MessageBox.Show("You must be connected to the internet to use TubeRip!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(0);
            }
            else
            {
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer2.Start();
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            timer1.Stop();
            label1.Text = "Loading Core Components..." + " " + progressBar1.Value.ToString() + "%";

            if (progressBar1.Value < 20)
            {
                progressBar1.Value += 1;
            }
            else
            {
                timer3.Start();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            label1.Text = "Loading Encryption Algorithyms..." + " " + progressBar1.Value.ToString() + "%";
            if (progressBar1.Value < 40)
            {
                progressBar1.Value += 1;
            }
            else
            {
                timer4.Start();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            timer3.Stop();
            label1.Text = "Enabling Download Services..." + " " + progressBar1.Value.ToString() + "%";
            if (progressBar1.Value < 60)
            {
                progressBar1.Value += 1;
            }
            else
            {
                timer5.Start();
            }

        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            timer4.Stop();
            label1.Text = "Disabling Youtube's Download Protection..." + " " + progressBar1.Value.ToString() + "%";
            if (progressBar1.Value < 80)
            {
                progressBar1.Value += 1;
            }
            else
            {
                timer6.Start();
            }

        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            timer5.Stop();
            label1.Text = "Drawing GUI" + " " + progressBar1.Value.ToString() + "%";
            if (progressBar1.Value < 90)
            {
                progressBar1.Value += 1;
            }
            else
            {
                timer7.Start();
            }
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            timer6.Stop();
            label1.Text = "Creating The first Humanlike Robot..." + " " + progressBar1.Value.ToString() + "%";
            if (progressBar1.Value < 100)
            {
                progressBar1.Value += 1;
            }
            else
            {
                timer7.Stop();
                getupdates();
            }
        }
        private void getupdates()
        {
            try
            {
                string updateurl = "http://dl.dropbox.com/u/22054429/TubeRip_version.txt";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string update = sr.ReadToEnd();
                int build = Convert.ToInt32(update);
                int thisbuild = 7;
                if (build > thisbuild)
                {
                    label2.Visible = true;
                    var result = MessageBox.Show("There is an update available for TubeRip! Would you like to download it now?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        updater updater = new updater();
                        updater.Show();
                        this.Dispose(false);
                    }
                }
                else
                {
                    label2.Visible = false;
                    container home = new container();
                    home.Show();
                    this.Dispose(false);
                }
            }
            catch
            {
                MessageBox.Show("Unable to connect to update server! TubeRip will check for updates at next launch!");
                label2.Visible = false;
                container home = new container();
                home.Show();
                this.Dispose(false);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

    }
}
