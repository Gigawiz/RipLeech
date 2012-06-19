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



namespace TubeRip
{
    public partial class mainpage : Form
    {
        string viddling = "";
        string vidout = "";
        Stopwatch sw = new Stopwatch();
        YouTubeRequestSettings settings = new YouTubeRequestSettings("TubeRip", "AI39si5SxBrG0x12TqlsrGpnsoCcqgV9-diBgRS5xOhcDB01sSH6WVSTJjhlQenMSt4qH_UC87Y8kqYaf4Ykgw-poTZ7yF2zDw");
        public mainpage()
        {
            InitializeComponent();
        }

        private void mainpage_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((!String.IsNullOrEmpty(textBox1.Text)) || (textBox1.Text != "Example: qbagLrDTzGY"))
            {
                // Our test youtube link
                string link = "http://www.youtube.com/watch?v=" + textBox1.Text;

                /*
                 * Get the available video formats.
                 * We'll work with them in the video and audio download examples.
                 */
                IEnumerable<VideoInfo> videoInfos = DownloadUrlResolver.GetDownloadUrls(link);
                VideoInfo video = videoInfos.First(info => info.VideoFormat == VideoFormat.HighDefinition720);
                viddling = video.Title + video.VideoExtension;
                label11.Text = video.Title + "(720p)";
                vidout = video.Title + ".mp3";
                WebClient client = new WebClient();
                string saveto = Directory.GetCurrentDirectory() + @"\Downloads\Temp\" + video.Title + video.VideoExtension;
                client.Encoding = System.Text.Encoding.UTF8;
                Uri update = new Uri(video.DownloadUrl);
                client.DownloadFileAsync(update, saveto);
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);
                client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
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
                File.Move(Directory.GetCurrentDirectory() + @"\Downloads\Temp\" + viddling, Directory.GetCurrentDirectory() + @"\Downloads\Video\" + viddling);
            }
            else if (radioButton2.Checked == true)
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                if (Environment.Is64BitOperatingSystem)
                {
                    psi.FileName = Directory.GetCurrentDirectory() + @"\Data\ffmpeg-64.exe";
                }
                else
                {
                    psi.FileName = Directory.GetCurrentDirectory() + @"\Data\ffmpeg-32.exe";
                }
                string convert = Directory.GetCurrentDirectory() + @"\Downloads\Temp\" + viddling;
                string converted = Directory.GetCurrentDirectory() + @"\Downloads\Audio\" + vidout;
                psi.Arguments = string.Format("-i \"{0}\" -vn -f mp3 -ab 192k \"{1}\"", convert, converted);
                psi.WindowStyle = ProcessWindowStyle.Normal;
                Process p = Process.Start(psi);
                p.WaitForExit();
                if (p.HasExited == true)
                {
                    //this.Cursor = Cursors.Default;
                    File.Delete(Directory.GetCurrentDirectory() + @"\Downloads\Temp\" + viddling);
                    MessageBox.Show("All Done!");
                }
            }
            else
            {

            }
            //sw.Stop();
            //sw.Reset();
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
            /*if (timer1.Enabled == false)
            {
                timer1.Enabled = true;
                timer1.Start();
            }*/
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
            listBox1.Items.Add("Title: " + video.Title);
            listBox1.Items.Add(video.Description);
            listBox1.Items.Add("Keywords: " + video.Keywords);
            listBox1.Items.Add("Uploaded by: " + video.Uploader);
            if (video.YouTubeEntry.Location != null)
            {
                listBox1.Items.Add("Latitude: " + video.YouTubeEntry.Location.Latitude);
                listBox1.Items.Add("Longitude: " + video.YouTubeEntry.Location.Longitude);
            }
            if (video.Media != null && video.Media.Rating != null)
            {
                listBox1.Items.Add("Restricted in: " + video.Media.Rating.Country);
            }

