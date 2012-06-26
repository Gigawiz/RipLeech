using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace TubeRip
{
    public partial class dlhistory : Form
    {
        string fileloc = Application.StartupPath + @"\history.trh";
        public dlhistory()
        {
            InitializeComponent();
        }

        private void dlhistory_Load(object sender, EventArgs e)
        {
            /*if (File.Exists("temp_history.trh"))
            {
                string logtodecrypt = Application.StartupPath + @"\temp_history.trh";
                string logtosave = Application.StartupPath + @"\reader_history.trh";
                logencryptor.DecryptFile(logtodecrypt, logtosave);
                textBox1.Text = File.ReadAllText("reader_history.trh");
                File.Delete("reader_history.trh");
            }
            else if (File.Exists("history.trh"))
            {
                string logtodecrypt = Application.StartupPath + @"\history.trh";
                string logtosave = Application.StartupPath + @"\reader_history.trh";
                logencryptor.DecryptFile(logtodecrypt, logtosave);
                textBox1.Text = File.ReadAllText("reader_history.trh");
                File.Delete("reader_history.trh");
            }
            else
            {
                textBox1.Text = "No History or Not set to save history";
            }*/
        }

    }
}
