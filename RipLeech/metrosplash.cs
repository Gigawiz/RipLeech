using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RipLeech
{
    public partial class metrosplash : Form
    {
        public metrosplash()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (circularProgress1.Value == 100)
            {
                timer1.Stop();
                metro home = new metro();
                home.Show();
                this.Dispose(false);
            }
            else
            {
                circularProgress1.Value += 1;
            }
        }

        private void metrosplash_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }
    }
}
