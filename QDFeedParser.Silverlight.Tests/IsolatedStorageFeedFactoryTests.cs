using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QDFeedParser.Silverlight.Tests
{
    [TestClass]
    public class IsolatedStorageFeedFactoryTests
    {
        private IFeedFactory _factory;
        private string _rssDocument;
        private string _atomDocument;
        private const string ComplexRssPath = "testdir\\dir\\dir2\\dir3\\rss.xml";
        private const string SimpleRssPath = "rss.xml";

        private const string ComplexAtomPath = "testdiratom\\dir\\dir2\\dir3\\atom.xml";
        private const string SimpleAtomPath = "atom.xml";

        [ClassInitialize]
        public void Initialize()
        {
            _factory = new IsolatedStorageFeedFactory();
            _rssDocument =
                SilverlightTestFileLoader.ReadFeedContents(SilverlightTestFileLoader.SampleRssFeeds().First());
            _atomDocument =
                SilverlightTestFileLoader.ReadFeedContents(SilverlightTestFileLoader.SampleAtomFeeds().First());
            SilverlightTestFileLoader.WriteFeedToIsolatedStorage(_rssDocument, new Uri(SimpleRssPath, UriKind.Relative));
            SilverlightTestFileLoader.WriteFeedToIsolatedStorage(_rssDocument, new Uri(ComplexRssPath, UriKind.Relative));
            SilverlightTestFileLoader.WriteFeedToIsolatedStorage(_atomDocument, new Uri(SimpleAtomPath, UriKind.Relative));
            SilverlightTestFileLoader.WriteFeedToIsolatedStorage(_atomDocument, new Uri(ComplexAtomPath, UriKind.Relative));
        }

        [ClassCleanup]
        public void Cleanup()
        {
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                try
                {
                    //Flushes everything out of IsolatedStorage
                    store.Remove();
                }
                catch (Exception ex)
                {
                    var debugEx = ex;
                }

            }
        }

        #region RSS Tests

        [TestMethod]
        public void CanDownloadXmlForRssFileWithSimplePath()
        {
            var asyncResult = _factory.BeginDownloadXml(new Uri(SimpleRssPath, UriKind.Relative), null);

            Assert.IsNotNull(asyncResult.AsyncState);

            var resultFeed = _factory.EndDownloadXml(asyncResult);

            Assert.IsNotNull(resultFeed);
            Assert.IsTrue(resultFeed.FeedContent.Length > 0);
        }

        [TestMethod]
        public void CanCreateIFeedObjectForRssFileWithSimplePath()
        {
            var asyncResult = _factory.BeginCreateFeed(new Uri(SimpleRssPath, UriKind.Relative), null);

            Assert.IsNotNull(asyncResult.AsyncState);

            var resultFeed = _factory.EndCreateFeed(asyncResult);

            Assert.IsNotNull(resultFeed);
            Assert.IsTrue(resultFeed.Title.Length > 0);
            Assert.IsTrue(resultFeed.Items.Count > 0);
        }

        [TestMethod]
        public void CanDownloadXmlForRssFileWithComplexPath()
        {
            var asyncResult = _factory.BeginDownloadXml(new Uri(ComplexRssPath, UriKind.Relative), null);

            Assert.IsNotNull(asyncResult.AsyncState);

            var resultFeed = _factory.EndDownloadXml(asyncResult);

            Assert.IsNotNull(resultFeed);
            Assert.IsTrue(resultFeed.FeedContent.Length > 0);
        }

        [TestMethod]
        public void CanCreateIFeedObjectForRssFileWithComplexPath()
        {
            var asyncResult = _factory.BeginCreateFeed(new Uri(ComplexRssPath, UriKind.Relative), null);

            Assert.IsNotNull(asyncResult.AsyncState);

            var resultFeed = _factory.EndCreateFeed(asyncResult);

            Assert.IsNotNull(resultFeed);
            Assert.IsTrue(resultFeed.Title.Length > 0);
            Assert.IsTrue(resultFeed.Items.Count > 0);
        }

        #endregion

        #region ATOM TESTS

        /* Atom tests */

        [TestMethod]
        public void CanDownloadXmlForAtomFileWithSimplePath()
        {
            var asyncResult = _factory.BeginDownloadXml(new Uri(SimpleAtomPath, UriKind.Relative), null);

            Assert.IsNotNull(asyncResult.AsyncState);

            var resultFeed = _factory.EndDownloadXml(asyncResult);

            Assert.IsNotNull(resultFeed);
            Assert.IsTrue(resultFeed.FeedContent.Length > 0);
        }

        [TestMethod]
        public void CanCreateIFeedObjectForAtomFileWithSimplePath()
        {
            var asyncResult = _factory.BeginCreateFeed(new Uri(SimpleAtomPath, UriKind.Relative), null);

            Assert.IsNotNull(asyncResult.AsyncState);

            var resultFeed = _factory.EndCreateFeed(asyncResult);

            Assert.IsNotNull(resultFeed);
            Assert.IsTrue(resultFeed.Title.Length > 0);
            Assert.IsTrue(resultFeed.Items.Count > 0);
        }

        [TestMethod]
        public void CanDownloadXmlForAtomFileWithComplexPath()
        {
            var asyncResult = _factory.BeginDownloadXml(new Uri(ComplexAtomPath, UriKind.Relative), null);

            Assert.IsNotNull(asyncResult.AsyncState);

            var resultFeed = _factory.EndDownloadXml(asyncResult);

            Assert.IsNotNull(resultFeed);
            Assert.IsTrue(resultFeed.FeedContent.Length > 0);
        }

        [TestMethod]
        public void CanCreateIFeedObjectForAtomFileWithComplexPath()
        {
            var asyncResult = _factory.BeginCreateFeed(new Uri(ComplexAtomPath, UriKind.Relative), null);

            Assert.IsNotNull(asyncResult.AsyncState);

            var resultFeed = _factory.EndCreateFeed(asyncResult);

            Assert.IsNotNull(resultFeed);
            Assert.IsTrue(resultFeed.Title.Length > 0);
            Assert.IsTrue(resultFeed.Items.Count > 0);
        }
        #endregion
    }
}