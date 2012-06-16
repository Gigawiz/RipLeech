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



namespace TubeRip
{
    public partial class mainpage : Form
    {
       // YouTubeRequestSettings settings = new YouTubeRequestSettings("TubeRip", "", "AI39si5SxBrG0x12TqlsrGpnsoCcqgV9-diBgRS5xOhcDB01sSH6WVSTJjhlQenMSt4qH_UC87Y8kqYaf4Ykgw-poTZ7yF2zDw");
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
            label1.Text = "Status: Loading Video";
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
        }

        private void listView1_ItemActivate_1(object sender, EventArgs e)
        {
            getstuff(listView1.FocusedItem.SubItems[3].Text);
        }
    }
}