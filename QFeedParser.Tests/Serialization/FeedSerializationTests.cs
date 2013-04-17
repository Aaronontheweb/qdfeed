using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using NUnit.Framework;

namespace QDFeedParser.Tests.Serialization
{
    [TestFixture, Description("Determines if classes which inherit from IFeed can be serialized to both Json and Xml")]
    public class FeedSerializationTests
    {
        private const string RssXmlFile = "temprssfile.xml";
        private const string AtomXmlFile = "tempatomfile.xml";

        protected IFeedFactory Factory;
        protected FeedType FeedType;

        public FeedSerializationTests()
        {
            Factory = new FileSystemFeedFactory();
            FeedType = FeedType.Rss20;
        }

        #region RSS serialization tests

        [Test, Description("Determines if a simple RSS feed can be serialized to XML.")]
        public void CanSerializeRssFeedToXml()
        {
            var testfeed = TestFileLoader.GetSingleRssTestFilePath(TestFileLoader.TestFileType.FileSys);
            var feed = Factory.CreateFeed(new Uri(testfeed));
            var serializer = new XmlSerializer(feed.GetType());

            using (var filestream = new FileStream(RssXmlFile, FileMode.Create))
            {
                serializer.Serialize(filestream, feed);
            }

            Assert.IsTrue(File.Exists(RssXmlFile));
            Assert.IsTrue(File.ReadAllText(RssXmlFile).Length > 0);
        }

        [Test, Description("Determines if an RSS feed can be deserialized from XML back into an IFeed object.")]
        public void CanDeserializeRssFeedFromXml()
        {
            var testfeed = TestFileLoader.GetSingleRssTestFilePath(TestFileLoader.TestFileType.FileSys);
            var feed = Factory.CreateFeed(new Uri(testfeed));
            var serializer = new XmlSerializer(feed.GetType());

            using (var filestream = new FileStream(RssXmlFile, FileMode.Create))
            {
                serializer.Serialize(filestream, feed);
            }

            using (var readstream = new FileStream(RssXmlFile, FileMode.Open))
            {
                using(var reader = XmlReader.Create(readstream))
                {
                    Assert.IsTrue(serializer.CanDeserialize(reader));
                    var resultantObject = serializer.Deserialize(reader);

                    //Assert that the result object is of the expected type (some derivative of IFeed)
                    Assert.IsInstanceOf(feed.GetType(), resultantObject);

                    //Assert that the two objects are distinct instances
                    Assert.AreNotSame(feed, resultantObject);

                    //Cast the object back into an IFeed and perform some specific assertions
                    var resultantFeed = resultantObject as IFeed;
                    Assert.AreEqual(feed.Title, resultantFeed.Title);
                    Assert.AreEqual(feed.LastUpdated, resultantFeed.LastUpdated);
                    Assert.AreEqual(feed.FeedUri, resultantFeed.FeedUri);
                    Assert.AreEqual(feed.FeedType, resultantFeed.FeedType);
                    Assert.AreEqual(feed.Items.Count, resultantFeed.Items.Count);
                    Assert.Pass("THE FEED CAN BE DESERIALIZED SUCCESSFULLY");
                }
            }
        }

         #endregion

        #region ATOM serialization tests

        [Test, Description("Determines if a simple Atom feed can be serialized to XML.")]
        public void CanSerializeAtomFeedToXml()
        {
            var testfeed = TestFileLoader.GetSingleAtomTestFilePath(TestFileLoader.TestFileType.FileSys);
            var feed = Factory.CreateFeed(new Uri(testfeed));
            var serializer = new XmlSerializer(feed.GetType());

            using (var filestream = new FileStream(AtomXmlFile, FileMode.Create))
            {
                serializer.Serialize(filestream, feed);
            }

            Assert.IsTrue(File.Exists(AtomXmlFile));
            Assert.IsTrue(File.ReadAllText(AtomXmlFile).Length > 0);
        }

        [Test, Description("Determines if an Atom feed can be deserialized from XML back into an IFeed object.")]
        public void CanDeserializeAtomFeedFromXml()
        {
            var testfeed = TestFileLoader.GetSingleAtomTestFilePath(TestFileLoader.TestFileType.FileSys);
            var feed = Factory.CreateFeed(new Uri(testfeed));
            var serializer = new XmlSerializer(feed.GetType());

            using (var filestream = new FileStream(AtomXmlFile, FileMode.Create))
            {
                serializer.Serialize(filestream, feed);
            }

            using (var readstream = new FileStream(AtomXmlFile, FileMode.Open))
            {
                using (var reader = XmlReader.Create(readstream))
                {
                    Assert.IsTrue(serializer.CanDeserialize(reader));
                    var resultantObject = serializer.Deserialize(reader);

                    //Assert that the result object is of the expected type (some derivative of IFeed)
                    Assert.IsInstanceOf(feed.GetType(), resultantObject);

                    //Assert that the two objects are distinct instances
                    Assert.AreNotSame(feed, resultantObject);

                    //Cast the object back into an IFeed and perform some specific assertions
                    var resultantFeed = resultantObject as IFeed;
                    Assert.AreEqual(feed.Title, resultantFeed.Title);
                    Assert.AreEqual(feed.LastUpdated, resultantFeed.LastUpdated);
                    Assert.AreEqual(feed.FeedUri, resultantFeed.FeedUri);
                    Assert.AreEqual(feed.FeedType, resultantFeed.FeedType);
                    Assert.AreEqual(feed.Items.Count, resultantFeed.Items.Count);
                    Assert.Pass("THE FEED CAN BE DESERIALIZED SUCCESSFULLY");
                }
            }
        }

        #endregion

        #region Test helper methods

        /// <summary>
        /// Cleanup method that deletes all of the files after each test.
        /// </summary>
        [TearDown]
        public void Cleanup()
        {
            if (File.Exists(RssXmlFile))
            {
                File.Delete(RssXmlFile);
            }
            if (File.Exists(AtomXmlFile))
            {
                File.Delete(AtomXmlFile);
            }
        }

        #endregion
    }
}
