using System;
using System.IO;
using QDFeedParser.Xml;

namespace QDFeedParser
{
    public partial class FileSystemFeedFactory : BaseFeedFactory
    {
        public FileSystemFeedFactory()
            : this(new LinqFeedXmlParser())
        { }

        public FileSystemFeedFactory(IFeedXmlParser parser)
            : this(parser, null)
        { }

        public FileSystemFeedFactory(IFeedInstanceProvider instanceProvider)
            : this(new LinqFeedXmlParser(), instanceProvider)
        { }

        public FileSystemFeedFactory(IFeedXmlParser parser, IFeedInstanceProvider instanceProvider)
            : base(parser, instanceProvider)
        { }

        public override bool PingFeed(Uri feeduri)
        {
            //TODO: Verificar com tratar acesso ao System.IO aqui
            return false;
            //return File.Exists(feeduri.LocalPath);
        }

        public override string DownloadXml(Uri feeduri)
        {
            if (!this.PingFeed(feeduri)) throw new MissingFeedException(string.Format("Was unable to open local XML file {0}", feeduri.LocalPath));

            return DownloadXmlFromUri(feeduri).FeedContent;
        }

        //Hacking asynchronous file IO just to make the interface consistent - there's not much performance benefit otheriwse
        public override IAsyncResult BeginDownloadXml(Uri feeduri, AsyncCallback callback)
        {
            if (!this.PingFeed(feeduri)) throw new MissingFeedException(string.Format("Was unable to open local XML file {0}", feeduri.LocalPath));

            return FeedWorkerDelegate.BeginInvoke(feeduri, callback, new FeedTuple());

        }

        public override FeedTuple EndDownloadXml(IAsyncResult asyncResult)
        {
            var result = FeedWorkerDelegate.EndInvoke(asyncResult);
            return result;
        }


        protected readonly FileIOWorker FeedWorkerDelegate = new FileIOWorker(DownloadXmlFromUri);

        protected delegate FeedTuple FileIOWorker(Uri feeduri);

        /// <summary>
        /// Requires a valid uri on local disk - otherwise the method will promptly fail.
        /// </summary>
        /// <param name="feeduri">A valid uri on local disk</param>
        /// <returns>The xml content of the uri</returns>
        protected static FeedTuple DownloadXmlFromUri(Uri feeduri)
        {
            //TODO: Verificar com tratar acesso ao System.IO aqui
            return null;
            //string xmlContent;
            //using (var reader = new StreamReader(feeduri.OriginalString))
            //{
            //    xmlContent = reader.ReadToEnd();
            //}

            //return new FeedTuple { FeedContent = xmlContent, FeedUri = feeduri };
        }
    }
}
