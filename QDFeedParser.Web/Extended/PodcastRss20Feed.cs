using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QDFeedParser.Examples.Web.Extended
{
    public class PodcastRss20Feed : BaseSyndicationFeed
    {
        #region Constructors

        /// <summary>
        /// Default constructor for PodcastRss20Feed objects
        /// </summary>
        public PodcastRss20Feed() : base(QDFeedParser.FeedType.Rss20){ }

        /// <summary>
        /// Constructor for PodcastRss20Feed objects
        /// </summary>
        /// <param name="feeduri">The Uri which uniquely identifies the feed</param>
        public PodcastRss20Feed(string feeduri) : base(feeduri, FeedType.Rss20)
        {
        }

        #endregion

        /// <summary>
        /// The description of this RSS feed.
        /// </summary>
        public string Description
        {
            get; set;
        }

        /// <summary>
        /// The language this RSS feed is encoded in.
        /// </summary>
        public string Language
        {
            get; set;
        }

        /// <summary>
        /// The category this RSS feed is encoded in.
        /// </summary>
        public string Category {
            get; set;
        }

        /// <summary>
        /// The copyright this RSS feed is encoded in.
        /// </summary>
        public string Copyright
        {
            get; set;
        }

        /// <summary>
        /// The managingEditor this RSS feed is encoded in.
        /// </summary>
        public string ManagingEditor
        {
            get; set;
        }

        /// <summary>
        /// The webMaster this RSS feed is encoded in.
        /// </summary>
        public string WebMaster
        {
            get; set;
        }

        /// <summary>
        /// The image this RSS feed is encoded in.
        /// </summary>
        public PodcastRss20FeedImage Image
        {
            get; set;
        }
        
    }
}