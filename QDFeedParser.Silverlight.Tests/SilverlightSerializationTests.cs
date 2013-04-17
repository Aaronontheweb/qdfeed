using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace QDFeedParser.Silverlight.Tests
{
    [TestClass]
    public class SilverlightSerializationTests
    {
        private IFeedFactory _factory;
        private IFeed _rssDocument;
        private IFeed _atomDocument;

        private const string SimpleRssPath = "serializationrss.xml";
        private const string JsonRssPath = "serializationrss.json";
        private const string SimpleAtomPath = "serializationatom.xml";
        private const string JsonAtomPath = "serializationatom.json";

        [ClassInitialize]
        public void Initialize()
        {
            _factory = new IsolatedStorageFeedFactory();
            _rssDocument = _factory.CreateFeed(new Uri(SilverlightTestFileLoader.SampleRssFeeds().First(), UriKind.Relative), FeedType.Rss20,
                                SilverlightTestFileLoader.ReadFeedContents(SilverlightTestFileLoader.SampleRssFeeds().First()));

            _atomDocument = _factory.CreateFeed(new Uri(SilverlightTestFileLoader.SampleAtomFeeds().First(), UriKind.Relative), FeedType.Atom10,
                                SilverlightTestFileLoader.ReadFeedContents(SilverlightTestFileLoader.SampleAtomFeeds().First()));

        }

        #region XML Serialization Tests

        [TestMethod]
        public void CanSerializeRssFeedToXml()
        {
            var serializer = new XmlSerializer(_rssDocument.GetType());
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var filestream = new IsolatedStorageFileStream(SimpleRssPath, FileMode.Create, storage))
                {
                    serializer.Serialize(filestream, _rssDocument);
                }

                Assert.IsTrue(storage.FileExists(SimpleRssPath));
                Assert.IsTrue(storage.OpenFile(SimpleRssPath, FileMode.Open).Length > 0);
            }
        }

        [TestMethod]
        public void CanDeserializeRssFeedFromXml()
        {
            var serializer = new XmlSerializer(_rssDocument.GetType());
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storage.FileExists(SimpleRssPath))
                {
                    using (var filestream = new IsolatedStorageFileStream(SimpleRssPath, FileMode.Create, storage))
                    {
                        serializer.Serialize(filestream, _rssDocument);
                    }
                }

                using (var filestream = storage.OpenFile(SimpleRssPath, FileMode.Open))
                {
                    using (var reader = XmlReader.Create(filestream))
                    {
                        Assert.IsTrue(serializer.CanDeserialize(reader));
                        var resultantObject = serializer.Deserialize(reader);

                        //Assert that the result object is of the expected type (some derivative of IFeed)
                        Assert.IsInstanceOfType(resultantObject, _rssDocument.GetType());

                        //Assert that the two objects are distinct instances
                        Assert.AreNotSame(_rssDocument, resultantObject);

                        //Cast the object back into an IFeed and perform some specific assertions
                        var resultantFeed = resultantObject as IFeed;
                        Assert.AreEqual(_rssDocument.Title, resultantFeed.Title);
                        Assert.AreEqual(_rssDocument.LastUpdated, resultantFeed.LastUpdated);
                        Assert.AreEqual(_rssDocument.FeedUri, resultantFeed.FeedUri);
                        Assert.AreEqual(_rssDocument.FeedType, resultantFeed.FeedType);
                        Assert.AreEqual(_rssDocument.Items.Count, resultantFeed.Items.Count);
                    }
                }
            }
        }

        [TestMethod]
        public void CanSerializeAtomFeedToXml()
        {
            var serializer = new XmlSerializer(_atomDocument.GetType());
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var filestream = new IsolatedStorageFileStream(SimpleAtomPath, FileMode.Create, storage))
                {
                    serializer.Serialize(filestream, _atomDocument);
                }

                Assert.IsTrue(storage.FileExists(SimpleAtomPath));
                Assert.IsTrue(storage.OpenFile(SimpleAtomPath, FileMode.Open).Length > 0);
            }
        }

        [TestMethod]
        public void CanDeserializeAtomFeedFromXml()
        {
            var serializer = new XmlSerializer(_atomDocument.GetType());
            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storage.FileExists(SimpleAtomPath))
                {
                    using (var filestream = new IsolatedStorageFileStream(SimpleAtomPath, FileMode.Create, storage))
                    {
                        serializer.Serialize(filestream, _atomDocument);
                    }
                }

                using (var filestream = storage.OpenFile(SimpleAtomPath, FileMode.Open))
                {
                    using (var reader = XmlReader.Create(filestream))
                    {
                        Assert.IsTrue(serializer.CanDeserialize(reader));
                        var resultantObject = serializer.Deserialize(reader);

                        //Assert that the result object is of the expected type (some derivative of IFeed)
                        Assert.IsInstanceOfType(resultantObject, _atomDocument.GetType());

                        //Assert that the two objects are distinct instances
                        Assert.AreNotSame(_atomDocument, resultantObject);

                        //Cast the object back into an IFeed and perform some specific assertions
                        var resultantFeed = resultantObject as IFeed;
                        Assert.AreEqual(_atomDocument.Title, resultantFeed.Title);
                        Assert.AreEqual(_atomDocument.LastUpdated, resultantFeed.LastUpdated);
                        Assert.AreEqual(_atomDocument.FeedUri, resultantFeed.FeedUri);
                        Assert.AreEqual(_atomDocument.FeedType, resultantFeed.FeedType);
                        Assert.AreEqual(_atomDocument.Items.Count, resultantFeed.Items.Count);
                    }
                }
            }
        }

        #endregion

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
    }
}
