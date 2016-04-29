using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDFeedParser.Xml
{
    public interface IFeedXmlParser
    {
        /// <summary>
        /// Parse the XML contents of a feed and populate its fields
        /// </summary>
        /// <param name="feed">An initialized IFeed object</param>
        void ParseFeed(IFeed feed, string feedxml);

        FeedType CheckFeedType(string xml);
    }
}
