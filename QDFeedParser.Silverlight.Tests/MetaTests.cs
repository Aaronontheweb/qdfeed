using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Silverlight.Testing.UnitTesting.Metadata.VisualStudio;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QDFeedParser.Silverlight.Tests
{
    [TestClass]
    public class MetaTests
    {
        private const string SimpleFileUri = "test.xml";
        private const string ComplexFileUri = @"test2\dir1\dir2\dir3\test.xml";
        private IFeedFactory _factory;

        [ClassInitialize]
        public void Initialize()
        {
            _factory = new IsolatedStorageFeedFactory();
        }

        [ClassCleanup]
        public void Cleanup()
        {
            using(var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if(store.FileExists(SimpleFileUri))
                {
                    store.DeleteFile(SimpleFileUri);
                }

                if(store.FileExists(ComplexFileUri))
                {
                    store.DeleteFile(ComplexFileUri);
                }
            }
        }

        [TestMethod]
        public void CanFindRssTestFiles()
        {
            var items = SilverlightTestFileLoader.SampleRssFeeds();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public void CanLoadRssTestFiles()
        {
            try
            {
                var items = SilverlightTestFileLoader.SampleRssFeeds();
                IList<string> parsedItems = items.Select(SilverlightTestFileLoader.ReadFeedContents).ToList();

                Assert.IsTrue(parsedItems.Count > 0);
                Assert.IsTrue(parsedItems[0].Length > 0);
            }
            catch(Exception ex)
            {
                var debugEx = ex;
                Assert.Fail(string.Format("Caught error {0}", ex.Message));
            }

            
        }

        [TestMethod]
        public void CanFindAtomTestFiles()
        {
            var items = SilverlightTestFileLoader.SampleAtomFeeds();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public void CanParseAtomTestFiles()
        {
            try
            {
                var items = SilverlightTestFileLoader.SampleAtomFeeds();
                IList<string> parsedItems = items.Select(SilverlightTestFileLoader.ReadFeedContents).ToList();
                Assert.IsTrue(parsedItems.Count > 0);
                Assert.IsTrue(parsedItems[0].Length > 0);
            }
            catch (Exception ex)
            {
                var debugEx = ex;
                Assert.Fail(string.Format("Caught error {0}", ex.Message));
            }
            
        }

        [TestMethod]
        public void CanWriteFeedsToIsolatedStorageWithSimplePath()
        {
            var items = SilverlightTestFileLoader.SampleRssFeeds();
            var feedXml = SilverlightTestFileLoader.ReadFeedContents(items[0]);
            var feedPath = new Uri(SimpleFileUri, UriKind.Relative);

            SilverlightTestFileLoader.WriteFeedToIsolatedStorage(feedXml, feedPath);

            using(var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                Assert.IsTrue(store.FileExists(feedPath.OriginalString));
            }

        }

        [TestMethod]
        public void CanWriteFeedsToIsolatedStorageWithComplexPath()
        {
            try
            {
                var items = SilverlightTestFileLoader.SampleRssFeeds();
                var feedXml = SilverlightTestFileLoader.ReadFeedContents(items[0]);
                var feedPath = new Uri(ComplexFileUri, UriKind.Relative);

                SilverlightTestFileLoader.WriteFeedToIsolatedStorage(feedXml, feedPath);

                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    Assert.IsTrue(store.FileExists(feedPath.OriginalString));

                }
            }
            catch(Exception ex)
            {
                var debugEx = ex;
                Assert.Fail(string.Format("Caught error {0}", ex.Message));
            }

        }

    }
}
