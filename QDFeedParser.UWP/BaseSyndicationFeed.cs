using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using System.Text;
using System.Xml.Serialization;

namespace QDFeedParser
{
    [XmlInclude(typeof(Rss20FeedItem))]
    [XmlInclude(typeof(Atom10FeedItem))]
    public abstract class BaseSyndicationFeed : IFeed
    {
        #region Constructors

        protected BaseSyndicationFeed(FeedType feedType)
        {
            FeedType = feedType;
            Items = new List<BaseFeedItem>();
        }

        protected BaseSyndicationFeed(string feeduri, FeedType feedtype) : this(feedtype)
        {
            FeedUri = feeduri;
        }

        #endregion

        #region ISyndicationFeed Members

        public string Title
        {
            get; set;
        }

        public string Link
        {
            get; set;
        }

        public string FeedUri
        {
            get; set;
        }

        public DateTime LastUpdated
        {
            get; set;
        }

        public string Generator
        {
            get; set;
        }

        public FeedType FeedType
        {
            get; set;
        }
        
        public List<BaseFeedItem> Items
        {
            get; set;
        }

        #endregion

    }
}
