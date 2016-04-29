using System;
using System.Collections.Generic;
using System.Linq;

namespace QDFeedParser
{
    public class PodcastRss20FeedImage
    {

        #region Constructors

        /// <summary>
        /// Default constructor for PodcastRss20Feed objects
        /// </summary>
        public PodcastRss20FeedImage() { }

        /// <summary>
        /// Default constructor for PodcastRss20Feed objects
        /// </summary>
        /// <param name="url"></param>
        /// <param name="title"></param>
        /// <param name="link"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public PodcastRss20FeedImage(string url, string title, string link, string width, string height) 
        {
            Url = url;
            Title = title;
            Link = link;
            Width = width;
            Height = height;
        }

        #endregion

        /// <summary>
        /// The url this RSS feed is encoded in.
        /// </summary>
        public string Url
        {
            get; set;
        }

        /// <summary>
        /// The title this RSS feed is encoded in.
        /// </summary>
        public string Title
        {
            get; set;
        }

        /// <summary>
        /// The link this RSS feed is encoded in.
        /// </summary>
        public string Link
        {
            get; set;
        }

        /// <summary>
        /// The width this RSS feed is encoded in.
        /// </summary>
        public string Width
        {
            get; set;
        }

        /// <summary>
        /// The height this RSS feed is encoded in.
        /// </summary>
        public string Height
        {
            get; set;
        }

    }
}