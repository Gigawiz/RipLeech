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
        string plugurl = null;
        string assembly = null;
        string plugindir = @"C:\Program Files\NiCoding\RipLeech\Data\Addons\";
        string installdir = @"C:\Program Files\NiCoding\RipLeech\";
        public Menu()
        {
            InitializeComponent();
        }

        private void Menu_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (File.Exists(@"C:\RipLeech\Temp\newvid.tmp"))
                {
                    File.Delete(@"C:\RipLeech\Temp\newvid.tmp");
                }
                if (File.Exists(@"C:\RipLeech\Temp\newsearch.tmp"))
                {
                    File.Delete(@"C:\RipLeech\Temp\newsearch.tmp");
                }
            }
            catch { }
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

        public bool DirectoryIsEmpty(string path)
        {
            int fileCount = Directory.GetFiles(path).Length;
            if (fileCount > 0)
            {
                return false;
            }

            string[] dirs = Directory.GetDirectories(path);
            foreach (string dir in dirs)
            {
                if (!DirectoryIsEmpty(dir))
                {
                    return false;
                }
            }

            return true;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            label16.Text = RipLeech.Properties.Settings.Default.buildvers;
            if (RipLeech.Properties.Settings.Default.theme == "steamthemes")
            {
                settheme("steamthemes");
                radioButton7.Checked = true;
            }
            else if (RipLeech.Properties.Settings.Default.theme == "nicoding")
            {
                settheme("nicoding");
                radioButton8.Checked = true;
            }
            else
            {
                settheme("default");
                radioButton4.Checked = true;
            }
            try
            {
                string updateurl = "http://nicoding.com/api.php?app=ripleech&update=check";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string pluginsavail = sr.ReadToEnd();
                label18.Text = pluginsavail;
                if (label16.Text != label18.Text)
                {
                    label18.ForeColor = Color.Red;
                }
            }
            catch
            {
                label18.Text = "Unknown";
            }
            if (!Directory.Exists(@"C:\RipLeech\Temp\"))
            {
                Directory.CreateDirectory(@"C:\RipLeech\Temp\");
            }
            else
            {
                if (DirectoryIsEmpty(@"C:\RipLeech\Temp\") == false)
                {
                    Directory.Delete(@"C:\RipLeech\Temp", true);
                    Directory.CreateDirectory(@"C:\RipLeech\Temp\");
                }
            }
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
            try
            {
                string updateurl = "http://nicoding.com/api.php?app=ripleech&plugin=all&info=name";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string pluginsavail = sr.ReadToEnd();
                string[] lines = Regex.Split(pluginsavail, "<br>");
                foreach (string line in lines)
                {
                    listBox3.Items.Add(line);
                }
            }
            catch
            {
                listBox3.Items.Add("Unable to get plugins at this time.");
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
            if (File.Exists(installdir + "updater.temp"))
            {
                if (File.Exists(installdir + "updater.exe"))
                {
                    File.Delete(installdir + "updater.exe");
                    File.Copy(installdir + "updater.temp", "updater.exe");
                    File.Delete(installdir + "updater.temp");
                }
                else
                {
                    File.Copy(installdir + "updater.temp", "updater.exe");
                    File.Delete(installdir + "updater.temp");
                }
            }
            if (!Directory.Exists(plugindir))
            {
                Directory.CreateDirectory(plugindir);
            }
            #region checkforaddons
            try
            {
                int plgcnt = 0;
                string[] fileEntries = Directory.GetFiles(plugindir, "*.dll");
                DirectoryInfo addonnfo = new DirectoryInfo(plugindir);
                if (addonnfo.Exists)
                {
                    if (IsDirectoryEmpty(plugindir) == false)
                    {
                        int i = 0;
                        foreach (string fileName in fileEntries)
                        {
                            assembly = fileName;
                            string totalfilename = Path.GetFileNameWithoutExtension(fileName);
                            Button btnPlay = new Button();
                            btnPlay.Name = totalfilename;
                            plgcnt++;
                            if (i <= 1)
                            {
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
                            else
                            {
                                int btntop = 30 * i;
                                btnPlay.Top = button1.Top + btntop;
                                btnPlay.Left = button1.Left;
                                btnPlay.Width = 189;
                                btnPlay.Height = 23;
                                btnPlay.Text = (btnPlay.Name.ToString());
                                btnPlay.Click += new EventHandler(btnPlay_Click);
                                tabPage2.Controls.Add(btnPlay);
                                btnPlay.BringToFront();
                            }
                            i++;
                        }
                    }
                }
                if (plgcnt > 0)
                {
                    label20.Text = plgcnt.ToString();
                }
                else
                {
                    label20.Text = "0";
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            #endregion
            if (File.Exists(@"C:\RipLeech\Temp\newvid.tmp"))
            {
                youtube yt = new youtube();
                yt.Show();
            }
            if (RipLeech.Properties.Settings.Default.surveyshown == false)
            {
                survey srvy = new survey();
                srvy.Show();
            }
            #region regstuff
            /*
            //here we write the reg file for the protocol.
            string dir = @"C:\RipLeech\Temp\protocol.reg";
            if (RipLeech.Properties.Settings.Default.reginstalled == false)
            {
                if (!File.Exists(dir))
                {
                    System.IO.File.WriteAllText(dir, RipLeech.Properties.Resources.protocol);
                    Process regeditProcess = Process.Start("regedit.exe", "/s " + dir);
                    regeditProcess.WaitForExit();
                    if (System.IO.File.Exists(dir))
                    {
                        System.IO.File.Delete(dir);
                    }
                    RipLeech.Properties.Settings.Default.reginstalled = true;
                    RipLeech.Properties.Settings.Default.Save();
                    restart();
                }
            }
            */
            #endregion
            timer4.Start();
        }
        void restart()
        {
            Process.Start(@"C:\Program Files\NiCoding\RipLeech\RipLeech.exe"); // to start new instance of application
            Environment.Exit(0); //to turn off current app
        }
        void btnPlay_Click(object sender, EventArgs e)
        {
            Button clickedItem = (Button)sender;
            string fileopenloc = plugindir + clickedItem.Name + ".dll";
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
                    string startdir = @"C:\Program Files\NiCoding\RipLeech\";
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
                    string startdir = @"C:\Program Files\NiCoding\RipLeech\";
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
                    string startdir = @"C:\Program Files\NiCoding\RipLeech\";
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

        private void button10_Click(object sender, EventArgs e)
        {
            YoutubeCountryBypasser ycb = new YoutubeCountryBypasser();
            ycb.Show();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessageBox.Show("Loading Addon Data! This may take a second.");
            try
            {
                string plug = listBox3.SelectedItem.ToString();
                string updateurl = "http://nicoding.com/api.php?app=ripleech&plugin=" + plug + "&info=about";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                textBox4.Text = sr.ReadToEnd();
                string plugindl = "http://nicoding.com/api.php?app=ripleech&plugin=" + plug + "&info=url";
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(plugindl);
                WebResponse response2 = request2.GetResponse();
                System.IO.StreamReader sr2 = new System.IO.StreamReader(response2.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                plugurl = sr2.ReadToEnd();
                assembly = plugurl.Substring(plugurl.LastIndexOf('/') + 1);
            }
            catch
            {
                textBox4.Text = "Unable to get details of that plugin!";
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string saveto = plugindir + assembly;
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;
            Uri update = new Uri(plugurl);
            client.DownloadFileAsync(update, saveto);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            MessageBox.Show("Plugin Successfully Downloaded!\r\nPlease restart RipLeech to see your new plugin!");
        }

        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            chatbox chat = new chatbox();
            chat.Show();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                RipLeech.Properties.Settings.Default.theme = "default";
                RipLeech.Properties.Settings.Default.Save();
                settheme("default");
            }
            else if (radioButton7.Checked == true)
            {
                RipLeech.Properties.Settings.Default.theme = "steamthemes";
                RipLeech.Properties.Settings.Default.Save();
                settheme("steamthemes");
            }
            else if (radioButton8.Checked == true)
            {
                RipLeech.Properties.Settings.Default.theme = "nicoding";
                RipLeech.Properties.Settings.Default.Save();
                settheme("nicoding");
            }
            else if (radioButton9.Checked == true)
            {
                RipLeech.Properties.Settings.Default.metro = true;
                RipLeech.Properties.Settings.Default.Save();
                restart();
            }
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                RipLeech.Properties.Settings.Default.theme = "default";
                RipLeech.Properties.Settings.Default.Save();
                settheme("default");
            }
            else if (radioButton7.Checked == true)
            {
                RipLeech.Properties.Settings.Default.theme = "steamthemes";
                RipLeech.Properties.Settings.Default.Save();
                settheme("steamthemes");
            }
            else if (radioButton8.Checked == true)
            {
                RipLeech.Properties.Settings.Default.theme = "nicoding";
                RipLeech.Properties.Settings.Default.Save();
                settheme("nicoding");
            }
            else if (radioButton9.Checked == true)
            {
                RipLeech.Properties.Settings.Default.metro = true;
                RipLeech.Properties.Settings.Default.Save();
                restart();
            }
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                RipLeech.Properties.Settings.Default.theme = "default";
                RipLeech.Properties.Settings.Default.Save();
                settheme("default");
            }
            else if (radioButton7.Checked == true)
            {
                RipLeech.Properties.Settings.Default.theme = "steamthemes";
                RipLeech.Properties.Settings.Default.Save();
                settheme("steamthemes");
            }
            else if (radioButton8.Checked == true)
            {
                RipLeech.Properties.Settings.Default.theme = "nicoding";
                RipLeech.Properties.Settings.Default.Save();
                settheme("nicoding");
            }
            else if (radioButton9.Checked == true)
            {
                RipLeech.Properties.Settings.Default.metro = true;
                RipLeech.Properties.Settings.Default.Save();
                restart();
            }
        }
        private void settheme(string name)
        {
            if (name == "steamthemes")
            {
                this.BackgroundImage = RipLeech.Properties.Resources.bg1;
                tabControl1.TabPages[0].BackgroundImage = RipLeech.Properties.Resources.bg1;
                tabControl1.TabPages[1].BackgroundImage = RipLeech.Properties.Resources.bg1;
                tabControl1.TabPages[2].BackgroundImage = RipLeech.Properties.Resources.bg1;
                tabControl1.TabPages[3].BackgroundImage = RipLeech.Properties.Resources.bg1;
                tabControl1.TabPages[4].BackgroundImage = RipLeech.Properties.Resources.bg1;
                tabControl1.TabPages[5].BackgroundImage = RipLeech.Properties.Resources.bg1;
                tabControl1.TabPages[6].BackgroundImage = RipLeech.Properties.Resources.bg1;
                tabControl1.TabPages[7].BackgroundImage = RipLeech.Properties.Resources.bg1;
                pictureBox1.Image = RipLeech.Properties.Resources.steamthemes_theme_logo;
            }
            else if (name == "nicoding")
            {
                this.BackgroundImage = RipLeech.Properties.Resources.bg3;
                tabControl1.TabPages[0].BackgroundImage = RipLeech.Properties.Resources.bg3;
                tabControl1.TabPages[1].BackgroundImage = RipLeech.Properties.Resources.bg3;
                tabControl1.TabPages[2].BackgroundImage = RipLeech.Properties.Resources.bg3;
                tabControl1.TabPages[3].BackgroundImage = RipLeech.Properties.Resources.bg3;
                tabControl1.TabPages[4].BackgroundImage = RipLeech.Properties.Resources.bg3;
                tabControl1.TabPages[5].BackgroundImage = RipLeech.Properties.Resources.bg3;
                tabControl1.TabPages[6].BackgroundImage = RipLeech.Properties.Resources.bg3;
                tabControl1.TabPages[7].BackgroundImage = RipLeech.Properties.Resources.bg3;
                pictureBox1.Image = RipLeech.Properties.Resources.ripleech_nic_logo;
            }
            else if (name == "default")
            {
                this.BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[0].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[1].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[2].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[3].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[4].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[5].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[6].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[7].BackgroundImage = RipLeech.Properties.Resources.bg;
                pictureBox1.Image = RipLeech.Properties.Resources.logo;
            }
            else
            {
                this.BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[0].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[1].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[2].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[3].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[4].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[5].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[6].BackgroundImage = RipLeech.Properties.Resources.bg;
                tabControl1.TabPages[7].BackgroundImage = RipLeech.Properties.Resources.bg;
                pictureBox1.Image = RipLeech.Properties.Resources.logo;
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
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
                int thisbuild = RipLeech.Properties.Settings.Default.progvers;
                if (build > thisbuild)
                {
                    updatefound upd = new updatefound();
                    upd.Show();
                    timer4.Stop();
                }
            }
            catch
            {
                timer4.Stop();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.audiosavepath))
            {
                string mymusic = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
                RipLeech.Properties.Settings.Default.audiosavepath = mymusic;
                RipLeech.Properties.Settings.Default.Save();
                textBox2.Text = mymusic;
            }
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
            {
                string myvids = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                RipLeech.Properties.Settings.Default.videosavepath = myvids;
                RipLeech.Properties.Settings.Default.Save();
                textBox1.Text = myvids;
            }
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked == true)
            {
                RipLeech.Properties.Settings.Default.theme = "default";
                RipLeech.Properties.Settings.Default.Save();
                settheme("default");
            }
            else if (radioButton7.Checked == true)
            {
                RipLeech.Properties.Settings.Default.theme = "steamthemes";
                RipLeech.Properties.Settings.Default.Save();
                settheme("steamthemes");
            }
            else if (radioButton8.Checked == true)
            {
                RipLeech.Properties.Settings.Default.theme = "nicoding";
                RipLeech.Properties.Settings.Default.Save();
                settheme("nicoding");
            }
            else if (radioButton9.Checked == true)
            {
                RipLeech.Properties.Settings.Default.metro = true;
                RipLeech.Properties.Settings.Default.Save();
                restart();
            }
        }
    }
}
