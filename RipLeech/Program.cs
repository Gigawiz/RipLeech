using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;
using System.Threading;

namespace RipLeech
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //SingleInstance.SingleApplication.Run(new Form1());
            //SingleInstance.SingleApplication.Run(new metrosplash());
            SingleInstance.SingleApplication.Run(new metro());
        }
    }
}
