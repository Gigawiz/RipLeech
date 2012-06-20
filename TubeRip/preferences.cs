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
    public partial class preferences : Form
    {
        public preferences()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                TubeRip.Properties.Settings.Default.audioquality = "128";
            }
            else if (radioButton2.Checked == true)
            {
                TubeRip.Properties.Settings.Default.audioquality = "256";
            }
            else if (radioButton3.Checked == true)
            {
                TubeRip.Properties.Settings.Default.audioquality = "320";
            }
            else
            {
                TubeRip.Properties.Settings.Default.audioquality = "320";
            }
            if (radioButton4.Checked == true)
            {
                TubeRip.Properties.Settings.Default.videoquality = "240";
            }
            else if (radioButton5.Checked == true)
            {
                TubeRip.Properties.Settings.Default.videoquality = "360";
            }
            else if (radioButton6.Checked == true)
            {
                TubeRip.Properties.Settings.Default.videoquality = "480";
            }
            else if (radioButton7.Checked == true)
            {
                TubeRip.Properties.Settings.Default.videoquality = "720";
            }
            else if (radioButton8.Checked == true)
            {
                TubeRip.Properties.Settings.Default.videoquality = "1080";
            }
            else
            {
                TubeRip.Properties.Settings.Default.videoquality = "720";
            }
            if (!String.IsNullOrEmpty(textBox1.Text))
            {
                TubeRip.Properties.Settings.Default.videosavepath = textBox1.Text;
            }
            if (!String.IsNullOrEmpty(textBox2.Text))
            {
                TubeRip.Properties.Settings.Default.audiosavepath = textBox2.Text;
            }
            TubeRip.Properties.Settings.Default.Save();
            this.Close();
        }

        private void preferences_Load(object sender, EventArgs e)
        {
            if (TubeRip.Properties.Settings.Default.audioquality == "")
            {
                radioButton1.Checked = true;
            }
            else if (TubeRip.Properties.Settings.Default.audioquality == "")
            {
                radioButton2.Checked = true;
            }
            else if (TubeRip.Properties.Settings.Default.audioquality == "")
            {
                radioButton3.Checked = true;
            }
            else
            {
                radioButton3.Checked = true;
            }
            if (TubeRip.Properties.Settings.Default.videoquality == "240")
            {
                radioButton4.Checked = true;
            }
            else if (TubeRip.Properties.Settings.Default.videoquality == "360")
            {
                radioButton5.Checked = true;
            }
            else if (TubeRip.Properties.Settings.Default.videoquality == "480")
            {
                radioButton6.Checked = true;
            }
            else if (TubeRip.Properties.Settings.Default.videoquality == "720")
            {
                radioButton7.Checked = true;
            }
            else if (TubeRip.Properties.Settings.Default.videoquality == "1080")
            {
                radioButton8.Checked = true;
            }
            else
            {
                radioButton7.Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD1 = new FolderBrowserDialog();
            FBD1.ShowNewFolderButton = true;
            FBD1.Description = "Choose a location to save the Video files downloaded by TubeRip";
            FBD1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            DialogResult result = FBD1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = FBD1.SelectedPath;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog FBD1 = new FolderBrowserDialog();
            FBD1.ShowNewFolderButton = true;
            FBD1.Description = "Choose a location to save the Audio files downloaded by TubeRip";
            FBD1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            DialogResult result = FBD1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.Text = FBD1.SelectedPath;
            }
        }
    }
}
