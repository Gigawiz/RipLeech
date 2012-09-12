using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Diagnostics;

namespace RipLeech_Helper
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                string argument = args[0].Replace("ripleech://", "").Replace("youtube.com/watch?v=", "").Replace("http://", "").Replace("www.", "").TrimEnd('/');
                if (!String.IsNullOrEmpty(argument))
                {
                    if (argument.Contains("&"))
                    {
                        argument = argument.Substring(0, argument.IndexOf('&'));
                    }
                }

                Match match = Regex.Match(argument, @"([A-Za-z0-9-]+)$", RegexOptions.IgnoreCase);
                // Here we check the Match instance.
                if (match.Success)
                {
                    File.WriteAllText(@"C:\RipLeech\Temp\newvid.tmp", argument);
                    Process [] localByName = Process.GetProcessesByName("RipLeech");
                    if (localByName == null || localByName.Length == 0)
                    {
                        Process.Start(@"C:\Program Files\NiCoding\RipLeech\RipLeech.exe");
                    }
                }
            }
        }
    }
}
