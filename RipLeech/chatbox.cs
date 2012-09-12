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

namespace RipLeech
{
    public partial class chatbox : Form
    {
        public chatbox()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            button1.Enabled = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sendmsg(textBox1.Text);
            textBox1.Text = "";
            button1.Enabled = false;
            getchats();
        }
        private void getchats()
        {
            string compare = "http://nicoding.com/api.php?app=ripleech&chat=msgs";
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(compare);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string messages = reader.ReadToEnd();
            string[] split = Regex.Split(messages, "<br>");
            listBox2.Items.Clear();
            listBox2.Items.Add("All times are GMT -5 (EST)");
            foreach (string item in split)
            {
                listBox2.Items.Add(item);
            }
            this.listBox2.SelectedIndex = this.listBox2.Items.Count - 1;
        }
        private void sendmsg(string msg)
        {
            string user = "Anonymous";
            string myDate = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time")).ToString();
            DateTime dateValue = DateTime.Parse(myDate);
            string formatForMySql = dateValue.ToString("yyyy-MM-dd HH:mm:ss");
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.loggedinuser)) { user = RipLeech.Properties.Settings.Default.loggedinuser; }
            string compare = "http://nicoding.com/api.php?app=ripleech&chat=sendmsg&message="+msg+"&msguser="+user+"&timestamp="+formatForMySql;
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(compare);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string messages = reader.ReadToEnd();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            getchats();
            getonlineusers();
        }

        private void chatbox_Load(object sender, EventArgs e)
        {
            #region login
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.loggedinuser))
            {
                string user = RipLeech.Properties.Settings.Default.loggedinuser;
                string compare = "http://nicoding.com/api.php?app=ripleech&chat=updatestatus&usr=" + user + "&status=1";
                WebClient web = new WebClient();
                System.IO.Stream stream = web.OpenRead(compare);
                System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                string messages = reader.ReadToEnd();
                optionsToolStripMenuItem.Visible = true;
                checkBox1.Visible = false;
                textBox2.Visible = false;
                label3.Visible = false;
                button2.Visible = false;
            }
            #endregion
            timer1.Start();
            getchats();
            getonlineusers();
        }
        private void getonlineusers()
        {
            string compare = "http://nicoding.com/api.php?app=ripleech&chat=listusrs";
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(compare);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string messages = reader.ReadToEnd();
            string[] split = Regex.Split(messages, "<br>");
            listBox1.Items.Clear();
            foreach (string item in split)
            {
                listBox1.Items.Add(item);
            }
        }
        private void onlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string user = "Anonymous";
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.loggedinuser)) { user = RipLeech.Properties.Settings.Default.loggedinuser; }
            string compare = "http://nicoding.com/api.php?app=ripleech&chat=updatestatus&usr="+user+"&status=1";
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(compare);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string messages = reader.ReadToEnd();
        }

        private void awayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string user = "Anonymous";
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.loggedinuser)) { user = RipLeech.Properties.Settings.Default.loggedinuser; }
            string compare = "http://nicoding.com/api.php?app=ripleech&chat=updatestatus&usr=" + user + "&status=2";
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(compare);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string messages = reader.ReadToEnd();
        }

        private void busyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string user = "Anonymous";
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.loggedinuser)) { user = RipLeech.Properties.Settings.Default.loggedinuser; }
            string compare = "http://nicoding.com/api.php?app=ripleech&chat=updatestatus&usr=" + user + "&status=3";
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(compare);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string messages = reader.ReadToEnd();
        }

        private void invisibleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string user = "Anonymous";
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.loggedinuser)) { user = RipLeech.Properties.Settings.Default.loggedinuser; }
            string compare = "http://nicoding.com/api.php?app=ripleech&chat=updatestatus&usr=" + user + "&status=4";
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(compare);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string messages = reader.ReadToEnd();
        }

        private void offlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string user = "Anonymous";
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.loggedinuser)) { user = RipLeech.Properties.Settings.Default.loggedinuser; }
            string compare = "http://nicoding.com/api.php?app=ripleech&chat=updatestatus&usr=" + user + "&status=5";
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(compare);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string messages = reader.ReadToEnd();
        }

        private void chatbox_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!String.IsNullOrEmpty(RipLeech.Properties.Settings.Default.loggedinuser))
            {
                string user = RipLeech.Properties.Settings.Default.loggedinuser;
                string compare = "http://nicoding.com/api.php?app=ripleech&chat=updatestatus&usr=" + user + "&status=5";
                WebClient web = new WebClient();
                System.IO.Stream stream = web.OpenRead(compare);
                System.IO.StreamReader reader = new System.IO.StreamReader(stream);
                string messages = reader.ReadToEnd();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox2.Enabled = true;
            }
            else
            {
                textBox2.Enabled = false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length > 6)
            {
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string reg = "http://nicoding.com/api.php?app=ripleech&newuser="+textBox2.Text+"&pass=&email=";
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(reg);
            System.IO.StreamReader reader = new System.IO.StreamReader(stream);
            string messages = reader.ReadToEnd();
            MessageBox.Show(messages);
        }
    }
}
