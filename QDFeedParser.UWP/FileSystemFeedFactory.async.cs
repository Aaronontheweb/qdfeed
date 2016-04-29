using System;
using System.IO;
using System.Threading.Tasks;

namespace QDFeedParser
{
    public partial class FileSystemFeedFactory
    {
        public override Task<bool> PingFeedAsync(Uri feeduri)
        {
            //TODO: Verificar com tratar acesso ao System.IO aqui
            return Task.Run(() => false);
            //return Task.Run(() => File.Exists(feeduri.LocalPath));
        }

        public override Task<string> DownloadXmlAsync(Uri feeduri)
        {
            //TODO: Verificar com tratar acesso ao System.IO aqui
            return Task.Run(() => String.Empty);
            //return Task.Run(() => DownloadXml(feeduri));
        }
    }
}
