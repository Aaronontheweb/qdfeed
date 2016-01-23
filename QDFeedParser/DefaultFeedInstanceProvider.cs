namespace QDFeedParser
{
    internal class DefaultFeedInstanceProvider : IFeedInstanceProvider
    {
        public IFeed CreateRss20Feed(string feeduri)
        {
            return new Rss20Feed(feeduri);
        }

        public IFeed CreateAtom10Feed(string feeduri)
        {
            return new Atom10Feed(feeduri);
        }
    }
}

