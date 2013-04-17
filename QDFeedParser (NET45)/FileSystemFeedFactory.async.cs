using System;
using System.IO;
using System.Threading.Tasks;

namespace QDFeedParser
{
    public partial class FileSystemFeedFactory
    {
        public override Task<bool> PingFeedAsync(Uri feeduri)
        {
            return Task.Run(() => File.Exists(feeduri.LocalPath));
        }

        public override Task<string> DownloadXmlAsync(Uri feeduri)
        {
            return Task.Run(() => DownloadXml(feeduri));
        }
    }
}
