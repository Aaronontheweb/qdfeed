using System;
using System.IO;
using System.IO.IsolatedStorage;
using QDFeedParser.Silverlight.Extensions;
using QDFeedParser.Xml;

namespace QDFeedParser
{
    public partial class IsolatedStorageFeedFactory : BaseFeedFactory
    {
        public IsolatedStorageFeedFactory()
            : this(new LinqFeedXmlParser())
        { }

        public IsolatedStorageFeedFactory(IFeedXmlParser parser)
            : this(parser, null)
        { }

        public IsolatedStorageFeedFactory(IFeedInstanceProvider instanceProvider)
            : this(new LinqFeedXmlParser(), instanceProvider)
        { }

        public IsolatedStorageFeedFactory(IFeedXmlParser parser, IFeedInstanceProvider instanceProvider)
            : base(parser, instanceProvider)
        { }

        public bool PingFeed(Uri feeduri, IsolatedStorageFile storage)
        {
            return storage.FileExists(feeduri.OriginalString);
        }

        public bool PingFeed(Uri feeduri)
        {
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return PingFeed(feeduri, storage);
            }
        }

        #region Overrides of BaseFeedFactory

        public override IAsyncResult BeginDownloadXml(Uri feeduri, AsyncCallback callback)
        {
#if !WINDOWS_PHONE
            if (!IsolatedStorageFile.IsEnabled) throw new MissingFeedException("IsolatedStorage is not enabled! Cannot access files from IsolatedStorage!");
#endif


            try
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if(!PingFeed(feeduri, store)) throw new MissingFeedException(string.Format("Could not find feed at location {0}", feeduri));

                    using (var stream = new IsolatedStorageFileStream(feeduri.OriginalString, FileMode.Open,
                                                                   FileAccess.Read, store))
                    {
                        using(var reader = new StreamReader(stream))
                        {
                            var strOutput = reader.ReadToEnd();
                            //Fake the async result
                            var result = new AsyncResult<FeedTuple>(callback, new FeedTuple { FeedContent = strOutput, FeedUri = feeduri }, true);
                            if (callback != null) callback.Invoke(result);
                            return result;
                        }
                    }
                }
            }
            catch (IsolatedStorageException ex)
            {
                throw new MissingFeedException(string.Format("Unable to open feed at {0}", feeduri), ex);
            }
        }

        public override FeedTuple EndDownloadXml(IAsyncResult asyncResult)
        {
            var feedResult = asyncResult.AsyncState as FeedTuple;
            return feedResult;
        }

        #endregion
    }
}
