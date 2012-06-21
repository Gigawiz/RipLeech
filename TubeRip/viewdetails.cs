using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TubeRip
{
    public partial class viewdetails : Form
    {
        public viewdetails()
        {
            InitializeComponent();
        }

        private void viewdetails_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != CryptorEngine.Decrypt(TubeRip.Properties.Settings.Default.password, true))
            {
                MessageBox.Show("Invalid Password!");
                this.Close();
            }
            else
            {
                label1.Visible = true;
                label2.Visible = true;
                label3.Visible = true;
                label4.Visible = true;
                label5.Visible = true;
                label6.Visible = true;
                label7.Visible = true;
                label8.Visible = true;
                label9.Visible = false;
                label10.Visible = true;
                label11.Visible = true;
                label12.Visible = true;
                label13.Visible = true;
                label14.Visible = true;
                label15.Visible = true;
                label16.Visible = true;
                label17.Visible = true;
                textBox1.Visible = false;
                button1.Visible = false;
                label2.Text = TubeRip.Properties.Settings.Default.username;
                label4.Text = CryptorEngine.Decrypt(TubeRip.Properties.Settings.Default.password, true);
                label6.Text = TubeRip.Properties.Settings.Default.email;
                label8.Text = TubeRip.Properties.Settings.Default.age;
                label10.Text = TubeRip.Properties.Settings.Default.videoswatched.ToString();
                label12.Text = TubeRip.Properties.Settings.Default.videosdownloaded.ToString();
                if (TubeRip.Properties.Settings.Default.isverified == false)
                {
                    label15.Text = "Not Email Verified";
                }
                else
                {
                    label15.Text = "Successfully Verified Email!";
                }
                if (TubeRip.Properties.Settings.Default.scriptstats == false)
                {
                    label17.Text = "Free";
                }
                else
                {
                    label17.Text = "Subscribed";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
