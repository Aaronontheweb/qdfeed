using System;
using System.Collections.Generic;
using System.Text;

namespace QDFeedParser
{
    /// <summary>
    /// Exception thrown when the feed cannot be found and thus cannot be parsed by a FeedFactory.
    /// </summary>
    public class MissingFeedException : Exception
    {
        public MissingFeedException(string message) : base(message)
        {
        }

        public MissingFeedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Exception thrown when an ISyndicationFeed object is unable to parse a child node or a header from an existing document.
    /// </summary>
    public class InvalidFeedXmlException : Exception
    {
        public InvalidFeedXmlException(string message)
            : base(message)
        {
        }

        public InvalidFeedXmlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

}
