using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using Microsoft.Win32;

namespace RipLeech
{
    public partial class survey : Form
    {
        public survey()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string compare = null;
            if (radioButton1.Checked == true)
            {
                compare = "http://nicoding.com/api.php?app=ripleech&survey=" + radioButton1.Text;
            }
            else if (radioButton2.Checked == true)
            {
                compare = "http://nicoding.com/api.php?app=ripleech&survey=" + radioButton2.Text;
            }
            else if (radioButton3.Checked == true)
            {
                compare = "http://nicoding.com/api.php?app=ripleech&survey=" + radioButton3.Text;
            }
            else if (radioButton4.Checked == true)
            {
                compare = "http://nicoding.com/api.php?app=ripleech&survey=" + radioButton4.Text;
            }
            else if (radioButton5.Checked == true)
            {
                compare = "http://nicoding.com/api.php?app=ripleech&survey=" + radioButton5.Text;
            }
            else if (radioButton6.Checked == true)
            {
                compare = "http://nicoding.com/api.php?app=ripleech&survey=" + radioButton6.Text;
            }
            else if (radioButton7.Checked == true)
            {
                compare = "http://nicoding.com/api.php?app=ripleech&survey=" + radioButton7.Text;
            }
            WebClient web = new WebClient();
            System.IO.Stream stream = web.OpenRead(compare);
            MessageBox.Show("Thanks for your input!");
            RipLeech.Properties.Settings.Default.surveyshown = true;
            RipLeech.Properties.Settings.Default.Save();
            this.Close();
        }

        private void survey_Load(object sender, EventArgs e)
        {

        }
    }
}
