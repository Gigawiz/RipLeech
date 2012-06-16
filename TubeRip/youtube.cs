using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Diagnostics;


namespace TubeRip
{
   static public class youtube
    {
        public enum formatType
        {
            mp3 = 0,
            mp4 = 1,
            flv = 2,
            mpg = 3,
            avi = 4
        }

        /// <summary>
        /// Gets a downloadable youtube link
        /// </summary>
        /// <param name="strUrl">the url to the video</param>
        /// <returns>returns the link on success, else it returns string.empty</returns>
        static public string GetYoutubeLink(string strUrl)
        {
            try
            {
                //Get the key from the url
                string strKey = strUrl.Substring(strUrl.LastIndexOf("v=") + 2, 11);

                if (strKey == string.Empty)
                    throw new ArgumentException();

                //Get the token from the video info
                Uri infoUri = new Uri("http://youtube.com/get_video_info?video_id=" + strKey);
                string strToken = getTokenFromUri(infoUri);

                if (strToken == string.Empty)
                    return string.Empty;

                string strDownloadLink =
                    "http://www.youtube.com/get_video.php?video_id=" + strKey + "&t=" + strToken + "&fmt=18";

                return strDownloadLink;
            }
            catch (Exception ex)
            {
                //Handle exception
            }
            return string.Empty;
        }

        /// <summary>
        /// This function requires ffmpeg, make a folder called ffmpeg in your project folder
        /// </summary>
        /// <param name="strInputFile">path to input file, .mp4 (e.g. file1.mp4)</param>
        /// <param name="strOutPutFile">path to output file, .mp3 (e.g. file2.mp3)</param>
        /// <returns></returns>
        static public int EncodeFormatFromMp4ToMp3(string strInputFile,
                                                    string strOutPutFile,
                                                    bool bolKeepInputFile)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "ffmpeg.EXE";
            startInfo.Arguments = "-i \"" + strInputFile + "\" -vn -ac 1 \"" + strOutPutFile + "\"";
            startInfo.CreateNoWindow = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            Process prcProcess = new Process();
            prcProcess.StartInfo = startInfo;
            prcProcess.Start();

            prcProcess.WaitForExit();
            prcProcess.Close();

            if (!bolKeepInputFile)
            {
                File.Delete(strInputFile);
            }

            return 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDownloadLink">download link</param>
        /// <param name="objFormat">the format you want to save the file as</param>
        /// <param name="strOutputFileName">output file name</param>
        /// <returns></returns>
        static public int CreateFileFromYoutubeDownloadLink(string strDownloadLink,
                                                            string strOutputFileName)
        {
            Uri objUri = new Uri(strDownloadLink);

            //Get file content
            WebRequest wrRequest = WebRequest.Create(objUri);
            WebResponse wrResponse = wrRequest.GetResponse();
            Stream objStream = wrResponse.GetResponseStream();

            FileStream fsStream = new FileStream(strOutputFileName, FileMode.Create);
            byte[] bteRead = new byte[256];

            int iCount = objStream.Read(bteRead, 0, bteRead.Length);

            while (iCount > 0)
            {
                fsStream.Write(bteRead, 0, iCount);
                iCount = objStream.Read(bteRead, 0, bteRead.Length);
            }

            fsStream.Close();
            objStream.Close();
            wrResponse.Close();


            return 0;
        }

        /// <summary>
        /// Gets the title of the youtube link
        /// </summary>
        /// <param name="strUrl">url to youtube video</param>
        /// <returns>return title on success</returns>
        static public string GetTitleFromYoutubeLink(string strUrl)
        {
            try
            {
                Uri objUri = new Uri(strUrl);

                //Get file content
                WebRequest wrRequest = WebRequest.Create(objUri);
                WebResponse wrResponse = wrRequest.GetResponse();
                Stream objStream = wrResponse.GetResponseStream();
                StreamReader objReader = new StreamReader(objStream);

                //Hopefully got it!
                string strFileContent = objReader.ReadToEnd();

                //Calculate the title length
                int iTitlePos = strFileContent.LastIndexOf("<meta name=\"title\" content=\"") + 28;
                int iEndTitlePos = strFileContent.IndexOf("\">", iTitlePos);
                int iDifference = iEndTitlePos - iTitlePos;

                //Moving on to finding the title
                string strTitle = strFileContent.Substring(iTitlePos, iDifference);

                objStream.Close();
                objReader.Close();
                wrResponse.Close();

                return strTitle;
            }
            catch (Exception ex)
            {
                //ex
            }
            return string.Empty;
        }

        static private string getTokenFromUri(Uri objUri)
        {
            //Get file content
            WebRequest wrRequest = WebRequest.Create(objUri);
            WebResponse wrResponse = wrRequest.GetResponse();
            Stream objStream = wrResponse.GetResponseStream();
            StreamReader objReader = new StreamReader(objStream);

            //Hopefully got it!
            string strFileContent = objReader.ReadToEnd();

            //Calculate the token length
            int iTokenPos = strFileContent.LastIndexOf("&token=") + 7;
            int iThumbPos = strFileContent.LastIndexOf("&thumbnail_url=");
            int iDifference = iThumbPos - iTokenPos;

            //Moving on to finding the token
            string strToken = strFileContent.Substring(iTokenPos, iDifference);

            objReader.Close();
            objStream.Close();
            wrResponse.Close();

            if (strToken == string.Empty)
                return string.Empty;
            else
                return strToken;
        }

    }
}
