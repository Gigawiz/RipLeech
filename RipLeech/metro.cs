using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using DevComponents.DotNetBar;
using DevComponents.WinForms;
using DevComponents.UI;
using System.Resources;
using System.Reflection;
using DevComponents.Instrumentation;
using Google.YouTube;
using Google.GData.Client;
using Google.GData.YouTube;
using YoutubeExtractor;

namespace RipLeech
{

    public partial class metro : DevComponents.DotNetBar.Metro.MetroAppForm
    {
        bool speedmeter = true;
        int done = 0;
        int count = 0;
        string plugurl = null;
        string assembly = null;
        string filedling = null;
        string tmpdir = @"C:\RipLeech\Temp\";
        string plugindir = @"C:\Program Files\NiCoding\RipLeech\Data\Addons\";
        string installdir = @"C:\Program Files\NiCoding\RipLeech\";
        string vidid2 = null;
        string root = @"C:\RipLeech\Temp\";
        string ffmpeg = @"C:\Program Files\NiCoding\RipLeech";
        string viddling = "";
        string vidout = "";
        string mp4out = "";
        string vidname = null;
        YouTubeRequestSettings settings = new YouTubeRequestSettings("RipLeech", "AI39si5SxBrG0x12TqlsrGpnsoCcqgV9-diBgRS5xOhcDB01sSH6WVSTJjhlQenMSt4qH_UC87Y8kqYaf4Ykgw-poTZ7yF2zDw");
        public metro()
        {
            InitializeComponent();
            #region checkforaddons
            try
            {
                int plgcnt = 2;
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
                            DevComponents.DotNetBar.Metro.MetroTileItem itemplug = new DevComponents.DotNetBar.Metro.MetroTileItem();
                            itemplug.Name = totalfilename;
                            try
                            {
                                Resources _r = new Resources(Assembly.LoadFile(assembly));
                                System.Drawing.Bitmap s = _r.Bitmaps["MetroIcon"];
                                if (s != null)
                                {
                                    itemplug.TileStyle.BackgroundImage = _r.Bitmaps["MetroIcon"];
                                    itemplug.TileStyle.BackgroundImagePosition = eStyleBackgroundImage.Stretch;
                                    itemplug.Refresh();
                                }
                                else
                                {
                                    itemplug.Text = itemplug.Name.ToString();
                                }
                            }
                            catch (Exception ex) { }
                            itemplug.Click += new EventHandler(itemplug_Click);
                            itemContainer3.SubItems.Add(itemplug);
                            plgcnt++;
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
                itemContainer3.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            #endregion
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
        public bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private void metro_Load(object sender, EventArgs e)
        {
            label16.Text = RipLeech.Properties.Settings.Default.buildvers;
            timer2.Start();
            switchButton1.Value = true;
            switchButton2.Value = true;
            if (File.Exists(installdir + "updater.temp"))
            {
                if (File.Exists(installdir + "updater.exe"))
                {
                    File.Delete(installdir + "updater.exe");
                }
                File.Copy(installdir + "updater.temp", installdir + "updater.exe");
                File.Delete(installdir + "updater.temp");
            }
            try
            {
                string updateurl = "https://dl.dropbox.com/u/22054429/RipLeech/news.txt";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string pluginsavail = sr.ReadToEnd();
                textBox10.Text = pluginsavail;
            }
            catch
            {
                textBox10.Text = "Unable to get News at this time.";
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
                    if (!File.Exists(@"C:\RipLeech\Temp\newvid.tmp"))
                    {
                        Directory.Delete(@"C:\RipLeech\Temp", true);
                        Directory.CreateDirectory(@"C:\RipLeech\Temp\");
                    }
                }
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
            if (File.Exists(@"C:\RipLeech\Temp\newvid.tmp"))
            {
                //parse youtube queue here
                timer4.Start();
            }
            if (RipLeech.Properties.Settings.Default.surveyshown == false)
            {
                survey srvy = new survey();
                srvy.Show();
            }
            timer5.Start();
        }
        void itemplug_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.Metro.MetroTileItem clickedItem = (DevComponents.DotNetBar.Metro.MetroTileItem)sender;
            string fileopenloc = plugindir + clickedItem.Name + ".dll";
            string formname = clickedItem.Name + ".Form1";
            System.Reflection.Assembly extAssembly = System.Reflection.Assembly.LoadFrom(fileopenloc);
            Form extForm = (Form)extAssembly.CreateInstance(formname, true);
            this.AddOwnedForm(extForm);
            extForm.Show();
        }

        private void buttonItem5_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://wexplain.com");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://stackoverflow.com");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://ffmpeg.org");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/flagbug/YoutubeExtractor");
        }

        private void metroTileItem27_Click(object sender, EventArgs e)
        {
            timer1.Start();
            youtube yt = new youtube();
            yt.Show();
        }

        private void superTabItem2_Click(object sender, EventArgs e)
        {
            try
            {
                string updateurl = "http://nicoding.com/api.php?app=ripleech&trending=list";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string pluginsavail = sr.ReadToEnd();
                string[] lines = Regex.Split(pluginsavail, "<br>");
                foreach (string line in lines)
                {
                    listBox2.Items.Add(line);
                }
            }
            catch
            {
                listBox2.Items.Add("Unable to get Trending Videos at this time.");
            }
            try
            {
                string updateurl = "http://nicoding.com/api.php?app=ripleech&trending=searches";
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
                listBox3.Items.Add("Unable to get Trending Searches at this time.");
            }
        }

        private void metroTileItem28_Click(object sender, EventArgs e)
        {
            chatbox chat = new chatbox();
            chat.Show();
        }

        private void metroTabItem6_Click(object sender, EventArgs e)
        {
            if (metroTabItem3.Visible == true)
            {
                metroTabItem3.Visible = false;
                metroShell1.Refresh();
            }
            if (itemContainer1.SubItems.Count == 0)
            {
                timer1.Start();
                try
                {
                    string updateurl = "http://nicoding.com/api.php?app=ripleech&plugin=all&info=name";
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                    WebResponse response = request.GetResponse();
                    System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                    string pluginsavail = sr.ReadToEnd();
                    string[] lines = Regex.Split(pluginsavail, "<br>");
                    itemContainer1.SubItems.Clear();
                    int currentline = 0;
                    int totallines = lines.Count() - 1;
                    foreach (string line in lines)
                    {
                        if (currentline != totallines)
                        {
                            DevComponents.DotNetBar.Metro.MetroTileItem itemplugdl = new DevComponents.DotNetBar.Metro.MetroTileItem();
                            itemplugdl.Name = line;
                            itemplugdl.Text = (itemplugdl.Name.ToString());
                            itemplugdl.Click += new EventHandler(itemplugdl_Click);
                            itemContainer1.SubItems.Add(itemplugdl);
                            currentline++;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Unable to get plugins at this time.");
                    metroTabItem6.Visible = false;
                }
            }
        }
        void itemplugdl_Click(object sender, EventArgs e)
        {
            DevComponents.DotNetBar.Metro.MetroTileItem clickedItem = (DevComponents.DotNetBar.Metro.MetroTileItem)sender;
            //string fileopenloc = plugindir + clickedItem.Name + ".dll";
            metroTabItem3.Visible = true;
            metroShell1.Refresh();
            try
            {
                string plug = RipLeech.Properties.Settings.Default.metro_plugin;
                string updateurl = "http://nicoding.com/api.php?app=ripleech&plugin=" + plug + "&info=about";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                textBoxX1.Text = sr.ReadToEnd();

                string plugindl = "http://nicoding.com/api.php?app=ripleech&plugin=" + plug + "&info=url";
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(plugindl);
                WebResponse response2 = request2.GetResponse();
                System.IO.StreamReader sr2 = new System.IO.StreamReader(response2.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                plugurl = sr2.ReadToEnd();
                assembly = plugurl.Substring(plugurl.LastIndexOf('/') + 1);
            }
            catch
            {
                textBoxX1.Text = "Unable to get details of that plugin!";
            }
            metroTabItem3.Select();
            RipLeech.Properties.Settings.Default.metro_plugin = clickedItem.Name;
            RipLeech.Properties.Settings.Default.Save();
        }

        private void buttonX1_Click(object sender, EventArgs e)
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
            progressBarItem1.Value = e.ProgressPercentage;
        }

        private void superTabItem1_Click(object sender, EventArgs e)
        {
            string services = "Test Plugin 1_Test Plugin 2_Test Plugin 3";
            listBox4.Items.Clear();
            if (!String.IsNullOrEmpty(services))
            {
                string[] split = services.Split('_');

                foreach (string item in split)
                {
                    listBox4.Items.Add(item);
                }
            }
        }

        private void superTabItem6_Click_1(object sender, EventArgs e)
        {
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
        }

        private void metro_FormClosing(object sender, FormClosingEventArgs e)
        {
            //closing form :D
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.loggedinuser))
            {
                string user = RipLeech.Properties.Settings.Default.loggedinuser;
                int downloads = RipLeech.Properties.Settings.Default.videosdownloaded;
                int views = RipLeech.Properties.Settings.Default.videoswatched;
                string compare = "http://nicoding.com/api.php?app=ripleech&updateuser=" + user + "&update=both&downloads=" + downloads + "&views=" + views;
                WebClient web = new WebClient();
                System.IO.Stream stream = web.OpenRead(compare);
                using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                }
            }
            if (File.Exists(tmpdir + @"\queue.rlqueue"))
            {
                File.Delete(tmpdir + @"\queue.rlqueue");
            }
            Environment.Exit(0);
        }
        int ticks = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            circularProgress1.Visible = true;
            if (circularProgress1.Value == 100)
            {
                if (ticks == 5)
                {
                    timer1.Stop();
                    circularProgress1.Visible = false;
                }
                else
                {
                    circularProgress1.Value = 0;
                    ticks++;
                }

            }
            else
            {
                circularProgress1.Value += 10;
            }
        }
        Stopwatch sw = new Stopwatch();
        private void switchButton1_ValueChanged(object sender, EventArgs e)
        {
            if (switchButton1.Value == true)
            {
                speedmeter = true;
            }
            else
            {
                speedmeter = false;
            }
        }
        private Queue<string> _downloadUrls = new Queue<string>();
        private Queue<string> _downloadNames = new Queue<string>();
        private Queue<string> _downloadTypes = new Queue<string>();
        bool found2 = false;
        bool isdownloading = false;
        string curntdl = null;
        string ytfilename = null;
        private void addtoqueue(string url, string format)
        {
            _downloadUrls.Enqueue(url);
            listBox5.Items.Add(url);
            if (isdownloading == false && _downloadUrls.Any())
            {
                isdownloading = true;
                DownloadFile();
            }
            count++;
        }
        string saveloc = @"C:\RipLeech\Temp\";
        private void RepoveDuplicate()
        {
            for (Int16 Row = 0; Row <= listBox5.Items.Count - 2; Row++)
            {
                for (int RowAgain = listBox5.Items.Count - 1; RowAgain >= Row + 1; RowAgain += -1)
                {
                    if (listBox5.Items[Row].ToString() == listBox5.Items[RowAgain].ToString())
                    {
                        listBox5.Items.RemoveAt(RowAgain);
                    }
                }
            }
        }
        private void RepoveDuplicateUpdates()
        {
            for (Int16 Row = 0; Row <= listBox8.Items.Count - 2; Row++)
            {
                for (int RowAgain = listBox8.Items.Count - 1; RowAgain >= Row + 1; RowAgain += -1)
                {
                    if (listBox8.Items[Row].ToString() == listBox8.Items[RowAgain].ToString())
                    {
                        listBox8.Items.RemoveAt(RowAgain);
                    }
                }
            }
        }
        private void DownloadFile()
        {
            if (_downloadUrls.Any())
            {
                if (!Directory.Exists(tmpdir))
                {
                    Directory.CreateDirectory(tmpdir);
                }
                RepoveDuplicate();
                var url = _downloadUrls.Dequeue();
                #region get quality
                try
                {
                    int tempvidsdled = RipLeech.Properties.Settings.Default.videosdownloaded;
                    RipLeech.Properties.Settings.Default.videosdownloaded = tempvidsdled + 1;
                    RipLeech.Properties.Settings.Default.Save();
                    // Our youtube link
                    /*
                     * Get the available video formats.
                     * We'll work with them in the video and audio download examples.
                     */
                    IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(url);
                    try
                    {
                        VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 1080);
                        if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
                        {
                            mp4out = RipLeech.Properties.Settings.Default.videosavepath + @"\" + video.Title + ".mp4";
                        }
                        else
                        {
                            mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                        }
                        _downloadNames.Enqueue(video.Title);
                        string saveto = tmpdir + video.Title + video.VideoExtension;
                        string viduri = video.DownloadUrl;
                        filedling = video.Title + video.VideoExtension;
                        WebClient client2 = new WebClient();
                        client2.DownloadProgressChanged += client2_DownloadProgressChanged;
                        client2.DownloadFileCompleted += client2_DownloadFileCompleted;
                        client2.DownloadFileAsync(new Uri(viduri), saveto);
                    }
                    catch
                    {
                        try
                        {
                            VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 720);
                            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
                            {
                                mp4out = RipLeech.Properties.Settings.Default.videosavepath + @"\" + video.Title + ".mp4";
                            }
                            else
                            {
                                mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                            }
                            _downloadNames.Enqueue(video.Title);
                            string saveto = tmpdir + video.Title + video.VideoExtension;
                            string viduri = video.DownloadUrl;
                            filedling = video.Title + video.VideoExtension;
                            WebClient client2 = new WebClient();
                            client2.DownloadProgressChanged += client2_DownloadProgressChanged;
                            client2.DownloadFileCompleted += client2_DownloadFileCompleted;
                            client2.DownloadFileAsync(new Uri(viduri), saveto);
                        }
                        catch
                        {
                            try
                            {
                                VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 480);
                                if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
                                {
                                    mp4out = RipLeech.Properties.Settings.Default.videosavepath + @"\" + video.Title + ".mp4";
                                }
                                else
                                {
                                    mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                                }
                                _downloadNames.Enqueue(video.Title);
                                string saveto = tmpdir + video.Title + video.VideoExtension;
                                string viduri = video.DownloadUrl;
                                filedling = video.Title + video.VideoExtension;
                                WebClient client2 = new WebClient();
                                client2.DownloadProgressChanged += client2_DownloadProgressChanged;
                                client2.DownloadFileCompleted += client2_DownloadFileCompleted;
                                client2.DownloadFileAsync(new Uri(viduri), saveto);
                            }
                            catch
                            {
                                try
                                {
                                    VideoInfo video = videoInfos.First(info => info.VideoType == VideoType.Mp4 && info.Resolution == 360);
                                    if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
                                    {
                                        mp4out = RipLeech.Properties.Settings.Default.videosavepath + @"\" + video.Title + ".mp4";
                                    }
                                    else
                                    {
                                        mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                                    }
                                    _downloadNames.Enqueue(video.Title);
                                    string saveto = tmpdir + video.Title + video.VideoExtension;
                                    string viduri = video.DownloadUrl;
                                    filedling = video.Title + video.VideoExtension;
                                    WebClient client2 = new WebClient();
                                    client2.DownloadProgressChanged += client2_DownloadProgressChanged;
                                    client2.DownloadFileCompleted += client2_DownloadFileCompleted;
                                    client2.DownloadFileAsync(new Uri(viduri), saveto);
                                }
                                catch
                                {
                                    MessageBox.Show("There has been an error trying to get the maximum quality! Please report the bug to Ripleech' Developers!");
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("It seems that there has been an error trying to get the Video ID. please try re-entering the id. If the problem persists, submit an issue on https://github.com/NiCoding/RipLeech/issues/new");
                }
                return;
                #endregion

            }
            done = count;
            // End of the download
            isdownloading = false;
            progressBarX2.Value = 100;
            listBox5.Items.Clear();
            gaugeControl1.SetPointerValue("Pointer1", 0);
        }
        static string GetParentUriString(Uri uri)
        {
            return uri.AbsoluteUri.Remove(uri.AbsoluteUri.Length - uri.Segments.Last().Length);
        }
        void client2_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBarX1.Value = e.ProgressPercentage;
            double totalBytescheck = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = ((double)done / (double)count) * (double)100;
            progressBarX2.Value = int.Parse(Math.Truncate(percentage).ToString());
            try
            {
                if (speedmeter == true)
                {
                    // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
                    double speed = (Convert.ToDouble(e.BytesReceived) / 1024);
                    gaugeControl1.SetPointerValue("Pointer1", speed);
                    gaugeControl1.Enabled = true;
                }
                else
                {
                    gaugeControl1.SetPointerValue("Pointer1", 0);
                    gaugeControl1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void client2_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null) { throw e.Error; }
            if (e.Cancelled) { }
            done++;
            string filenamez = _downloadNames.Dequeue();
            string filetypez = _downloadTypes.Dequeue();
            try
            {
                if (!String.IsNullOrEmpty(filenamez))
                {
                    if (filetypez == "video")
                    {
                        if (File.Exists(tmpdir + @"\" + filenamez + ".mp4"))
                        {
                            File.Copy(tmpdir + @"\" + filenamez + ".mp4", RipLeech.Properties.Settings.Default.videosavepath + @"\" + filenamez + ".mp4");
                        }
                        listBox6.Items.Add(RipLeech.Properties.Settings.Default.videosavepath + @"\" + filenamez + ".mp4");
                    }
                    else if (filetypez == "audio")
                    {
                        if (File.Exists(tmpdir + @"\" + filenamez + ".mp4"))
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
                            psi.Arguments = string.Format("-i \"{0}\" -vn -y -f mp3 -ab {2}k \"{1}\"", tmpdir + @"\" + filenamez + ".mp4", RipLeech.Properties.Settings.Default.audiosavepath + @"\" + filenamez + ".mp3", bitrate);
                            psi.WindowStyle = ProcessWindowStyle.Normal;
                            Process p = Process.Start(psi);
                            p.WaitForExit();
                            if (p.HasExited == true)
                            {
                                MessageBox.Show("Conversion Complete!");
                            }
                        }
                        listBox6.Items.Add(RipLeech.Properties.Settings.Default.audiosavepath + @"\" + filenamez + ".mp3");

                    }
                    else
                    {
                        MessageBox.Show("Error!");
                    }
                }
                else
                {
                    MessageBox.Show("error!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DownloadFile();
        }

        private void metroTabItem4_Click(object sender, EventArgs e)
        {
            //betacheckupdate();
            //button1.Enabled = false;
        }
        private void betacheckupdate()
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
                    labelItem1.Text = "Update in progress...";
                    metroStatusBar1.Refresh();
                    betagetupdate();
                }
                else
                {
                    labelItem1.Text = "You have the latest build of RipLeech!";
                    metroStatusBar1.Refresh();
                }
            }
            catch
            {
                labelItem1.Text = "Unable to connect to update server!";
                metroStatusBar1.Refresh();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            betacheckupdate();
        }
        private Queue<string> _updateUrls = new Queue<string>();
        string installfolder2 = @"C:\Program Files\NiCoding\RipLeech\Temp\";
        string filedling2 = null;
        int count2 = 0;
        int done2 = 0;
        double totalBytes2 = 0;
        double bytesIn2 = 0;
        private void betagetupdate()
        {
            string newsurl = "https://dl.dropbox.com/u/22054429/RipLeech/update_news.txt";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(newsurl);
                WebResponse response = request.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                textBox4.Text = sr.ReadToEnd();
                string updateurl = null;
                if (System.IO.File.Exists(installdir + "beta.lock"))
                {
                    updateurl = "https://dl.dropbox.com/u/22054429/RipLeech/beta_filelist.txt";
                }
                else if (System.IO.File.Exists(installdir + "update.lock"))
                {
                    updateurl = "https://dl.dropbox.com/u/22054429/RipLeech/update_filelist.txt";
                }
                else
                {
                    updateurl = "https://dl.dropbox.com/u/22054429/RipLeech/filelist.txt";
                }
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(updateurl);
                WebResponse response2 = request2.GetResponse();
                System.IO.StreamReader sr2 = new System.IO.StreamReader(response2.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string test = sr2.ReadToEnd();
                string[] parts = test.Split('\r');
                IEnumerable<string> urls = parts;
                foreach (string url in urls)
                {
                    Match match = Regex.Match(url, @"([A-Za-z0-9\-]+)\.([A-Za-z0-9\-]+)$",
                    RegexOptions.IgnoreCase);

                    // Here we check the Match instance.
                    if (match.Success)
                    {
                        listBox8.Items.Add(url);
                        _updateUrls.Enqueue(url);
                        count2++;
                    }
                }
                //download updates.
                DownloadUpdate();

            }
            catch
            {
                textBox4.Text = "Unable to get update news at this time.";
                DownloadUpdate();

            }
        }
        private void DownloadUpdate()
        {
            if (_updateUrls.Any())
            {
                RepoveDuplicateUpdates();
                WebClient client3 = new WebClient();
                client3.DownloadProgressChanged += client3_DownloadProgressChanged;
                client3.DownloadFileCompleted += client3_DownloadFileCompleted;

                var url = _updateUrls.Dequeue();
                string FileName = url.Substring(url.LastIndexOf("/") + 1,
                            (url.Length - url.LastIndexOf("/") - 1));
                string filepath = GetParentUriString(new Uri(url));
                string uril2 = filepath.Replace("https://dl.dropbox.com/u/22054429/RipLeech/", installfolder2).Replace("/", @"\");
                if (!String.IsNullOrEmpty(uril2))
                {
                    if (!Directory.Exists(uril2))
                    {
                        Directory.CreateDirectory(uril2);
                    }
                }
                curntupd = url;
                filedling = FileName;
                client3.DownloadFileAsync(new Uri(url), uril2 + FileName);
                return;
            }
            done2 = count2;
            progressBarX3.Value = 100;
            progressBarItem1.Value = 100;
            listBox8.Items.Clear();
            // End of the download
            Process.Start(installdir + @"updater.exe");
            Environment.Exit(0);
        }
        string curntupd = null;
        private void client3_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null) { throw e.Error; }
            if (e.Cancelled) { }
            bool found = false;
            foreach (string LVI1 in listBox7.Items)
            {
                if (LVI1 == curntupd)
                { found = true; }
            }

            if (!found)
            {
                listBox7.Items.Add(curntupd);
            }
            done2++;
            DownloadUpdate();
        }

        void client3_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBarX4.Value = e.ProgressPercentage;
            label2.Text = e.ProgressPercentage.ToString() + "%";
            double bytesIn = double.Parse(e.BytesReceived.ToString());
            string stripped = bytesIn.ToString().Split('.')[0];
            label34.Text = filedling;
            label33.Text = stripped + "Bytes";
            double percentage = ((double)done2 / (double)count2) * (double)100;
            progressBarX3.Value = int.Parse(Math.Truncate(percentage).ToString());
            progressBarItem1.Value = int.Parse(Math.Truncate(percentage).ToString());
            try
            {
                if (speedmeter == true)
                {
                    // Update the label with how much data have been downloaded so far and the total size of the file we are currently downloading
                    double speed = (Convert.ToDouble(e.BytesReceived) / 1024);
                    gaugeControl2.SetPointerValue("Pointer1", speed);
                    gaugeControl2.Enabled = true;
                }
                else
                {
                    gaugeControl2.SetPointerValue("Pointer1", 0);
                    gaugeControl2.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            md5engine md5 = new md5engine();
            string pass = md5.EncodePassword(textBox6.Text).ToLower();
            string compare = "http://nicoding.com/api.php?app=ripleech&user=" + textBox5.Text + "&info=password";
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(compare);
            string storedpass = null;
            using (System.IO.StreamReader reader = new System.IO.StreamReader(stream))
            {
                storedpass = reader.ReadToEnd();
            }
            if (storedpass == pass)
            {
                RipLeech.Properties.Settings.Default.loggedinuser = textBox5.Text;
                RipLeech.Properties.Settings.Default.Save();
                label1.Text = textBox5.Text;
                string acctype = "http://nicoding.com/api.php?app=ripleech&user=" + textBox5.Text + "&info=ispaid";
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
                timer3.Start();
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            pictureBox5.Visible = false;
            timer3.Stop();
            metroShell1.Refresh();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            label38.Text = "Related Videos:";
            listView1.Items.Clear();
            getstuff(textBox8.Text);
        }
        private void getstuff(string videoid)
        {
            int tempvidswatched = RipLeech.Properties.Settings.Default.videoswatched;
            RipLeech.Properties.Settings.Default.videoswatched = tempvidswatched + 1;
            RipLeech.Properties.Settings.Default.Save();
            webBrowser1.Navigate("http://youtube.com/v/" + videoid + "&vq=hd720");
            YouTubeRequest request = new YouTubeRequest(settings);
            try
            {
                Uri videoEntryUrl = new Uri("http://gdata.youtube.com/feeds/api/videos/" + videoid);
                Video video = request.Retrieve<Video>(videoEntryUrl);
                Feed<Video> relatedVideos = request.GetRelatedVideos(video);
                printVideoFeed(relatedVideos);
                printVideoEntry(video);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void printVideoEntry(Video video)
        {
            textBox9.Text = video.Description;
            if (video.Media != null && video.Media.Rating != null)
            {
                textBox9.Text = video.Description + "\r\n Restricted in: " + video.Media.Rating.Country.ToString();
            }
        }
        public void printVideoFeed(Feed<Video> feed)
        {
            foreach (Video entry in feed.Entries)
            {
                ListViewItem lvi = new ListViewItem(entry.Title);
                lvi.SubItems.Add(entry.ViewCount.ToString());
                lvi.SubItems.Add(entry.Uploader.ToString());
                lvi.SubItems.Add(entry.VideoId.ToString());
                listView1.Items.Add(lvi);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            label38.Text = "Search Results:";
            try
            {
                string searchupd = "http://nicoding.com/api.php?app=ripleech&updtrend=search&term=" + textBox7.Text;
                HttpWebRequest searchreq = (HttpWebRequest)WebRequest.Create(searchupd);
                WebResponse searchres = searchreq.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(searchres.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string pluginsavail = sr.ReadToEnd();
            }
            catch { }
            listView1.Items.Clear();
            YouTubeRequest request = new YouTubeRequest(settings);
            YouTubeQuery query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);

            //order results by the number of views (most viewed first)
            query.OrderBy = "relevance";

            // search for puppies and include restricted content in the search results
            // query.SafeSearch could also be set to YouTubeQuery.SafeSearchValues.Moderate
            query.Query = textBox7.Text;
            query.SafeSearch = YouTubeQuery.SafeSearchValues.None;

            Feed<Video> videoFeed = request.Get<Video>(query);
            printVideoFeed(videoFeed);
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            label38.Text = "Related Videos:";
            string focused = listView1.FocusedItem.SubItems[3].Text;
            try
            {
                string searchupd = "http://nicoding.com/api.php?app=ripleech&updtrend=video&id=" + focused + "&name=" + listView1.FocusedItem.SubItems[0].Text;
                HttpWebRequest searchreq = (HttpWebRequest)WebRequest.Create(searchupd);
                WebResponse searchres = searchreq.GetResponse();
                System.IO.StreamReader sr = new System.IO.StreamReader(searchres.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string pluginsavail = sr.ReadToEnd();
            }
            catch { }
            listView1.Items.Clear();
            getstuff(focused);
            textBox8.Text = focused;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string format = null;
            if (!String.IsNullOrEmpty(textBox8.Text))
            {
                addtoqueue("http://youtube.com/watch?v=" + textBox8.Text, "");
                if (radioButton10.Checked == true)
                {
                    _downloadTypes.Enqueue("audio");
                }
                else
                {
                    _downloadTypes.Enqueue("video");
                }
            }
            else
            {
                MessageBox.Show("Please Enter a valid Video ID before trying to download the video!");
            }
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
                string compare = "http://nicoding.com/api.php?app=ripleech&newuser=" + textBox5.Text + "&pass=" + textBox6.Text + "&email=" + email;
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

        private void button2_Click(object sender, EventArgs e)
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
        #region converters
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
        private void listBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\RipLeech\Temp\newvid.tmp"))
            {
                string vidid = File.ReadAllText(@"C:\RipLeech\Temp\newvid.tmp");
                if (vidid != Environment.NewLine)
                {
                    textBox8.Text = vidid;
                    getstuff(vidid);
                    Youtube.Select();
                    File.WriteAllText(@"C:\RipLeech\Temp\newvid.tmp", Environment.NewLine);
                }
            }
        }

        private void listBox6_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (File.Exists(listBox6.SelectedItem.ToString()))
            {
                string argument = @"/select, " + listBox6.SelectedItem.ToString();
                System.Diagnostics.Process.Start("explorer.exe", argument);
            }
            else
            {
                MessageBox.Show("Whoops! Seems we've lost your download! Please report the error at https://github.com/NiCoding/RipLeech/issues");
            }
        }

        private void timer5_Tick(object sender, EventArgs e)
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
                    var result = MessageBox.Show("There is an update available for RipLeech! Would you like to download it now?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        metroTabItem4.Select();
                        button1_Click((object)sender, (EventArgs)e);
                        timer5.Stop();
                    }
                }
            }
            catch
            {
                timer5.Stop();
            }
        }

        private void textBox7_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button10_Click((object)sender, (EventArgs)e);
            }
        }

        private void textBox8_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button11_Click((object)sender, (EventArgs)e);
            }
        }

        private void textBox6_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button4_Click((object)sender, (EventArgs)e);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            betagetupdate();
        }
    }
}
