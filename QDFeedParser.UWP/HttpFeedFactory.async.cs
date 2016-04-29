using System;
using System.Net;
using System.Threading.Tasks;
#if WINDOWS_PHONE
using QDFeedParser.Extensions;
#endif

namespace QDFeedParser
{
    public partial class HttpFeedFactory
    {
        public override async Task<bool> PingFeedAsync(Uri feeduri)
        {
            try
            {
                var request = WebRequest.Create(feeduri) as HttpWebRequest;
                var response = await request.GetResponseAsync() as HttpWebResponse;

                return IsValidXmlReponse(response);
            }
            catch (WebException)
            {
                return false;
            }
        }

        public override async Task<string> DownloadXmlAsync(Uri feeduri)
        {
            try
            {
                var request = WebRequest.Create(feeduri) as HttpWebRequest;

                using (var response = await request.GetResponseAsync() as HttpWebResponse)
                {
                    return GetResponseXml(response);
                }
            }
            /* Usually this means we encountered a 404 / 501 error of some sort. */
            catch (WebException ex)
            {
                throw new MissingFeedException(string.Format("Was unable to open web-hosted file {0}", feeduri.LocalPath), ex);
            }
        }
    }
}
