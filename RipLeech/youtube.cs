using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.YouTube;
using Google.GData.Extensions.MediaRss;
using Google.YouTube;
using System.Net;
using System.IO;
using System.Web;
using System.Diagnostics;
using YoutubeExtractor;



namespace RipLeech
{
    public partial class youtube : Form
    {
        string vidid2 = null;
        string root = @"C:\RipLeech\Temp\";
        string ffmpeg = @"C:\Program Files\NiCoding\RipLeech";
        string viddling = "";
        string vidout = "";
        string mp4out = "";
        string vidname = null;
        YouTubeRequestSettings settings = new YouTubeRequestSettings("TubeRip", "AI39si5SxBrG0x12TqlsrGpnsoCcqgV9-diBgRS5xOhcDB01sSH6WVSTJjhlQenMSt4qH_UC87Y8kqYaf4Ykgw-poTZ7yF2zDw");
        public youtube()
        {
            InitializeComponent();
        }

        private void mainpage_FormClosing(object sender, FormClosingEventArgs e)
        {
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((!String.IsNullOrEmpty(textBox1.Text)) || (textBox1.Text != "Example: qbagLrDTzGY"))
            {
                try
                {
                    int tempvidsdled = RipLeech.Properties.Settings.Default.videosdownloaded;
                    RipLeech.Properties.Settings.Default.videosdownloaded = tempvidsdled + 1;
                    RipLeech.Properties.Settings.Default.Save();
                    // Our youtube link
                    string link = "http://www.youtube.com/watch?v=" + textBox1.Text;
                    /*
                     * Get the available video formats.
                     * We'll work with them in the video and audio download examples.
                     */
                    IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);
                    #region get vid max quality
                    if (radioButton2.Checked == true)
                    {
                        #region get audio quality
                        if (RipLeech.Properties.Settings.Default.ffmpeg == false)
                        {
                            try
                            {
                                VideoInfo video = videoInfos.Where(info => info.CanExtractAudio).First(info => info.VideoFormat == VideoFormat.FlashAacHighQuality || info.VideoFormat == VideoFormat.FlashAacLowQuality || info.VideoFormat == VideoFormat.FlashMp3HighQuality || info.VideoFormat == VideoFormat.FlashMp3LowQuality);
                                label11.Text = video.Title;
                                label17.Text = "HQ Mp3";
                                WebClient client = new WebClient();
                                string saveto = root + video.Title + video.VideoExtension;
                                vidout = root + video.Title + ".mp3";
                                viddling = saveto;
                                vidname = video.Title + ".mp3";
                                client.Encoding = System.Text.Encoding.UTF8;
                                Uri update = new Uri(video.DownloadUrl);
                                client.DownloadFileAsync(update, saveto);
                                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            try
                            {
                                VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.HighDefinition1080);
                                if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
                                {
                                    mp4out = RipLeech.Properties.Settings.Default.videosavepath;
                                }
                                else
                                {
                                    mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                                }
                                label11.Text = video.Title;
                                label17.Text = "1080p";
                                WebClient client = new WebClient();
                                string saveto = root + video.Title + video.VideoExtension;
                                vidout = root + video.Title + ".mp3";
                                viddling = saveto;
                                vidname = video.Title + ".mp3";
                                client.Encoding = System.Text.Encoding.UTF8;
                                Uri update = new Uri(video.DownloadUrl);
                                client.DownloadFileAsync(update, saveto);
                                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                            }
                            catch
                            {
                                try
                                {
                                    VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.HighDefinition720);
                                    if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
                                    {
                                        mp4out = RipLeech.Properties.Settings.Default.videosavepath;
                                    }
                                    else
                                    {
                                        mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                                    }
                                    label11.Text = video.Title;
                                    label17.Text = "720p";
                                    WebClient client = new WebClient();
                                    string saveto = root + video.Title + video.VideoExtension;
                                    vidout = root + video.Title + ".mp3";
                                    viddling = saveto;
                                    vidname = video.Title + ".mp3";
                                    client.Encoding = System.Text.Encoding.UTF8;
                                    Uri update = new Uri(video.DownloadUrl);
                                    client.DownloadFileAsync(update, saveto);
                                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                                }
                                catch
                                {
                                    try
                                    {
                                        VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.HighDefinition4K);
                                        if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
                                        {
                                            mp4out = RipLeech.Properties.Settings.Default.videosavepath;
                                        }
                                        else
                                        {
                                            mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                                        }
                                        label11.Text = video.Title;
                                        label17.Text = "480p";
                                        WebClient client = new WebClient();
                                        string saveto = root + video.Title + video.VideoExtension;
                                        vidout = root + video.Title + ".mp3";
                                        viddling = saveto;
                                        vidname = video.Title + ".mp3";
                                        client.Encoding = System.Text.Encoding.UTF8;
                                        Uri update = new Uri(video.DownloadUrl);
                                        client.DownloadFileAsync(update, saveto);
                                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                                    }
                                    catch
                                    {
                                        VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.Standard360);
                                        if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
                                        {
                                            mp4out = RipLeech.Properties.Settings.Default.videosavepath;
                                        }
                                        else
                                        {
                                            mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                                        }
                                        label11.Text = video.Title;
                                        label17.Text = "360p";
                                        WebClient client = new WebClient();
                                        string saveto = root + video.Title + video.VideoExtension;
                                        vidout = root + video.Title + ".mp3";
                                        viddling = saveto;
                                        vidname = video.Title + ".mp3";
                                        client.Encoding = System.Text.Encoding.UTF8;
                                        Uri update = new Uri(video.DownloadUrl);
                                        client.DownloadFileAsync(update, saveto);
                                        client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                                        client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                                    }
                                }
                            }
                        }
#endregion
                    }
                    else
                    {
                        #region get video quality
                        try
                        {
                            VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.HighDefinition1080);
                            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
                            {
                                mp4out = RipLeech.Properties.Settings.Default.videosavepath + @"\" + video.Title + ".mp4";
                            }
                            else
                            {
                                mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                            }
                            label11.Text = video.Title;
                            label17.Text = "1080p";
                            WebClient client = new WebClient();
                            string saveto = root + video.Title + video.VideoExtension;
                            vidout = root + video.Title + video.VideoExtension;
                            viddling = saveto;
                            vidname = video.Title + video.VideoExtension;
                            client.Encoding = System.Text.Encoding.UTF8;
                            Uri update = new Uri(video.DownloadUrl);
                            client.DownloadFileAsync(update, saveto);
                            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                            client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                        }
                        catch
                        {
                            try
                            {
                                VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.HighDefinition720);
                                if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
                                {
                                    mp4out = RipLeech.Properties.Settings.Default.videosavepath + @"\" + video.Title + ".mp4";
                                }
                                else
                                {
                                    mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                                }
                                label11.Text = video.Title;
                                label17.Text = "720p";
                                WebClient client = new WebClient();
                                string saveto = root + video.Title + video.VideoExtension;
                                vidout = root + video.Title + video.VideoExtension;
                                viddling = saveto;
                                vidname = video.Title + video.VideoExtension;
                                client.Encoding = System.Text.Encoding.UTF8;
                                Uri update = new Uri(video.DownloadUrl);
                                client.DownloadFileAsync(update, saveto);
                                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                            }
                            catch
                            {
                                try
                                {
                                    VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.HighDefinition4K);
                                    if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
                                    {
                                        mp4out = RipLeech.Properties.Settings.Default.videosavepath + @"\" + video.Title + ".mp4";
                                    }
                                    else
                                    {
                                        mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                                    }
                                    label11.Text = video.Title;
                                    label17.Text = "480p";
                                    WebClient client = new WebClient();
                                    string saveto = root + video.Title + video.VideoExtension;
                                    vidout = root + video.Title + video.VideoExtension;
                                    viddling = saveto;
                                    vidname = video.Title + video.VideoExtension;
                                    client.Encoding = System.Text.Encoding.UTF8;
                                    Uri update = new Uri(video.DownloadUrl);
                                    client.DownloadFileAsync(update, saveto);
                                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                                }
                                catch
                                {
                                    VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.Standard360);
                                    if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
                                    {
                                        mp4out = RipLeech.Properties.Settings.Default.videosavepath + @"\" + video.Title + ".mp4";
                                    }
                                    else
                                    {
                                        mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                                    }
                                    label11.Text = video.Title;
                                    label17.Text = "360p";
                                    WebClient client = new WebClient();
                                    string saveto = root + video.Title + video.VideoExtension;
                                    vidout = root + video.Title + video.VideoExtension;
                                    viddling = saveto;
                                    vidname = video.Title + video.VideoExtension;
                                    client.Encoding = System.Text.Encoding.UTF8;
                                    Uri update = new Uri(video.DownloadUrl);
                                    client.DownloadFileAsync(update, saveto);
                                    client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                                    client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                                }
                            }
                        }
                        #endregion
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    MessageBox.Show("It seems that there has been an error trying to get the Video ID. please try re-entering the id. If the problem persists, submit an issue on https://github.com/NiCoding/RipLeech/issues/new");
                }
            }
            else
            {
                MessageBox.Show("Please Enter a valid Video ID before trying to download the video!");
            }
        }
        void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                if (!viddling.Contains("flv"))
                {
                    label1.Text = "Status: Ready";
                    File.Copy(viddling, mp4out);
                    File.Delete(viddling);
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    ProcessStartInfo psi = new ProcessStartInfo();
                    string startdir = Directory.GetCurrentDirectory();
                    if (Environment.Is64BitOperatingSystem)
                    {
                        psi.FileName = ffmpeg + @"\Data\ffmpeg-64.exe";
                    }
                    else
                    {
                        psi.FileName = ffmpeg + @"\Data\ffmpeg-32.exe";
                    }
                    string convert = viddling;
                    psi.Arguments = string.Format("-i \"{0}\" -y -sameq -ar 22050 \"{1}\"", viddling, mp4out);
                    psi.WindowStyle = ProcessWindowStyle.Normal;
                    Process p = Process.Start(psi);
                    p.WaitForExit();
                    if (p.HasExited == true)
                    {
                        this.Cursor = Cursors.Default;
                        File.Delete(viddling);
                    }
                }
            }
            else if (radioButton2.Checked == true)
            {
                if (RipLeech.Properties.Settings.Default.ffmpeg == true)
                {
                    this.Cursor = Cursors.WaitCursor;
                    Process proc = new Process();
                    string convert = viddling;
                    string bitrate = RipLeech.Properties.Settings.Default.audioquality;
                    if (Environment.Is64BitOperatingSystem)
                    {
                        proc.StartInfo.FileName = ffmpeg + @"\Data\ffmpeg-64.exe";
                    }
                    else
                    {
                        proc.StartInfo.FileName = ffmpeg + @"\Data\ffmpeg-32.exe";
                    }
                    proc.StartInfo.Arguments = string.Format("-i \"{0}\" -vn -y -f mp3 -ab 320k \"{1}\"", viddling, vidout);
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.UseShellExecute = false;
                    if (!proc.Start())
                    {
                        listBox1.Items.Add("Error starting");
                        return;
                    }
                    StreamReader reader = proc.StandardError;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        listBox1.Items.Add(line);
                    }
                    proc.Close();
                    try
                    {
                        System.IO.StreamWriter sw = new System.IO.StreamWriter(Directory.GetCurrentDirectory() + @"\ffmpeg.log");
                        foreach (object item in listBox1.Items)
                        {
                            sw.WriteLine(item.ToString());
                        }
                        sw.Close();
                    }
                    catch
                    {

                    }
                    button6.Visible = true;
                    if (File.Exists(viddling))
                    {
                        File.Delete(viddling);
                        if (File.Exists(vidout))
                        {
                            string mymusic = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + vidname;
                            //MessageBox.Show(mymusic);
                            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.audiosavepath))
                            {
                                File.Copy(vidout, RipLeech.Properties.Settings.Default.audiosavepath + @"\" + vidname);
                            }
                            else
                            {
                                if (!File.Exists(mymusic))
                                {
                                    File.Copy(vidout, mymusic);
                                }
                            }
                            File.Delete(vidout);
                        }
                    }
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    //non ffmpeg functions
                    var flvFile = new FlvFile(viddling, root + @"\" + vidname);
                    flvFile.ExtractStreams();
                    if (File.Exists(viddling))
                    {
                        File.Delete(viddling);
                        if (File.Exists(vidout))
                        {
                            string mymusic = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + vidname;
                            //MessageBox.Show(mymusic);
                            if (!File.Exists(mymusic))
                            {
                                File.Copy(vidout, mymusic);
                            }
                            File.Delete(vidout);
                        }
                    }
                }
            }
            else
            {

            }
            if (timer1.Enabled == true)
            {
                timer1.Stop();
                timer1.Enabled = false;
            }
            label1.Text = "Status: Complete";
            progressBar1.Value = 0;
            button4.Visible = true;
            button8.Visible = true;
        }
        void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            long dlsize = e.TotalBytesToReceive / 1024 / 1024;
            long dldone = e.BytesReceived / 1024 / 1024;
            label1.Text = "Status: Downloading";
            label12.Text = Convert.ToString(dlsize) + "MB";
            label13.Text = "";
            label14.Text = "";
            label15.Text = Convert.ToString(dldone) +  "MB (" + e.ProgressPercentage.ToString() + "%)";
            progressBar1.Value = e.ProgressPercentage;
        }

        public void printVideoEntry(Video video)
        {
            textBox6.Text = video.Description;
            if (video.Media != null && video.Media.Rating != null)
            {
                textBox6.Text = video.Description + "\r\n Restricted in: " + video.Media.Rating.Country.ToString();
            }
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
        private void button2_Click(object sender, EventArgs e)
        {
            label5.Text = "Related Videos:";
            listView1.Items.Clear();
            getstuff(textBox1.Text);
        }
        private void webBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            progressBar1.MarqueeAnimationSpeed = 80;
            progressBar1.Style = ProgressBarStyle.Marquee;
        }
        void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            progressBar1.MarqueeAnimationSpeed = 0;
            progressBar1.Style = ProgressBarStyle.Blocks;

            label1.Text = "Status: Ready";
        }
        void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            if (!webBrowser1.IsBusy)
            {
                progressBar1.MarqueeAnimationSpeed = 0;
                progressBar1.Style = ProgressBarStyle.Blocks;

                label1.Text = "Status: Ready";
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
        private void listView1_ItemActivate(object sender, EventArgs e)
        {
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
            textBox1.Text = focused;
        }

        private void listView1_ItemActivate_1(object sender, EventArgs e)
        {
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
            textBox1.Text = focused;
        }

        private void mainpage_Load(object sender, EventArgs e)
        {
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
            
            timer3.Start();
            timer4.Start();
            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            if (String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.audiosavepath))
            {
                RipLeech.Properties.Settings.Default.audiosavepath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            }
            if (String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
            {
                RipLeech.Properties.Settings.Default.videosavepath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            label5.Text = "Search Results:";
            try
            {
                string searchupd = "http://nicoding.com/api.php?app=ripleech&updtrend=search&term=" + textBox2.Text;
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
            query.Query = textBox2.Text;
            query.SafeSearch = YouTubeQuery.SafeSearchValues.None;

            Feed<Video> videoFeed = request.Get<Video>(query);
            printVideoFeed(videoFeed);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string dlspeed = Directory.GetCurrentDirectory() + "\\" + viddling;
                FileInfo info = new FileInfo(dlspeed);
                if (info.Length > 0)
                {
                    int length = Convert.ToInt32(info.Length / 1000);
                    //label13.Text = Convert.ToString(speed) + " kb/s";
                }
                else
                {
                    timer1.Stop();
                    label13.Text = "Unable to get current speed";
                }
            }
            catch (Exception dlex)
            {
                MessageBox.Show(dlex.Message);
                timer1.Stop();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Process.Start(mp4out);
            }
            else if (viddling.Contains("flv"))
            {
                if (RipLeech.Properties.Settings.Default.ffmpeg == true)
                {
                    Process.Start(mp4out);
                }
                else
                {
                    Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + vidname);
                }
            }
            else
            {
                Process.Start(Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + vidname);
            }
            button4.Visible = false;
            button8.Visible = false;
            button6.Enabled = false;
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
        if (e.KeyCode == Keys.Enter)
        {
            button2_Click((object)sender, (EventArgs)e);
        }

        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button3_Click((object)sender, (EventArgs)e);
            }
        }
        string[] temp = null;
        //http://gdata.youtube.com/feeds/api/playlists/6B4AA5F8DE307567?v=2
        private void button5_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox3.Text))
            {
                YouTubeRequest request = new YouTubeRequest(settings);
                Feed<Playlist> userPlaylists = request.GetPlaylistsFeed(textBox3.Text);
                foreach (Playlist pl in userPlaylists.Entries)
                {
                    temp = pl.Id.Split(':');
                    cmbSubService.Items.Add(pl.Title);
                }
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button5_Click((object)sender, (EventArgs)e);
            }
        }

        private void cmbSubService_SelectedIndexChanged(object sender, EventArgs e)
        {
            label5.Text = "Videos in playlist '" + cmbSubService.SelectedItem.ToString() + "':";
            int ind = temp.GetUpperBound(0);
            MessageBox.Show(temp[ind]);
            textBox3.Text = temp[ind];
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (File.Exists("ffmpeg.log"))
            {
                Process.Start("ffmpeg.log");
            }
            else
            {
                MessageBox.Show("It seems that the FFMPEG log did not save!");
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            recents trending = new recents();
            trending.Show();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\RipLeech\Temp\newvid.tmp"))
            {
                string vidid = File.ReadAllText(@"C:\RipLeech\Temp\newvid.tmp");
                if (vidid != Environment.NewLine)
                {
                    getstuff(vidid);
                    textBox1.Text = vidid;
                    File.WriteAllText(@"C:\RipLeech\Temp\newvid.tmp", Environment.NewLine);
                }
            }
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (File.Exists(@"C:\RipLeech\Temp\newsearch.tmp"))
            {
                vidid2 = File.ReadAllText(@"C:\RipLeech\Temp\newsearch.tmp");
                if (vidid2 != Environment.NewLine)
                {
                    textBox2.Text = vidid2;
                    File.WriteAllText(@"C:\RipLeech\Temp\newsearch.tmp", Environment.NewLine);
                    button3_Click((object)sender, (EventArgs)e);
                }
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string filepath = null;
            if (radioButton1.Checked == true)
            {
                filepath = mp4out;
            }
            else if (viddling.Contains("flv"))
            {
                if (RipLeech.Properties.Settings.Default.ffmpeg == true)
                {
                    filepath = mp4out;
                }
                else
                {
                    if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.audiosavepath))
                    {
                        filepath = RipLeech.Properties.Settings.Default.audiosavepath + @"\" + vidname;
                    }
                    else
                    {
                        filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + vidname;
                    }
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.audiosavepath))
                {
                    filepath = RipLeech.Properties.Settings.Default.audiosavepath + @"\" + vidname;
                }
                else
                {
                    filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + vidname;
                }
            }
            string argument = @"/select, " + filepath;
            System.Diagnostics.Process.Start("explorer.exe", argument);
            button4.Visible = false;
            button8.Visible = false;
            button6.Enabled = false;
        }
    }
}