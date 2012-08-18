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
        string viddling = "";
        string vidout = "";
        string mp4out = "";
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
                    try
                    {
                        VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.HighDefinition1080);
                        mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                        label11.Text = video.Title;
                        label17.Text = "1080p";
                        WebClient client = new WebClient();
                        string saveto = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + video.VideoExtension;
                        vidout = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + video.Title + ".mp3";
                        viddling = saveto;
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
                            mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                            label11.Text = video.Title;
                            label17.Text = "720p";
                            WebClient client = new WebClient();
                            string saveto = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + video.VideoExtension;
                            vidout = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + video.Title + ".mp3";
                            viddling = saveto;
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
                                mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                                label11.Text = video.Title;
                                label17.Text = "480p";
                                WebClient client = new WebClient();
                                string saveto = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + video.VideoExtension;
                                vidout = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + video.Title + ".mp3";
                                viddling = saveto;
                                client.Encoding = System.Text.Encoding.UTF8;
                                Uri update = new Uri(video.DownloadUrl);
                                client.DownloadFileAsync(update, saveto);
                                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
                            }
                            catch
                            {
                                VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.Standard360);
                                mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                                label11.Text = video.Title;
                                label17.Text = "360p";
                                WebClient client = new WebClient();
                                string saveto = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + video.VideoExtension;
                                vidout = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + video.Title + ".mp3";
                                viddling = saveto;
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
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    ProcessStartInfo psi = new ProcessStartInfo();
                    string startdir = Directory.GetCurrentDirectory();
                    if (Environment.Is64BitOperatingSystem)
                    {
                        psi.FileName = startdir + @"\Data\ffmpeg-64.exe";
                    }
                    else
                    {
                        psi.FileName = startdir + @"\Data\ffmpeg-32.exe";
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
                ProcessStartInfo psi = new ProcessStartInfo();
                string startdir = Directory.GetCurrentDirectory();
                if (Environment.Is64BitOperatingSystem)
                {
                    psi.FileName = startdir + @"\Data\ffmpeg-64.exe";
                }
                else
                {
                    psi.FileName = startdir + @"\Data\ffmpeg-32.exe";
                }
                string convert = viddling;
                string bitrate = RipLeech.Properties.Settings.Default.audioquality;
                psi.Arguments = string.Format("-i \"{0}\" -vn -y -f mp3 -ab {2}k \"{1}\"", viddling, vidout, bitrate);
                psi.WindowStyle = ProcessWindowStyle.Normal;
                Process p = Process.Start(psi);
                p.WaitForExit();
                if (p.HasExited == true)
                {
                    File.Delete(viddling);
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
            listView1.Items.Clear();
            getstuff(focused);
            textBox1.Text = focused;
        }

        private void listView1_ItemActivate_1(object sender, EventArgs e)
        {
            string focused = listView1.FocusedItem.SubItems[3].Text;
            listView1.Items.Clear();
            getstuff(focused);
            textBox1.Text = focused;
        }

        private void mainpage_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.audiosavepath))
            {
                RipLeech.Properties.Settings.Default.audiosavepath = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic);
            }
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.videosavepath))
            {
                RipLeech.Properties.Settings.Default.videosavepath = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label5.Text = "Search Results:"; 
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
                Process.Start(viddling);
            }
            else if (viddling.Contains("flv"))
            {
                Process.Start(mp4out);
            }
            else
            {
                Process.Start(vidout);
            }
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

        private void button5_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox3.Text))
            {
                int i = 0;
                YouTubeRequest request = new YouTubeRequest(settings);
                Feed<Playlist> userPlaylists = request.GetPlaylistsFeed(textBox3.Text);
                List<string> myCollection = new List<string>();
                List<string> myYtQueue = new List<string>();
                foreach (Playlist p in userPlaylists.Entries)
                {
                    Feed<PlayListMember> list = request.GetPlaylist(p);
                    foreach (Video v in list.Entries)
                    {
                        myCollection.Add(v.WatchPage.ToString().Replace("&feature=youtube_gdata_player", ""));
                        myYtQueue.Add(v.WatchPage.ToString().Replace("&feature=youtube_gdata_player", ","));
                    }
                }
                string[] myYTQueue = myYtQueue.ToArray();
                RipLeech.Properties.Settings.Default.ytqueue = String.Join(",", myYTQueue);
                foreach (string url in myCollection.ToArray())
                {
                    _downloadUrls.Enqueue(url);
                    count++;
                }
                DownloadFile();
            }
        }

        private void textBox3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button5_Click((object)sender, (EventArgs)e);
            }
        }
        private Queue<string> _downloadUrls = new Queue<string>();
        int count = 0;
        int done = 0;
        double bytesIn = 0;

        private void DownloadFile()
        {
            if (_downloadUrls.Any())
            {
                WebClient client2 = new WebClient();
                client2.DownloadProgressChanged += client2_DownloadProgressChanged;
                client2.DownloadFileCompleted += client2_DownloadFileCompleted;
                var url = _downloadUrls.Dequeue();
                IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(url);
                label1.Text = "Status: Downloading...";
                #region get vid max quality
                try
                {
                    VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.HighDefinition1080);
                    mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                    string saveto = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + video.VideoExtension;
                    vidout = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + video.Title + ".mp3";
                    viddling = saveto;
                    Uri update = new Uri(video.DownloadUrl);
                    client2.DownloadFileAsync(update, saveto);
                    client2.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client2_DownloadProgressChanged);
                    client2.DownloadFileCompleted += new AsyncCompletedEventHandler(client2_DownloadFileCompleted);
                }
                catch
                {
                    try
                    {
                        VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.HighDefinition720);
                        mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                        WebClient client = new WebClient();
                        string saveto = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + video.VideoExtension;
                        vidout = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + video.Title + ".mp3";
                        viddling = saveto;
                        Uri update = new Uri(video.DownloadUrl);
                        client2.DownloadFileAsync(update, saveto);
                        client2.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client2_DownloadProgressChanged);
                        client2.DownloadFileCompleted += new AsyncCompletedEventHandler(client2_DownloadFileCompleted);
                    }
                    catch
                    {
                        try
                        {
                            VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.HighDefinition4K);
                            mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                            WebClient client = new WebClient();
                            string saveto = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + video.VideoExtension;
                            vidout = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + video.Title + ".mp3";
                            viddling = saveto;
                            Uri update = new Uri(video.DownloadUrl);
                            client2.DownloadFileAsync(update, saveto);
                            client2.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client2_DownloadProgressChanged);
                            client2.DownloadFileCompleted += new AsyncCompletedEventHandler(client2_DownloadFileCompleted);
                        }
                        catch
                        {
                            VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.Standard360);
                            mp4out = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + ".mp4";
                            WebClient client = new WebClient();
                            string saveto = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + @"\" + video.Title + video.VideoExtension;
                            vidout = Environment.GetFolderPath(Environment.SpecialFolder.MyMusic) + @"\" + video.Title + ".mp3";
                            viddling = saveto;
                            Uri update = new Uri(video.DownloadUrl);
                            client2.DownloadFileAsync(update, saveto);
                            client2.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client2_DownloadProgressChanged);
                            client2.DownloadFileCompleted += new AsyncCompletedEventHandler(client2_DownloadFileCompleted);
                        }
                    }
                }
                #endregion
                return;
            }
            // End of the download
            label1.Text = "Status: Ready";
            RipLeech.Properties.Settings.Default.ytqueue = "";
            RipLeech.Properties.Settings.Default.Save();
        }
        private void client2_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // handle error scenario
                throw e.Error;
            }
            if (e.Cancelled)
            {
                // handle cancelled scenario
            }
            done++;
            DownloadFile();
        }

        void client2_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            bytesIn = double.Parse(e.BytesReceived.ToString());
            double totalBytescheck = double.Parse(e.TotalBytesToReceive.ToString());
            double percentage = ((double)done / (double)count) * (double)100;
            progressBar2.Value = int.Parse(Math.Truncate(percentage).ToString());
        }
        
    }
}