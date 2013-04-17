using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QDFeedParser
{
    public partial interface IFeedFactory
    {
#if FRAMEWORK
        /// <summary>
        /// Pings the feed to verify that it actually exists.
        /// </summary>
        /// <param name="feeduri">The Uri of the feed to ping.</param>
        /// <returns>True if the feed was successfully pinged, false otherwise.</returns>
        Task<bool> PingFeedAsync(Uri feeduri);
#endif

        /// <summary>
        /// Creates a new ISyndicationFeed class parsed from the provided Uri.
        /// </summary>
        /// <param name="feeduri">The Uri of the feed to be parsed.</param>
        /// <returns>A new ISynidcationFeed object. The exact type returned depends on the type of feed detected.</returns>
        Task<IFeed> CreateFeedAsync(Uri feeduri);

        /// <summary>
        /// Creates a new ISyndicationFeed class of a specific type from the provided Uri. Method will throw an error
        /// if the type specified and the type actually detected in the document don't match.
        /// </summary>
        /// <param name="feeduri">The Uri of the syndication feed to parse.</param>
        /// <param name="feedtype">The type of syndication feed.</param>
        /// <returns>A new ISyndicationFeed object of type [feedtype].</returns>
        Task<IFeed> CreateFeedAsync(Uri feeduri, FeedType feedtype);

#if FRAMEWORK

        /// <summary>
        /// Downloads the XML content of the feed and returns it as a string.
        /// </summary>
        /// <param name="feeduri">The Uri of the feed to parse.</param>
        /// <returns>A string containing the XML document in its entirety.</returns>
        Task<string> DownloadXmlAsync(Uri feeduri);


        /// <summary>
        /// Quickly parses the top-most XML to determine what type of syndication feed the feed hosted
        /// at [feeduri] is.
        /// </summary>
        /// <param name="feeduri">The Uri of the feed to parse.</param>
        /// <returns>The type of feed located at [feeduri].</returns>
        Task<FeedType> CheckFeedTypeAsync(Uri feeduri);
#endif
    }
}
