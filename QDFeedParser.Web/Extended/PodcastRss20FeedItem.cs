using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QDFeedParser.Examples.Web.Extended
{
    public class PodcastRss20FeedItem : BaseFeedItem
    {
        public string Comments { get; set; }
        public string DcCreator { get; set; }
       // public string ContentEncoded { get; set; }
        public string Enclosure { get; set; }
    }
}