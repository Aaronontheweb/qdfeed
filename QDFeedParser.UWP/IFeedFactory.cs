using System;

namespace QDFeedParser
{
    /// <summary>
    /// An interface which defines the factories used for creating SyndicationFeed objects.
    /// The primary responsibility of this class is to download / load the XML into a string and enable the caller
    /// to run some minor tests against it before it comes time to parse it.
    /// </summary>
    public partial interface IFeedFactory
    {

#if FRAMEWORK

        /// <summary>
        /// Pings the feed to verify that it actually exists.
        /// </summary>
        /// <param name="feeduri">The Uri of the feed to ping.</param>
        /// <returns>True if the feed was successfully pinged, false otherwise.</returns>
        bool PingFeed(Uri feeduri);

#endif

        /// <summary>
        /// Creates a new ISyndicationFeed class parsed from the provided Uri.
        /// </summary>
        /// <param name="feeduri">The Uri of the feed to be parsed.</param>
        /// <returns>A new ISynidcationFeed object. The exact type returned depends on the type of feed detected.</returns>
        IFeed CreateFeed(Uri feeduri);

        /// <summary>
        /// Creates a new ISyndicationFeed class of a specific type from the provided Uri. Method will throw an error
        /// if the type specified and the type actually detected in the document don't match.
        /// </summary>
        /// <param name="feeduri">The Uri of the syndication feed to parse.</param>
        /// <param name="feedtype">The type of syndication feed.</param>
        /// <returns>A new ISyndicationFeed object of type [feedtype].</returns>
        IFeed CreateFeed(Uri feeduri, FeedType feedtype);


        /// <summary>
        /// Creates a new ISyndicationFeed class of a specified type from the provided xml string. 
        /// The URI is used as a unique identifier for the feed.
        /// </summary>
        /// <param name="feeduri">The Uri of the syndication feed to parse.</param>
        /// <param name="feedtype">The type of feed.</param>
        /// <param name="feedxml">The xml content of the feed.</param>
        /// <returns>A new ISyndicationFeed object of [feedtype].</returns>
        IFeed CreateFeed(Uri feeduri, FeedType feedtype, string feedxml);

        /// <summary>
        /// Asynchronous operation to begin creating a new feed from a source Uri.
        /// </summary>
        /// <param name="feeduri">The Uri of the feed to be parsed.</param>
        /// <param name="callback">A callback function</param>
        /// <returns>A new ISynidcationFeed object. The exact type returned depends on the type of feed detected.</returns>
        IAsyncResult BeginCreateFeed(Uri feeduri, AsyncCallback callback);

        /// <summary>
        /// Creates a new ISyndicationFeed class as the result of the end of an asynchronous operation.
        /// The original URI is used as the unique identifier for the feed.
        /// </summary>
        /// <param name="asyncResult">The results of the original asynchronous method call.</param>
        /// <returns>A new ISyndicationFeed object</returns>
        IFeed EndCreateFeed(IAsyncResult asyncResult);

#if FRAMEWORK

        /// <summary>
        /// Downloads the XML content of the feed and returns it as a string.
        /// </summary>
        /// <param name="feeduri">The Uri of the feed to parse.</param>
        /// <returns>A string containing the XML document in its entirety.</returns>
        string DownloadXml(Uri feeduri);

#endif

        /// <summary>
        /// Begins an asychronous request to the XML content of the feed and returns it as a string.
        /// </summary>
        /// <param name="feeduri">The Uri of the feed to parse.</param>
        /// <param name="callback">A callback function.</param>
        /// <returns>A string containing the XML document in its entirety.</returns>
        IAsyncResult BeginDownloadXml(Uri feeduri, AsyncCallback callback);

        /// <summary>
        /// Returns the XML content from the end of an asynchronous request.
        /// </summary>
        /// <param name="asyncResult">The asynchronous result passed in via a callback</param>
        /// <returns>The feed's XML at the specified Uri</returns>
        FeedTuple EndDownloadXml(IAsyncResult asyncResult);

#if FRAMEWORK
        /// <summary>
        /// Quickly parses the top-most XML to determine what type of syndication feed the feed hosted
        /// at [feeduri] is.
        /// </summary>
        /// <param name="feeduri">The Uri of the feed to parse.</param>
        /// <returns>The type of feed located at [feeduri].</returns>
        FeedType CheckFeedType(Uri feeduri);
#endif

        /// <summary>
        /// Quickly parses the top-most XML to determine what type of syndication feed [feedxml] is.
        /// </summary>
        /// <param name="feedxml">The Xml content of the feed to parse.</param>
        /// <returns>The type of feed contained in [feedxml].</returns>
        FeedType CheckFeedType(string feedxml);
    }
}
