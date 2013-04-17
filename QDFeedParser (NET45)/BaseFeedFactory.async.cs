using System;
using System.Threading.Tasks;

namespace QDFeedParser
{
    public partial class BaseFeedFactory
    {
#if FRAMEWORK || WP8
        public abstract Task<bool> PingFeedAsync(Uri feeduri);

        public abstract Task<string> DownloadXmlAsync(Uri feeduri); 
#endif

        public async Task<IFeed> CreateFeedAsync(Uri feeduri)
        {
            var feedXml = await DownloadXmlAsync(feeduri);

            var feedtype = CheckFeedType(feedXml);
            return CreateFeed(feeduri, feedtype, feedXml);
        }

        public async Task<IFeed> CreateFeedAsync(Uri feeduri, FeedType feedtype)
        {
            var feedXml = await DownloadXmlAsync(feeduri);

            return CreateFeed(feeduri, feedtype, feedXml);
        }

        public async Task<FeedType> CheckFeedTypeAsync(Uri feeduri)
        {
            var feedXml = await DownloadXmlAsync(feeduri);
            return CheckFeedType(feedXml);
        }
    }
}
