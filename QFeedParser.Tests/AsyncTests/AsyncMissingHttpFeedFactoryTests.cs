using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace QDFeedParser.Tests.AsyncTests
{
    public class AsyncMissingHttpFeedFactoryTests : BaseMultiTestCase
    {
        protected IFeedFactory Factory;
        protected FeedType FeedType;

        public AsyncMissingHttpFeedFactoryTests()
            : base(TestFileLoader.LoadMissingTestCases(TestFileLoader.TestFileType.Http))
        {
            Factory = new HttpFeedFactory();
            FeedType = FeedType.Rss20;
        }

        [Test, TestCaseSource("TestCases"), Description("Tests to see is a MissingFeedException is thrown when the Http feed factory attempts to DownloadXml from a non-existent file.")]
        [ExpectedException(typeof(MissingFeedException))]
        public void DoesHttpFeedFactoryThrowExceptionWhenDownloadXmlAsyncAcceptsMissingFile(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            var result = Factory.BeginDownloadXml(feeduri, null);
            var resultantTuple = Factory.EndDownloadXml(result);
        }

        [Test, TestCaseSource("TestCases"), Description("Tests to see is a MissingFeedException is thrown when the Http feed factory attempts to DownloadXml from a non-existent file.")]
        [ExpectedException(typeof(MissingFeedException))]
        public void DoesHttpFeedFactoryThrowExceptionWhenCreateFeedAsyncAcceptsMissingFile(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            var result = Factory.BeginCreateFeed(feeduri, null);
            var resultantFeed = Factory.EndCreateFeed(result);
        }

    }
}
