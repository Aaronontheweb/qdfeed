using System;
using System.Collections.Generic;
using System.Text;

namespace QDFeedParser
{
    public class Rss20Feed : BaseSyndicationFeed
    {
        #region Constructors

        /// <summary>
        /// Default constructor for Rss20Feed objects
        /// </summary>
        public Rss20Feed() : base(QDFeedParser.FeedType.Rss20){}

        /// <summary>
        /// Constructor for Rss20Feed objects
        /// </summary>
        /// <param name="feeduri">The Uri which uniquely identifies the feed</param>
        public Rss20Feed(string feeduri) : base(feeduri, FeedType.Rss20)
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
    }
}
