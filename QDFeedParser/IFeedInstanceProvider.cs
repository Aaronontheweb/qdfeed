namespace QDFeedParser
{
    public interface IFeedInstanceProvider
    {
        IFeed CreateRss20Feed(string feeduri);
        IFeed CreateAtom10Feed(string feeduri);
    }
}
