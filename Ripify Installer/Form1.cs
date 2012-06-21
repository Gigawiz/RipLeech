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
using IWshRuntimeLibrary;


namespace TubeRip_Installer
{
    public partial class Form1 : Form
    {
        string installfolder = @"C:\Program Files\Dj Lyriz\TubeRip\";
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer2.Start();
        }
        private void writeitall()
        {
            timer9.Stop();
            DirectoryInfo dirnfo = new DirectoryInfo(installfolder + @"\Data");
            DirectoryInfo dlnfo = new DirectoryInfo(installfolder + @"\Downloads");
            string dir = installfolder + "Google.GData.Client.dll";
            System.IO.File.WriteAllBytes(dir, TubeRip_Installer.Properties.Resources.Google_GData_Client);
            string dir2 = installfolder + "Google.GData.Extensions.dll";
            System.IO.File.WriteAllBytes(dir2, TubeRip_Installer.Properties.Resources.Google_GData_Extensions);
            string dir3 = installfolder + "Google.GData.YouTube.dll";
            System.IO.File.WriteAllBytes(dir3, TubeRip_Installer.Properties.Resources.Google_GData_YouTube);
            string dir4 = installfolder + "LumenWorks.Framework.IO.dll";
            System.IO.File.WriteAllBytes(dir4, TubeRip_Installer.Properties.Resources.LumenWorks_Framework_IO);
            string dir5 = installfolder + "Newtonsoft.Json.dll";
            System.IO.File.WriteAllBytes(dir5, TubeRip_Installer.Properties.Resources.Newtonsoft_Json);
            string dir6 = installfolder + "TubeRip.exe";
            System.IO.File.WriteAllBytes(dir6, TubeRip_Installer.Properties.Resources.TubeRip1);
            string dir9 = installfolder + "DevComponents.DotNetBar2.dll";
            System.IO.File.WriteAllBytes(dir9, TubeRip_Installer.Properties.Resources.DevComponents_DotNetBar2);
            string dir10 = installfolder + "MySql.Data.dll";
            System.IO.File.WriteAllBytes(dir10, TubeRip_Installer.Properties.Resources.MySql_Data);
            if (dlnfo.Exists)
            {
                Directory.Delete(installfolder + @"\Downloads", true);
            }
            if (!dirnfo.Exists)
            {
                Directory.CreateDirectory(installfolder);
                Directory.CreateDirectory(installfolder + @"Data");
            }
            string dir7 = installfolder + @"Data\ffmpeg-32.exe";
            System.IO.File.WriteAllBytes(dir7, TubeRip_Installer.Properties.Resources.ffmpeg_32);
            string dir8 = installfolder + @"Data\ffmpeg-64.exe";
            System.IO.File.WriteAllBytes(dir8, TubeRip_Installer.Properties.Resources.ffmpeg_64);

            #region shortcut installer
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\TubeRip.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.Description = "TubeRip";
            shortcut.Hotkey = "Ctrl+Shift+T";
            shortcut.TargetPath = installfolder + @"\\TubeRip.exe";
            shortcut.Save();
            #endregion
            end();
        }
        private void timer2_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            if (progressBar1.Value < 12)
            {
                progressBar1.Value += 1;
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
                listBox1.Items.Add("Extracting Libraries..." + " " + progressBar1.Value.ToString() + "%");
            }
            else
            {
                timer3.Start();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            timer2.Stop();
            if (progressBar1.Value < 24)
            {
                progressBar1.Value += 1;
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
                listBox1.Items.Add("Extracting Libraries..." + " " + progressBar1.Value.ToString() + "%");
            }
            else
            {
                timer4.Start();
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            timer3.Stop();
            if (progressBar1.Value < 36)
            {
                progressBar1.Value += 1;
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
                listBox1.Items.Add("Extracting Libraries..." + " " + progressBar1.Value.ToString() + "%");
            }
            else
            {
                timer5.Start();
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
        {
            timer4.Stop();
            if (progressBar1.Value < 48)
            {
                progressBar1.Value += 1;
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
                listBox1.Items.Add("Extracting Libraries..." + " " + progressBar1.Value.ToString() + "%");
            }
            else
            {
                timer6.Start();
            }
        }

        private void timer6_Tick(object sender, EventArgs e)
        {
            timer5.Stop();
            if (progressBar1.Value < 60)
            {
                progressBar1.Value += 1;
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
                listBox1.Items.Add("Extracting Libraries..." + " " + progressBar1.Value.ToString() + "%");
            }
            else
            {
                timer7.Start();
            }
        }

        private void timer7_Tick(object sender, EventArgs e)
        {
            timer6.Stop();
            if (!Directory.Exists(installfolder + @"\Data"))
            {
                Directory.CreateDirectory(installfolder + @"\Data");
            }
            if (progressBar1.Value < 74)
            {
                progressBar1.Value += 1;
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
                listBox1.Items.Add("Extracting Libraries..." + " " + progressBar1.Value.ToString() + "%");
            }
            else
            {
                timer8.Start();
            }
        }

        private void timer8_Tick(object sender, EventArgs e)
        {
            timer7.Stop();
            if (progressBar1.Value < 86)
            {
                progressBar1.Value += 1;
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
                listBox1.Items.Add("Extracting Libraries..." + " " + progressBar1.Value.ToString() + "%");
            }
            else
            {
                timer9.Start();
            }
        }

        private void timer9_Tick(object sender, EventArgs e)
        {
            timer8.Stop();
            if (progressBar1.Value < 100)
            {
                progressBar1.Value += 1; listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
                listBox1.Items.Add("Extracting Libraries..." + " " + progressBar1.Value.ToString() + "%");
            }
            else
            {
                listBox1.SelectedIndex = listBox1.Items.Count - 1;
                listBox1.SelectedIndex = -1;
                writeitall();
            }
        }

        private void end()
        {
            MessageBox.Show("Successfully Installed!");
            Process.Start(installfolder + "TubeRip.exe");
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
