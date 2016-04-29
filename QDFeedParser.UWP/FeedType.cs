using System;
using System.Collections.Generic;

using System.Text;

namespace QDFeedParser
{
    /// <summary>
    /// An enumerated type used to indicate which type of syndication feed an object
    /// or element is parsed from. It's more or less meant to save libary users the trouble
    /// of type-checking resultant objects.
    /// </summary>
    public enum FeedType
    {
        Rss20 = 1,
        Atom10 = 2
    }
}
