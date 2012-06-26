using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace TubeRip
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/flagbug/YoutubeExtractor");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://ffmpeg.org");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://www.wexplain.com/");
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://stackoverflow.com/");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(TubeRip.Properties.Settings.Default.backgroundloc))
            {
                this.BackgroundImage = Image.FromFile(TubeRip.Properties.Settings.Default.backgroundloc);
                if (TubeRip.Properties.Settings.Default.bgstyle == "Tile")
                {
                    this.BackgroundImageLayout = ImageLayout.Tile;
                }
                else if (TubeRip.Properties.Settings.Default.bgstyle == "Stretch")
                {
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else if (TubeRip.Properties.Settings.Default.bgstyle == "Center")
                {
                    this.BackgroundImageLayout = ImageLayout.Center;
                }
                else
                {
                    this.BackgroundImageLayout = ImageLayout.Tile;
                }
            }
        }
    }
}
