using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using QDFeedParser.Xml;

namespace QDFeedParser
{
    public partial class HttpFeedFactory : BaseFeedFactory
    {

        public HttpFeedFactory()
            : this(new LinqFeedXmlParser())
        { }

        public HttpFeedFactory(IFeedXmlParser parser)
            : this(parser, null)
        { }

        public HttpFeedFactory(IFeedInstanceProvider instanceProvider)
            : this(new LinqFeedXmlParser(), instanceProvider)
        { }

        public HttpFeedFactory(IFeedXmlParser parser, IFeedInstanceProvider instanceProvider)
            : base(parser, instanceProvider)
        { }


#if FRAMEWORK

        public override bool PingFeed(Uri feeduri)
        {
            try
            {
                var request = WebRequest.Create(feeduri) as HttpWebRequest;
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    return IsValidXmlReponse(response);
                }
            }
            /* Usually this means we encountered a 404 / 501 error of some sort. */
            catch (WebException)
            {
                return false;
            }
        }

        public override string DownloadXml(Uri feeduri)
        {
            try
            {
                var request = WebRequest.Create(feeduri) as HttpWebRequest;
                request.KeepAlive = false;
                string responseXml;

                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    responseXml = GetResponseXml(response);
                }

                return responseXml;
            }
            /* Usually this means we encountered a 404 / 501 error of some sort. */
            catch (WebException ex)
            {
                throw new MissingFeedException(string.Format("Was unable to open web-hosted file {0}", feeduri.LocalPath), ex);
            }
        }

#endif
        private static string GetResponseXml(HttpWebResponse response)
        {
            var reader = new StreamReader(response.GetResponseStream());
            var responseXml = reader.ReadToEnd();
            return responseXml;
        }

        public override IAsyncResult BeginDownloadXml(Uri feeduri, AsyncCallback callback)
        {
            try
            {
                var request = WebRequest.Create(feeduri) as HttpWebRequest;
                var requestState = new AsyncFeedRequestState { FeedUri = feeduri, OriginalRequest = request };
                return request.BeginGetResponse(callback, requestState);
            }
            /* Usually this means we encountered a 404 / 501 error of some sort. */
            catch (WebException ex)
            {
                throw new MissingFeedException(string.Format("Was unable to open web-hosted file {0}", feeduri.LocalPath), ex);
            }
        }

        public override FeedTuple EndDownloadXml(IAsyncResult asyncResult)
        {
            try
            {
                var requestState = asyncResult.AsyncState as AsyncFeedRequestState;
                var response = requestState.OriginalRequest.EndGetResponse(asyncResult) as HttpWebResponse;
                var responseXml = GetResponseXml(response);

                return new FeedTuple { FeedContent = responseXml, FeedUri = requestState.FeedUri };
            }
            catch (WebException ex)
            {
                throw new MissingFeedException("Was unable to open web-hosted file {0}", ex);
            }
        }

        #region useUnsafeHeaderParsing configuration hack

#if FRAMEWORK

        /* This is a nasty hack IMHO, but this is the only way I can get HttpWebRequest to successfully download some
        * perfectly valid feeds like http://news.ycombinator.com/rss without it choking on the infamous "server comitted a protocol violation"
        * error message. The static method called in this static constructor effectively opens the gate so all HTTP GET requests can be parsed
        * any URI regardless of whether or not the header is mal-formed.
        * */
        static HttpFeedFactory()
        {
            SetUseUnsafeHeaderParsing(true);
        }

        /// <summary>
        /// Private, static method used for modifying the HttpWebRequest configuration at run-time using reflection.
        /// This allows parsing of "unsafe" (aka "not RFC compliant") headers in order to suppress errors for valid feeds.
        /// It only needs to be called once, and this is done by the static constructor on this very class.
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool SetUseUnsafeHeaderParsing(bool b)
        {
            var a = Assembly.GetAssembly(typeof(System.Net.Configuration.SettingsSection));
            if (a == null) return false;

            var t = a.GetType("System.Net.Configuration.SettingsSectionInternal");
            if (t == null) return false;

            var o = t.InvokeMember("Section",
                BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });
            if (o == null) return false;

            var f = t.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
            if (f == null) return false;

            f.SetValue(o, b);

            return true;
        }
#endif
        #endregion

        private static bool IsValidXmlReponse(HttpWebResponse response)
        {
            return response != null &&
                   response.StatusCode == HttpStatusCode.OK &&
                   response.ContentType.Contains("xml");
        }
    }
}
