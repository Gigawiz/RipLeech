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
            if (textBox2.Text.Length > 4)
            {
                button2.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            InputBoxValidation validation = delegate(string val)
            {
                if (val == "")
                    return "Value cannot be empty.";
                if (!(new Regex(@"^[a-zA-Z0-9_\-\.]+@[a-zA-Z0-9_\-\.]+\.[a-zA-Z]{2,}$")).IsMatch(val))
                    return "Email address is not valid.";
                return "";
            };

            InputBoxValidation validation2 = delegate(string val2)
            {
                if (val2 == "")
                    return "Value cannot be empty.";
                if (val2.Length <= 5)
                    return "password must be 6 or more characters";
                return "";
            };

            string value = "info@example.com";
            string value2 = "123456";
            if (InputBox.Show("Enter your email address", "Email address:", ref value, validation) == DialogResult.OK)
            {
                if (InputBox.Show("Please enter a password 6 characters or more.", "Password:", ref value2, validation2) == DialogResult.OK)
                {
                    string compare = "http://nicoding.com/api.php?app=ripleech&newuser=" + textBox2.Text + "&pass="+value2+"&email="+value;
                    WebClient web2 = new WebClient();
                    System.IO.Stream stream2 = web2.OpenRead(compare);
                    string result2 = null;
                    using (System.IO.StreamReader reader2 = new System.IO.StreamReader(stream2))
                    {
                        result2 = reader2.ReadToEnd();
                    }
                    if (result2 == "Error! User already exists!")
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
                    else if (result2 == "User info updated successfully")
                    {
                        MessageBox.Show("You have Successfully registered! You may now login with your username and password.", "Registration Successful!");
                    }
                    else
                    {
                        MessageBox.Show("There seems to be an error with the user system at the present time. Please try again later.", "System Error");
                    }
                }
            }
        }
    }
}
