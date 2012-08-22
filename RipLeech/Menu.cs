using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace RipLeech
{
    public partial class Menu : Form
    {
        string assembly = "";
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            RipLeech.Properties.Settings.Default.loggedinuser = "";
            RipLeech.Properties.Settings.Default.ytqueue = "";
            RipLeech.Properties.Settings.Default.Save();
            notifyIcon1.Dispose();
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            youtube yt = new youtube();
            yt.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            md5engine md5 = new md5engine();
            string pass = md5.EncodePassword(transparentLabel3.Text).ToLower();
            string compare = "http://nicoding.com/api.php?app=ripleech&user=" + transparentLabel1.Text + "&info=password"; 
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(compare);
            string storedpass = null;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                storedpass = reader.ReadToEnd();
            }
            if (storedpass == pass)
            {
                MessageBox.Show("Successfully Logged in!");
                RipLeech.Properties.Settings.Default.loggedinuser = transparentLabel1.Text;
                RipLeech.Properties.Settings.Default.Save();
                label1.Text = transparentLabel1.Text;
                string acctype = "http://nicoding.com/api.php?app=ripleech&user=" + transparentLabel1.Text + "&info=ispaid";
                WebClient acc = new WebClient();
                System.IO.Stream streamlol = web.OpenRead(acctype);
                using (System.IO.StreamReader acctypereader = new System.IO.StreamReader(streamlol))
                {
                    string accountype = acctypereader.ReadToEnd();
                    if (accountype == "no")
                    {
                        label2.Text = "Free";
                    }
                    else
                    {
                        label2.Text = "Monthly";
                    }
                }
                groupBox1.Visible = true;
                groupBox2.Visible = false;
            }
            else
            {
                pictureBox5.Visible = true;
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            pictureBox5.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            md5engine md5 = new md5engine();
            string pass = md5.EncodePassword(transparentLabel3.Text).ToLower();
            InputBoxValidation validation = delegate(string val)
            {
                if (val == "")
                    return "Value cannot be empty.";
                if (!(new Regex(@"^[a-zA-Z0-9_\-\.]+@[a-zA-Z0-9_\-\.]+\.[a-zA-Z]{2,}$")).IsMatch(val))
                    return "Email address is not valid.";
                return "";
            };

            string value = "info@example.com";
            if (InputBox.Show("Enter your email address", "Email address:", ref value, validation) == DialogResult.OK)
            {
                string email = value;
                string compare = "http://nicoding.com/api.php?app=ripleech&newuser="+transparentLabel1.Text+"&pass="+transparentLabel3.Text+"&email="+email;
                WebClient web = new WebClient();
                System.IO.Stream stream = web.OpenRead(compare);
                string result = null;
                using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
                if (result == "Error! User already exists!")
                {
                    string message = "Error! That user allready exists! Did you forget your password?";
                    string caption = "User Exists";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult error;
                    error = MessageBox.Show(message, caption, buttons);

                    if (error == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.Close();
                    }
                }
                else if (result == "User info updated successfully")
                {
                    MessageBox.Show("You have Successfully registered! You may now login with your username and password.", "Registration Successful!");
                }
                else
                {
                    MessageBox.Show("There seems to be an error with the user system at the present time. Please try again later.", "System Error");
                }
            }

        }

        private void Menu_Load(object sender, EventArgs e)
        {
            //timer2.Start();
            try
            {
                string updateurl = "https://dl.dropbox.com/u/22054429/RipLeech/changelog.txt";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                textBox3.Text = sr.ReadToEnd();
            }
            catch
            {
                textBox3.Text = "Unable to get changelog at this time.";
            }
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
            {
                textBox1.Text = RipLeech.Properties.Settings.Default.videosavepath;
            }
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.audiosavepath))
            {
                 textBox2.Text = RipLeech.Properties.Settings.Default.audiosavepath;
            }
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.audioquality))
            {
                if (RipLeech.Properties.Settings.Default.audioquality == "128")
                {
                    radioButton1.Checked = true;
                }
                else if (RipLeech.Properties.Settings.Default.audioquality == "256")
                {
                    radioButton2.Checked = true;
                }
                else if (RipLeech.Properties.Settings.Default.audioquality == "320")
                {
                    radioButton3.Checked = true;
                }
            }
            if (RipLeech.Properties.Settings.Default.betaupdates == true)
            {
                checkBox1.Checked = true;
            }
            if (File.Exists("updater.temp"))
            {
                if (File.Exists("updater.exe"))
                {
                    File.Delete("updater.exe");
                }
                File.Copy("updater.temp", "updater.exe");
                File.Delete("updater.temp");
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
                        int i = 0;
                        foreach (string fileName in fileEntries)
                        {
                            assembly = fileName;
                            string totalfilename = Path.GetFileNameWithoutExtension(fileName);
                            Button btnPlay = new Button();
                            btnPlay.Name = totalfilename;
                            btnPlay.Top = button1.Top + 30;
                            btnPlay.Left = button1.Left;
                            btnPlay.Width = 189;
                            btnPlay.Height = 23;
                            btnPlay.Text = (btnPlay.Name.ToString());
                            btnPlay.Click += new EventHandler(btnPlay_Click);
                            tabPage2.Controls.Add(btnPlay);
                            btnPlay.BringToFront();
                            i++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }

        void btnPlay_Click(object sender, EventArgs e)
        {
            Button clickedItem = (Button)sender;
            string fileopenloc = Directory.GetCurrentDirectory() + @"\Data\Addons\" + clickedItem.Name + ".dll";
            string formname = clickedItem.Name + ".Form1";
            System.Reflection.Assembly extAssembly = System.Reflection.Assembly.LoadFrom(fileopenloc);
            Form extForm = (Form)extAssembly.CreateInstance(formname, true);
            this.AddOwnedForm(extForm);
            extForm.Show();
        }
        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD1 = new FolderBrowserDialog();
            FBD1.ShowNewFolderButton = true;
            FBD1.Description = "Choose a location to save the Video files downloaded by RipLeech";
            FBD1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            DialogResult result = FBD1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = FBD1.SelectedPath;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD1 = new FolderBrowserDialog();
            FBD1.ShowNewFolderButton = true;
            FBD1.Description = "Choose a location to save the Audio files downloaded by RipLeech";
            FBD1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            DialogResult result = FBD1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.Text = FBD1.SelectedPath;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                RipLeech.Properties.Settings.Default.videosavepath = textBox1.Text;
            }
            if (!String.IsNullOrEmpty(textBox2.Text))
            {
                RipLeech.Properties.Settings.Default.audiosavepath = textBox2.Text;
            }
            if (radioButton1.Checked == true)
            {
                RipLeech.Properties.Settings.Default.audioquality = "128";
            }
            else if (radioButton2.Checked == true)
            {
                RipLeech.Properties.Settings.Default.audioquality = "256";
            }
            else if (radioButton3.Checked == true)
            {
                RipLeech.Properties.Settings.Default.audioquality = "320";
            }
            else
            {
                RipLeech.Properties.Settings.Default.audioquality = "320";
            }
            if (checkBox1.Checked == true)
            {
                RipLeech.Properties.Settings.Default.betaupdates = true;
            }
            if (radioButton5.Checked == true)
            {
                RipLeech.Properties.Settings.Default.ffmpeg = false;
            }
            else if (radioButton6.Checked == true)
            {
                RipLeech.Properties.Settings.Default.ffmpeg = true;
            }
            else
            {
                RipLeech.Properties.Settings.Default.ffmpeg = true;
            }
            RipLeech.Properties.Settings.Default.Save();
        }
        #region ffmpeg tools

        private void button7_Click(object sender, EventArgs e)
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

        private void button8_Click(object sender, EventArgs e)
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

        private void button9_Click(object sender, EventArgs e)
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

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/flagbug/YoutubeExtractor");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://ffmpeg.org");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.ytqueue))
            {
                timer3.Start();
                timer2.Stop();
            }
        }
        private void timer3_Tick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.ytqueue))
            {
                timer3.Stop();
                timer2.Start();
            }
            else
            {
                string services = RipLeech.Properties.Settings.Default.ytqueue;
                if (!String.IsNullOrEmpty(services))
                {
                    listBox2.Items.Clear();
                    string[] split = services.Split(',');

                    foreach (string item in split)
                    {
                        listBox2.Items.Add(item);
                    }
                }
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void minimizeToTrayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            minimizeToTrayToolStripMenuItem.Visible = false;
            restoreToolStripMenuItem.Visible = true;
            notifyIcon1.ShowBalloonTip(500, "Minimized to Tray", "RipLeech has been minimized to the tray and will continue running in the background", ToolTipIcon.Info);
            this.Hide();
        }

        private void restoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            restoreToolStripMenuItem.Visible = false;
            minimizeToTrayToolStripMenuItem.Visible = true;
        }

        private void Menu_Resize(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                if (FormWindowState.Minimized == this.WindowState)
                {
                    minimizeToTrayToolStripMenuItem.Visible = false;
                    restoreToolStripMenuItem.Visible = true;
                    notifyIcon1.ShowBalloonTip(500, "Minimized to Tray", "RipLeech has been minimized to the tray and will continue running in the background", ToolTipIcon.Info);
                    this.Hide();
                }
            }
        }
    }
}
