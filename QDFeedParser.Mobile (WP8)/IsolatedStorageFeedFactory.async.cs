using System;
using System.Threading.Tasks;

namespace QDFeedParser
{
    public partial class IsolatedStorageFeedFactory
    {
        public override Task<bool> PingFeedAsync(Uri feeduri)
        {
            return Task.Run(() => PingFeed(feeduri));
        }

        public override Task<string> DownloadXmlAsync(Uri feeduri)
        {
            return null;
        }
    }
}
