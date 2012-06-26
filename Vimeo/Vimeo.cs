using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common;
using BusinessObject;
using BusinessObject.Enums;

namespace Vimeo
{
    public partial class Vimeo : Form
    {
        public Vimeo()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var response = VimeoAPI.Search(textBox1.Text);

            if (VimeoAPI.QueryIsOk(response))
            {
                var vimeoSearchResponse = VimeoAPI.BuildVimeoSearchResponse(response);

                var videoList = new List<VideoExtracted>();

                if (vimeoSearchResponse.videos.total != "0")
                {
                    foreach (var video in vimeoSearchResponse.videos.video)
                    {
                        var thumbnail = GetMediumVimeoThumbnail(video.id);

                        if (!string.IsNullOrEmpty(thumbnail))
                            videoList.Add(new VideoExtracted
                            {
                                Thumbnail = "<img src=\"" + thumbnail + "\" />",
                                Title = video.title,
                                VideoId = video.id
                            });
                    }
                }

                BindSearchRepeaterAndAddToCache(videoList);
            }
            else
            {
                var vimeoErrorResponse = VimeoAPI.BuildVimeoErrorResponse(response);

                DisplayErrorMessage(
                    (VimeoApiErrorCode)Convert.ToInt32(vimeoErrorResponse.error.code) ==
                        VimeoApiErrorCode.ServiceCurrentlyUnavailable
                            ? GlobalConstants.ErrorMessages.SearchServiceUnavailable
                            : GlobalConstants.ErrorMessages.SearchError);
            }
        }
        private void BindSearchRepeaterAndAddToCache(IEnumerable<VideoExtracted> videoList)
        {
            listView1.DataSource = videoList;
            listView1.DataBind();

            if (SearchResultsRepeater.Items.Count > 0)
                phSearchResults.Visible = true;
            else
            {
                DisplayErrorMessage(GlobalConstants.ErrorMessages.NoSearchResults);
                phSearchResults.Visible = false;
            }
        }

        /// <summary>
        /// Gets the medium vimeo thumbnail.
        /// </summary>
        /// <param name="videoId">The video id.</param>
        /// <returns></returns>
        protected string GetMediumVimeoThumbnail(string videoId)
        {
            var response = VimeoAPI.GetVideoThumbnails(videoId);

            if (VimeoAPI.QueryIsOk(response))
            {
                var vimeoVideoThumbnailsResponse = 
                    VimeoAPI.BuildVimeoVideoThumbnailsResponse(response);

                return vimeoVideoThumbnailsResponse.thumbnails.thumbnail[1].thumbnail;
            }

            return null;
        }

        /// <summary>
        /// Displays the error message.
        /// </summary>
        /// <param name="errorMessage">The error message.</param>
        private void DisplayErrorMessage(string errorMessage)
        {
            MessageBox.Show(errorMessage);
            return;
        }
    }
}
