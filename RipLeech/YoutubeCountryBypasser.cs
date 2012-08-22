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

namespace RipLeech
{
    public partial class YoutubeCountryBypasser : Form
    {
        YouTubeRequestSettings settings = new YouTubeRequestSettings("TubeRip", "AI39si5SxBrG0x12TqlsrGpnsoCcqgV9-diBgRS5xOhcDB01sSH6WVSTJjhlQenMSt4qH_UC87Y8kqYaf4Ykgw-poTZ7yF2zDw");
        public YoutubeCountryBypasser()
        {
            InitializeComponent();
        }

        private void YoutubeCountryBypasser_Load(object sender, EventArgs e)
        {

        }

        static void printVideoFeed(Feed<Video> feed)
        {
            foreach (Video entry in feed.Entries)
            {
                printVideoEntry(entry);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                string viduri = "http://gdata.youtube.com/feeds/api/videos/" + textBox1.Text;
                YouTubeRequest request = new YouTubeRequest(settings);
                Feed<Video> videoFeed = request.Get<Video>(new Uri(viduri));
                printVideoFeed(videoFeed);
            }
        }
        static void printVideoEntry(Video video)
        {
            if (video.Media != null && video.Media.Rating != null)
            {
                MessageBox.Show("Restricted in: " + video.Media.Rating.Country);
            }
            else
            {
                MessageBox.Show("Not Restricted!");
            }
        }
    }
}
