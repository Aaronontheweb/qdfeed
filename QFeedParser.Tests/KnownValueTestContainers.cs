using System;

namespace QDFeedParser.Tests
{
    public interface IFeedKnownValueTest
    {
        string Title { get; set; }
        string Link { get; set; }
        Uri FeedUri { get; set; }
        DateTime LastUpdated { get; set; }
        string Generator { get; set; }
        FeedType FeedType { get; set; }
        Type FeedObjectType { get; set; }
    }

    public class FeedKnownValueTest : IFeedKnownValueTest
    {
        public string Title { get; set; }
        public string Link { get; set; }
        public Uri FeedUri { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Generator { get; set; }
        public FeedType FeedType { get; set; }
        public Type FeedObjectType { get; set; }
    }

    public class RssFeedKnownValueTest : FeedKnownValueTest
    {
        public string Description { get; set; }
        public string Language { get; set; }
    }

    public class AtomFeedKnownValueTest : FeedKnownValueTest{}
}
