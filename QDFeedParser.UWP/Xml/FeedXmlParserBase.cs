using System;

namespace QDFeedParser.Xml
{
    public abstract class FeedXmlParserBase : IFeedXmlParser
    {
        public abstract void ParseFeed(IFeed feed, string xml);
        public abstract FeedType CheckFeedType(string xml);

        public const string AtomRootElementName = "feed";
        public const string RssRootElementName = "rss";
        public const string RssVersionAttributeName = "version";

        protected DateTime SafeGetDate(string datetime)
        {
            DateTime newDate;
            if (DateTime.TryParse(datetime, out newDate))
                return newDate;

            //if we fail the normal parsing, we try and strip any timezone information from the end
            datetime = datetime.Substring(0, datetime.LastIndexOf(' '));

            return DateTime.TryParse(datetime, out newDate) ? newDate : DateTime.UtcNow;
        }
    }
}