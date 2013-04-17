using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;

namespace QDFeedParser
{
    public class Atom10Feed : BaseSyndicationFeed
    {
        #region Constructors

        /// <summary>
        /// Default constructor for Atom10Feed
        /// </summary>
        public Atom10Feed():base(FeedType.Atom10){}

        /// <summary>
        /// Constuctor for Atom10Feed object
        /// </summary>
        /// <param name="feeduri">The Uri used to identify the feed</param>
        public Atom10Feed(string feeduri) : base(feeduri, FeedType.Atom10)
        {
        }

        #endregion
    }
}
