using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;

namespace QDFeedParser.Silverlight.Tests
{
    public class SilverlightTestFileLoader
    {
        public static IList<string> SampleRssFeeds()
        {
            return new[] { "Aaronontheweb-RSS.xml", "del.icio.us-RSS.xml" };
        }

        public static IList<string> SampleAtomFeeds()
        {
            return new[] { "ScottGu-Atom.xml", "GoogleNews-Atom.xml" };
        }

        public static string ReadFeedContents(string path)
        {
            using(var stream = typeof(SilverlightTestFileLoader).Assembly.GetManifestResourceStream(string.Format("QDFeedParser.Silverlight.Tests.TestFiles.{0}", path)))
            {
                using(var streamreader = new StreamReader(stream))
                {
                    return streamreader.ReadToEnd();
                }
            }
        }

        public static void WriteFeedToIsolatedStorage(string feedxml, Uri feedUri)
        {
            using(var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                CreateDirectoryTree(feedUri, storage);

                using(var filestream = new IsolatedStorageFileStream(feedUri.OriginalString, FileMode.Create, FileAccess.Write, storage))
                {
                    var data = Encoding.UTF8.GetBytes(feedxml);

                    filestream.Write(data, 0, data.Count());
                }
            }
        }

        public static void CreateDirectoryTree(Uri feedUri)
        {
            using(var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                CreateDirectoryTree(feedUri, storage);
            }
        }

        public static void CreateDirectoryTree(Uri feedUri, IsolatedStorageFile storage)
        {
            if (!feedUri.OriginalString.Contains('\\')) return;

            var directory = GetDirectoryPath(feedUri);

            if (!storage.DirectoryExists(directory)) 
                storage.CreateDirectory(directory);
        }

        public static string GetDirectoryPath(Uri feedUri)
        {
            if (!feedUri.OriginalString.Contains('\\')) return string.Empty;

            var directoryPos = feedUri.OriginalString.LastIndexOf('\\');
            return feedUri.OriginalString.Substring(0, directoryPos);
        }
    }

}
