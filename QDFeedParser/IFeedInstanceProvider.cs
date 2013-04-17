namespace QDFeedParser
{
    public interface IFeedInstanceProvider
    {
        Rss20Feed CreateRss20Feed(string feeduri);
        Atom10Feed CreateAtom10Feed(string feeduri);
    }
}
