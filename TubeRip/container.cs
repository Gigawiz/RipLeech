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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Net;

namespace TubeRip
{
    public partial class container : Form
    {
        string assembly = "";
        public container()
        {
            InitializeComponent();
        }

        private void container_Load(object sender, EventArgs e)
        {
            #region system messages
            try
            {
                string motdurl = "http://dl.dropbox.com/u/22054429/tuberip_system_messages.txt";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(motdurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string motdpost = sr.ReadToEnd();
                if (!String.IsNullOrEmpty(motdpost))
                {
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.BalloonTipTitle = "System Message";
                    notifyIcon1.BalloonTipText = motdpost;
                    notifyIcon1.ShowBalloonTip(5000);
                }
                else
                {
                    notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                    notifyIcon1.BalloonTipTitle = "Welcome!";
                    notifyIcon1.BalloonTipText = "Welcome to TubeRip!" + Environment.NewLine + "Right-Click this icon for more options.";
                    notifyIcon1.ShowBalloonTip(5000);
                }
            }
            catch (Exception motdexception)
            {
                notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
                notifyIcon1.BalloonTipTitle = "System Error!";
                notifyIcon1.BalloonTipText = "Unable to connect to TubeRip messaging server!" +Environment.NewLine + "Please try again later.";
                notifyIcon1.ShowBalloonTip(5000);
            }
            #endregion
            if (File.Exists("updater.exe"))
            {
                File.Delete("updater.exe");
            }
            if (!String.IsNullOrEmpty(TubeRip.Properties.Settings.Default.backgroundloc))
            {
                this.BackgroundImage = Image.FromFile(TubeRip.Properties.Settings.Default.backgroundloc);
                if (TubeRip.Properties.Settings.Default.bgstyle == "Tile")
                {
                    this.BackgroundImageLayout = ImageLayout.Tile;
                }
                else if (TubeRip.Properties.Settings.Default.bgstyle == "Stretch")
                {
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else if (TubeRip.Properties.Settings.Default.bgstyle == "Center")
                {
                    this.BackgroundImageLayout = ImageLayout.Center;
                }
                else
                {
                    this.BackgroundImageLayout = ImageLayout.Tile;
                }
            }
            else
            {
                this.BackgroundImage = TubeRip.Properties.Resources.bg;
            }
            #region checkforaddons
            try
            {
                string[] fileEntries = Directory.GetFiles(Directory.GetCurrentDirectory() + @"\Data\Addons\", "*.dll");
                DirectoryInfo addonnfo = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\Data\Addons");
                if (addonnfo.Exists)
                {
                    if (IsDirectoryEmpty(Directory.GetCurrentDirectory() + @"\Data\Addons") == false)
                    {
                        foreach (string fileName in fileEntries)
                        {
                            assembly = fileName;
                            string totalfilename = Path.GetFileNameWithoutExtension(fileName);
                            ToolStripMenuItem tpMenuItem21 = new ToolStripMenuItem();
                            tpMenuItem21.Name = totalfilename;
                            tpMenuItem21.Text = totalfilename;
                            tpMenuItem21.Tag = totalfilename;
                            tpMenuItem21.Click += new EventHandler(tpMenuItem21_Click);
                            rippersToolStripMenuItem.DropDownItems.Add(tpMenuItem21);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
            mainpage youtuberip = new mainpage();
            youtuberip.MdiParent = this;
            //youtuberip.WindowState = FormWindowState.Maximized;
            youtuberip.Show();
        }

        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        void tpMenuItem21_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            //MessageBox.Show(clickedItem.Tag.ToString());
            string fileopenloc = Directory.GetCurrentDirectory() + @"\Data\Addons\" + clickedItem.Tag.ToString() + ".dll";
            string formname = clickedItem.Tag.ToString() + ".Form1";
            System.Reflection.Assembly extAssembly = System.Reflection.Assembly.LoadFrom(fileopenloc);
            Form extForm = (Form)extAssembly.CreateInstance(formname, true);
            this.AddOwnedForm(extForm);
            extForm.MdiParent = this;
            extForm.Show();
        }

        private void youtubeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainpage youtuberip = new mainpage();
            youtuberip.MdiParent = this;
            //youtuberip.WindowState = FormWindowState.Maximized;
            youtuberip.Show();
        }

        private void container_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void closeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 about = new Form2();
            about.MdiParent = this;
            about.Show();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            login logged = new login();
            logged.MdiParent = this;
            logged.Show();
            timer2.Start();
        }

        private void licenseToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            addfeats feats = new addfeats();
            feats.MdiParent = this;
            feats.Show();
        }

        private void getSourceCodeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
             Process.Start("https://github.com/djlyriz/RipTube");
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TubeRip.Properties.Settings.Default.username = "";
            TubeRip.Properties.Settings.Default.password = "";
            TubeRip.Properties.Settings.Default.age = "";
            TubeRip.Properties.Settings.Default.email = "";
            TubeRip.Properties.Settings.Default.Save();
            loginToolStripMenuItem.Text = "Login";
            logOutToolStripMenuItem.Visible = false;
            profileToolStripMenuItem.Visible = false;
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            preferences prefs = new preferences();
            prefs.MdiParent = this;
            prefs.Show();
        }

        private void viewMyDetailsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            viewdetails usernfo = new viewdetails();
            usernfo.MdiParent = this;
            usernfo.Show();
        }
        #region ffmpeg tools
        private void mP4ToMP3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFLV = new OpenFileDialog();
            OFLV.Title = "Choose an MP4 File to Convert!";
            OFLV.Filter = "	MPEG-4 Video|*.mp4";
            DialogResult result = OFLV.ShowDialog();
            if (result == DialogResult.OK)
            {
                SaveFileDialog SMP3 = new SaveFileDialog();
                SMP3.Title = "Name the converted MP3 File";
                SMP3.Filter = "MP3 Format Sound|*.mp3";
                DialogResult saveresult = SMP3.ShowDialog();
                if (saveresult == DialogResult.OK)
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    string startdir = Application.StartupPath;
                    if (Environment.Is64BitOperatingSystem)
                    {
                        psi.FileName = startdir + @"\Data\ffmpeg-64.exe";
                    }
                    else
                    {
                        psi.FileName = startdir + @"\Data\ffmpeg-32.exe";
                    }
                    string bitrate = "320";
                    psi.Arguments = string.Format("-i \"{0}\" -vn -y -f mp3 -ab {2}k \"{1}\"", OFLV.FileName, SMP3.FileName, bitrate);
                    psi.WindowStyle = ProcessWindowStyle.Normal;
                    Process p = Process.Start(psi);
                    p.WaitForExit();
                    if (p.HasExited == true)
                    {
                        MessageBox.Show("Conversion Complete!");
                    }
                }
                else
                {
                    MessageBox.Show("Conversion Canceled!");
                }
            }
            else
            {
                MessageBox.Show("Conversion Canceled!");
            }
        }

