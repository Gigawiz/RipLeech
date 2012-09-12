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

namespace RipLeech
{
    public partial class recents : Form
    {
        public recents()
        {
            InitializeComponent();
        }

        private void recents_Load(object sender, EventArgs e)
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
                    listBox1.Items.Add(line);
                }
            }
            catch
            {
                listBox1.Items.Add("Unable to get Trending Videos at this time.");
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
                    listBox2.Items.Add(line);
                }
            }
            catch
            {
                listBox2.Items.Add("Unable to get Trending Searches at this time.");
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string trendname = listBox1.SelectedItem.ToString();
                string trendurl = "http://nicoding.com/api.php?app=ripleech&trending=videos&trend=" + trendname;
                HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create(trendurl);
                WebResponse response2 = request2.GetResponse();
                System.IO.StreamReader sr2 = new System.IO.StreamReader(response2.GetResponseStream(), System.Text.Encoding.GetEncoding("windows-1252"));
                string plugurl = sr2.ReadToEnd();
                File.WriteAllText(@"C:\RipLeech\Temp\newvid.tmp", plugurl);
            }
            catch
            {
                MessageBox.Show("Unable to get that video at the moment. Please try another video.");
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            File.WriteAllText(@"C:\RipLeech\Temp\newsearch.tmp", listBox2.SelectedItem.ToString());
        }
    }
}
