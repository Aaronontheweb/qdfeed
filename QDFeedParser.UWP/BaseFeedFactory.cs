using System;
using System.Collections.Generic;
using System.Xml;
using System.Text;
using QDFeedParser.Xml;

namespace QDFeedParser
{
    public abstract partial class BaseFeedFactory : IFeedFactory
    {

        #region protected members

        protected IFeedXmlParser _parser;
        private readonly IFeedInstanceProvider _instanceProvider;

        #endregion

        #region constructors

        protected BaseFeedFactory(IFeedXmlParser parser)
        {
            _parser = parser;
        }

        protected BaseFeedFactory(IFeedXmlParser parser, IFeedInstanceProvider instanceProvider)
            :this(parser)
        {
            _instanceProvider = instanceProvider;
            _instanceProvider = instanceProvider ?? new DefaultFeedInstanceProvider();
        }

        #endregion

        #region abstract IFeedFactory members

#if FRAMEWORK

        public abstract bool PingFeed(Uri feeduri);


        public abstract string DownloadXml(Uri feeduri);
#endif

#if WINDOWS_UWP
        //TODO: Verificar como fazer para segregar por esse tipo de Projeto, tentei o "#if PORTABLE" não não funcionou
        public abstract bool PingFeed(Uri feeduri);
        //TODO: Verificar como fazer para segregar por esse tipo de Projeto, tentei o "#if PORTABLE" não não funcionou
        public abstract string DownloadXml(Uri feeduri);
#endif

        public abstract IAsyncResult BeginDownloadXml(Uri feeduri, AsyncCallback callback);

        public abstract FeedTuple EndDownloadXml(IAsyncResult asyncResult);

        #endregion

        #region IFeedFactory Members



        public IFeed CreateFeed(Uri feeduri)
        {
#if FRAMEWORK
            var feedxml = this.DownloadXml(feeduri);
#endif

#if SILVERLIGHT || WINDOWS_PHONE
            var feedXmlResult = this.BeginDownloadXml(feeduri, null);
            var feedxml = this.EndDownloadXml(feedXmlResult).FeedContent;
#endif

            //TODO: Verificar como fazer para segregar por esse tipo de Projeto, tentei o "#if PORTABLE" não não funcionou
            var feedXmlResult = this.BeginDownloadXml(feeduri, null);
            var feedxml = this.EndDownloadXml(feedXmlResult).FeedContent;

            var feedtype = this.CheckFeedType(feedxml);
            return this.CreateFeed(feeduri, feedtype, feedxml);

        }

        public IFeed CreateFeed(Uri feeduri, FeedType feedtype)
        {
#if FRAMEWORK
            var feedxml = this.DownloadXml(feeduri);
#endif

#if SILVERLIGHT || WINDOWS_PHONE
            var feedXmlResult = this.BeginDownloadXml(feeduri, null);
            var feedxml = this.EndDownloadXml(feedXmlResult).FeedContent;
#endif
            //TODO: Verificar como fazer para segregar por esse tipo de Projeto, tentei o "#if PORTABLE" não não funcionou
            var feedXmlResult = this.BeginDownloadXml(feeduri, null);
            var feedxml = this.EndDownloadXml(feedXmlResult).FeedContent;

            return this.CreateFeed(feeduri, feedtype, feedxml);
        }



        public IFeed CreateFeed(Uri feeduri, FeedType feedtype, string feedxml)
        {
            try
            {
                IFeed returnFeed;
                if (feedtype == FeedType.Atom10)
                {
                    returnFeed = _instanceProvider.CreateAtom10Feed(feeduri.OriginalString);
                }
                else
                {
                    returnFeed = _instanceProvider.CreateRss20Feed(feeduri.OriginalString);
                }

                try
                {
                    _parser.ParseFeed(returnFeed, feedxml);
                }
                catch (System.Xml.XmlException ex)
                {
                    throw new InvalidFeedXmlException(string.Format("The xml for feed {0} is invalid", feeduri), ex);
                }


                return returnFeed;
            }
            catch (XmlException ex)
            {
                throw new InvalidFeedXmlException(string.Format("Invalid XML for feed {0}", feeduri.AbsoluteUri), ex);
            }
        }

#if FRAMEWORK

        public FeedType CheckFeedType(Uri feeduri)
        {
            try
            {
                var strXmlContent = this.DownloadXml(feeduri);
                return this.CheckFeedType(strXmlContent);
            }
            catch (MissingFeedException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidFeedXmlException(
                    string.Format("Unable to parse feedtype for feed at {0}", feeduri.AbsoluteUri), ex);
            }
        }

#endif

        public FeedType CheckFeedType(string feedxml)
        {
            try
            {
                return _parser.CheckFeedType(feedxml);
            }
            catch (XmlException ex)
            {
                throw new InvalidFeedXmlException("Unable to parse feedtype from feed", ex);
            }
        }

#endregion

#region Asynchronous methods

        public IAsyncResult BeginCreateFeed(Uri feeduri, AsyncCallback callback)
        {
            return BeginDownloadXml(feeduri, callback);
        }

        public IFeed EndCreateFeed(IAsyncResult asyncResult)
        {
            var feedData = EndDownloadXml(asyncResult);
            var feedType = CheckFeedType(feedData.FeedContent);
            return CreateFeed(feedData.FeedUri, feedType, feedData.FeedContent);
        }

#endregion

    }
}
