using System;
using System.Collections.Generic;

using System.Text;

namespace QDFeedParser
{
    /// <summary>
    /// Interface used to represent the common elements between all entries in ATOM / RSS syndication feeds.
    /// </summary>
    public interface IFeedItem
    {
        /// <summary>
        /// The title of the synidcation feed entry.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The author of the syndication feed entry.
        /// </summary>
        string Author { get; set; }

        /// <summary>
        /// The unique ID of this syndication feed entry. Depending upon how the service uses it, it can be a URI, a
        /// Guid, and lord knows else what. Read the RSS specification if you don't believe me :p
        /// <link>http://cyber.law.harvard.edu/rss/rss.html#ltguidgtSubelementOfLtitemgt</link>
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// A Uri which points to the syndication item.
        /// </summary>
        string Link { get; set; }

        /// <summary>
        /// The UTC date and time when the syndication feed entry was published.
        /// </summary>
        DateTime DatePublished { get; set; }

        /// <summary>
        /// The text content of the syndication feed.
        /// </summary>
        string Content { get; set; }

        /// <summary>
        /// A string of categories used to classify the syndication feed entry.
        /// </summary>
        IList<string> Categories { get; }
    }
}
