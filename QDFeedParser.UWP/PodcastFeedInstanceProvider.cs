using System;
using System.Collections.Generic;
using System.Linq;

namespace QDFeedParser
{
    public class PodcastFeedInstanceProvider : IFeedInstanceProvider
    {
        //public PodcastRss20Feed CreateRss20Feed(string feeduri)
        //{
        //    return new PodcastRss20Feed(feeduri);
        //}

        //public Atom10Feed CreateAtom10Feed(string feeduri)
        //{
        //    return new Atom10Feed(feeduri);
        //}

        IFeed IFeedInstanceProvider.CreateRss20Feed(string feeduri)
        {
            return new PodcastRss20Feed(feeduri);
        }

        IFeed IFeedInstanceProvider.CreateAtom10Feed(string feeduri)
        {
            return new Atom10Feed(feeduri);
        }
    }
}