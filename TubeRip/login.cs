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
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if ((String.IsNullOrEmpty(textBox1.Text)) || (String.IsNullOrEmpty(textBox2.Text)))
            {
                MessageBox.Show("Error! You have not entered your username/password correctly! Please try again.");
            }
            else
            {

            }
        }
    }
}