            if (video.IsDraft)
            {
                listBox1.Items.Add("Video is not live.");
                string stateName = video.Status.Name;
                if (stateName == "processing")
                {
                    listBox1.Items.Add("Video is still being processed.");
                }
                else if (stateName == "rejected")
                {
                    listBox1.Items.Add("Video has been rejected because: ");
                    listBox1.Items.Add(video.Status.Value);
                    listBox1.Items.Add("For help visit: ");
                    listBox1.Items.Add(video.Status.Help);
                }
                else if (stateName == "failed")
                {
                    listBox1.Items.Add("Video failed uploading because:");
                    listBox1.Items.Add(video.Status.Value);

                    listBox1.Items.Add("For help visit: ");
                    listBox1.Items.Add(video.Status.Help);
                }

                if (video.ReadOnly == false)
                {
                    listBox1.Items.Add("Video is editable by the current user.");
                }

                if (video.RatingAverage != -1)
                {
                    listBox1.Items.Add("Average rating: " + video.RatingAverage);
                }
                if (video.ViewCount != -1)
                {
                    listBox1.Items.Add("View count: " + video.ViewCount);
                }

                listBox1.Items.Add("Thumbnails:");
                foreach (MediaThumbnail thumbnail in video.Thumbnails)
                {
                    listBox1.Items.Add("\tThumbnail URL: " + thumbnail.Url);
                    listBox1.Items.Add("\tThumbnail time index: " + thumbnail.Time);
                }

                listBox1.Items.Add("Media:");
                foreach (Google.GData.YouTube.MediaContent mediaContent in video.Contents)
                {
                    listBox1.Items.Add("\tMedia Location: " + mediaContent.Url);
                    listBox1.Items.Add("\tMedia Type: " + mediaContent.Format);
                    listBox1.Items.Add("\tDuration: " + mediaContent.Duration);
                }
            }
        }
        private void getstuff(string videoid)
        {
            webBrowser1.Navigate("http://youtube.com/v/" + videoid);
            YouTubeRequest request = new YouTubeRequest(settings);
            try
            {
                listBox1.Items.Clear();
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
            getstuff(listView1.FocusedItem.SubItems[3].Text);
            textBox1.Text = listView1.FocusedItem.SubItems[3].Text;
        }

        private void listView1_ItemActivate_1(object sender, EventArgs e)
        {
            getstuff(listView1.FocusedItem.SubItems[3].Text);
            textBox1.Text = listView1.FocusedItem.SubItems[3].Text;
        }

        private void mainpage_Load(object sender, EventArgs e)
        {
            string folders = Directory.GetCurrentDirectory() + @"\Downloads";
            DirectoryInfo dir = new DirectoryInfo(folders);
            if (!dir.Exists)
            {
                Directory.CreateDirectory(folders);
                Directory.CreateDirectory(folders + @"\Audio");
                Directory.CreateDirectory(folders + @"\Video");
                Directory.CreateDirectory(folders + @"\Temp");
            }
            if (TubeRip.Properties.Settings.Default.UpdateAvail == true)
            {
                var result = MessageBox.Show("There is an update available for TubeRip! Would you like to download it now?", "Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes)
                {
                    updater update = new updater();
                    update.Show();
                }
            }
            if (File.Exists("updater.exe"))
            {
                File.Delete("updater.exe");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label5.Text = "Search Results:"; 
            listView1.Items.Clear();
            YouTubeRequest request = new YouTubeRequest(settings);
            YouTubeQuery query = new YouTubeQuery(YouTubeQuery.DefaultVideoUri);

            if (!String.IsNullOrEmpty(textBox2.Text) || (textBox2.Text != "Enter"))
            {
                AtomCategory category1 = new AtomCategory(textBox2.Text, YouTubeNameTable.KeywordSchema);
                query.Categories.Add(new QueryCategory(category1));
            }
            else if ((textBox3.Text != "") || (textBox3.Text != "Up To"))
            {
                AtomCategory category1 = new AtomCategory(textBox2.Text, YouTubeNameTable.KeywordSchema);
                query.Categories.Add(new QueryCategory(category1));
                AtomCategory category2 = new AtomCategory(textBox3.Text, YouTubeNameTable.KeywordSchema);
                query.Categories.Add(new QueryCategory(category2));
            }
            else if ((textBox4.Text != "") || (textBox4.Text != "4 Search"))
            {
                AtomCategory category1 = new AtomCategory(textBox2.Text, YouTubeNameTable.KeywordSchema);
                query.Categories.Add(new QueryCategory(category1));
                AtomCategory category2 = new AtomCategory(textBox3.Text, YouTubeNameTable.KeywordSchema);
                query.Categories.Add(new QueryCategory(category2));
                AtomCategory category3 = new AtomCategory(textBox4.Text, YouTubeNameTable.KeywordSchema);
                query.Categories.Add(new QueryCategory(category3));
            }
            else if ((textBox5.Text != "") || (textBox5.Text != "Terms Here"))
            {
                AtomCategory category1 = new AtomCategory(textBox2.Text, YouTubeNameTable.KeywordSchema);
                query.Categories.Add(new QueryCategory(category1));
                AtomCategory category2 = new AtomCategory(textBox3.Text, YouTubeNameTable.KeywordSchema);
                query.Categories.Add(new QueryCategory(category2));
                AtomCategory category3 = new AtomCategory(textBox4.Text, YouTubeNameTable.KeywordSchema);
                query.Categories.Add(new QueryCategory(category3));
                AtomCategory category4 = new AtomCategory(textBox5.Text, YouTubeNameTable.KeywordSchema);
                query.Categories.Add(new QueryCategory(category4));
            }
            else
            {
                MessageBox.Show("Please enter at least one search term!");
            }

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
                    int speed = length / Convert.ToInt32(sw.Elapsed.TotalSeconds);
                    label13.Text = Convert.ToString(speed) + " kb/s";
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
            if (viddling.Contains("mp4"))
            {
                Process.Start(Directory.GetCurrentDirectory() + @"\Downloads\Video");
            }
            else
            {
                Process.Start(Directory.GetCurrentDirectory() + @"\Downloads\Audio");
            }
        }
        
    }
}