        private void fLVToMP4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFLV = new OpenFileDialog();
            OFLV.Title = "Choose a FLV File to Convert!";
            OFLV.Filter = "Flash Video|*.flv";
            DialogResult result = OFLV.ShowDialog();
            if (result == DialogResult.OK)
            {
                SaveFileDialog SMP3 = new SaveFileDialog();
                SMP3.Title = "Name the converted MP4 File";
                SMP3.Filter = "MPEG-4 Video|*.mp4";
                DialogResult saveresult = SMP3.ShowDialog();
                if (saveresult == DialogResult.OK)
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    string startdir = Application.StartupPath;
                    if (Environment.Is64BitOperatingSystem)
                    {
                        psi.FileName = startdir + @"\Data\ffmpeg-64.exe";
                    }
                    else
                    {
                        psi.FileName = startdir + @"\Data\ffmpeg-32.exe";
                    }
                    psi.Arguments = string.Format("-i \"{0}\" -y -sameq -ar 22050 \"{1}\"", OFLV.FileName, SMP3.FileName);
                    psi.WindowStyle = ProcessWindowStyle.Normal;
                    Process p = Process.Start(psi);
                    p.WaitForExit();
                    if (p.HasExited == true)
                    {
                        MessageBox.Show("Conversion Complete!");
                    }
                }
                else
                {
                    MessageBox.Show("Conversion Canceled!");
                }
            }
            else
            {
                MessageBox.Show("Conversion Canceled!");
            }
        }

        private void fLVToMP3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog OFLV = new OpenFileDialog();
            OFLV.Title = "Choose a FLV File to Convert!";
            OFLV.Filter = "Flash Video|*.flv";
            DialogResult result = OFLV.ShowDialog();
            if (result == DialogResult.OK)
            {
                SaveFileDialog SMP3 = new SaveFileDialog();
                SMP3.Title = "Name the converted MP3 File";
                SMP3.Filter = "MP3 Format Sound|*.m3";
                DialogResult saveresult = SMP3.ShowDialog();
                if (saveresult == DialogResult.OK)
                {
                    ProcessStartInfo psi = new ProcessStartInfo();
                    string startdir = Application.StartupPath;
                    if (Environment.Is64BitOperatingSystem)
                    {
                        psi.FileName = startdir + @"\Data\ffmpeg-64.exe";
                    }
                    else
                    {
                        psi.FileName = startdir + @"\Data\ffmpeg-32.exe";
                    }
                    string bitrate = "320";
                    psi.Arguments = string.Format("-i \"{0}\" -vn -y -f mp3 -ab {2}k \"{1}\"", OFLV.FileName, SMP3.FileName, bitrate);
                    psi.WindowStyle = ProcessWindowStyle.Normal;
                    Process p = Process.Start(psi);
                    p.WaitForExit();
                    if (p.HasExited == true)
                    {
                        MessageBox.Show("Conversion Complete!");
                    }
                }
                else
                {
                    MessageBox.Show("Conversion Canceled!");
                }
            }
            else
            {
                MessageBox.Show("Conversion Canceled!");
            }
        }
        #endregion
        private void closeAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Gets forms that represent the MDI child forms 
            //that are parented to this form in an array 
            Form[] charr = this.MdiChildren;

            //For each child form set the window state to Maximized 
            foreach (Form chform in charr)
            {
                chform.Close();
            }

        }

        private void cascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set the layout to cascade.
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set the layout to tile horizontal.
            this.LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set the layout to tile vertical.
            this.LayoutMdi(MdiLayout.TileVertical);
        }

        private void arrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Set the layout to arrange icons.
            this.LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void timer2_Tick_1(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(TubeRip.Properties.Settings.Default.username))
            {
                loginToolStripMenuItem.Text = TubeRip.Properties.Settings.Default.username;
                logOutToolStripMenuItem.Visible = true;
                profileToolStripMenuItem.Visible = true;
                timer2.Stop();
            }
        }

        private void myDownloadHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            /*string logtoencrypt = Application.StartupPath + @"\temp_history.trh";
            string logtosave = Application.StartupPath + @"\history.trh";
            logencryptor.EncryptFile(logtoencrypt, logtosave);
            File.Delete("temp_history.trh");
            */
            dlhistory history = new dlhistory();
            history.Show();
        }

        private void preferencesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            preferences prefs = new preferences();
            prefs.MdiParent = this;
            prefs.Show();
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form2 about = new Form2();
            about.MdiParent = this;
            about.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void feedBackAndSupportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            feedbackandsupport support = new feedbackandsupport();
            support.MdiParent = this;
            support.Show();
        }
    }
}
