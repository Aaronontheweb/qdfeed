using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using QDFeedParser.Tests.FeedFactory;

namespace QDFeedParser.Tests.AsyncTests
{
    [TestFixture, Description("Runs a battery tests to see if Asynchronous file IO operations work correctly.")]
    public class AsyncFileSysFeedFactoryTests : BaseMultiTestCase
    {
        protected IFeedFactory Factory;
        protected FeedType FeedType;

        public AsyncFileSysFeedFactoryTests()
            : base(TestFileLoader.LoadValidRssTestCases(TestFileLoader.TestFileType.FileSys))
        {
            Factory = new FileSystemFeedFactory();
            FeedType = FeedType.Rss20;
        }

        [Test, TestCaseSource("TestCases"), Description("Tests whether or not the FeedFactory can successfully initiate and complete asynchronous requests to download xml.")]
        public void CanDownloadXmlStreamAsync(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            var result = Factory.BeginDownloadXml(feeduri, null);
            var resultantTuple = Factory.EndDownloadXml(result);

            Assert.IsNotNull(resultantTuple, "The resultant FeedTuple from the Async operation should not be null!");
            Assert.That(resultantTuple.FeedContent != String.Empty, "The resultant feed should not be empty!");
            Assert.AreEqual(feeduri, resultantTuple.FeedUri, "The two uris should be equal!");
        }

        [Test, TestCaseSource("TestCases"), Description("Tests whether or not the FeedFactory can successfully initiate and complete asynchronous requests to create feeds.")]
        public void CanCreateFeedAsync(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            var result = Factory.BeginCreateFeed(feeduri, null);
            var resultantFeed = Factory.EndCreateFeed(result);

            Assert.IsNotNull(resultantFeed, "The resultant FeedTuple from the Async operation should not be null!");
            Assert.That(resultantFeed.Title != String.Empty, "The resultant feed should not be empty!");
            Assert.AreEqual(feeduri, resultantFeed.FeedUri, "The two uris should be equal!");
            Assert.IsTrue(resultantFeed.Items.Count() > 0);
        }
    }
}
