using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace QDFeedParser.Tests.AsyncTests
{
    [TestFixture, Description("Tests to ensure that the appropriate exceptions are thrown when an Asynchronous FileSystemFactory tries to parse a missing feed.")]
    public class AsyncMissingFileSysFeedFactoryTest : BaseMultiTestCase
    {
        protected IFeedFactory Factory;
        protected FeedType FeedType;

        public AsyncMissingFileSysFeedFactoryTest()
            : base(TestFileLoader.LoadMissingTestCases(TestFileLoader.TestFileType.FileSys))
        {
            Factory = new FileSystemFeedFactory();
            FeedType = FeedType.Rss20;
        }

        [Test, TestCaseSource("TestCases"), Description("Tests to see is a MissingFeedException is thrown when the FileSys feed factory attempts to DownloadXml from a non-existent file.")]
        [ExpectedException(typeof(MissingFeedException))]
        public void DoesFileSystemFeedFactoryThrowExceptionWhenDownloadXmlAsyncAcceptsMissingFile(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            var result = Factory.BeginDownloadXml(feeduri, null);
            var resultantTuple = Factory.EndDownloadXml(result);
        }

        [Test, TestCaseSource("TestCases"), Description("Tests to see is a MissingFeedException is thrown when the FileSys feed factory attempts to DownloadXml from a non-existent file.")]
        [ExpectedException(typeof(MissingFeedException))]
        public void DoesFileSystemFeedFactoryThrowExceptionWhenCreateFeedAsyncAcceptsMissingFile(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            var result = Factory.BeginCreateFeed(feeduri, null);
            var resultantFeed = Factory.EndCreateFeed(result);
        }
    }
}
