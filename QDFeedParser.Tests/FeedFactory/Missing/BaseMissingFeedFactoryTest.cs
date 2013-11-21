using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace QDFeedParser.Tests.FeedFactory.Missing
{
    
    public abstract class BaseMissingFeedFactoryTest : BaseFeedFactoryTests<Rss20Feed>
    {
        protected BaseMissingFeedFactoryTest(IFeedFactory factory, FeedType feedtype, IEnumerable<TestCaseData> testcases)
            : base(factory, feedtype, testcases)
        {}

        [Test, TestCaseSource("TestCases"), Description("Tests whether or not the FeedFactory can successfully determine the feed's type.")]
        [ExpectedException(typeof(MissingFeedException))]
        public override void TestFactoryFeedTypeDetection(string rsslocation)
        {
            Uri feeduri = new Uri(rsslocation);
            FeedType testType = Factory.CheckFeedType(feeduri);
        }

        [Test, TestCaseSource("TestCases"), Description("Tests whether or not the FeedFactory fails to parse the feed which does not exist.")]
        [ExpectedException(typeof(MissingFeedException))]
        public override void TestFactoryFeedObjectSynthesis(string rsslocation)
        {
            Uri feeduri = new Uri(rsslocation);
            Factory.CreateFeed(feeduri);
        }

        [Test, TestCaseSource("TestCases"), Description("Tests that the FeedFactory properly fails to ping the missing URI.")]
        public override void TestFactoryFeedUriPing(string rsslocation)
        {
            Uri feeduri = new Uri(rsslocation);
            Assert.That(!Factory.PingFeed(feeduri), string.Format("Should not have been able to ping feed at location {0}", feeduri.OriginalString));
        }

        [Test, TestCaseSource("TestCases"), Description("Ensures that the FeedFactory object properly fails to load any XML from the missing file.")]
        [ExpectedException(typeof(MissingFeedException))]
        public override void TestFactoryFeedXmlDownload(string rsslocation)
        {
            Uri feeduri = new Uri(rsslocation);
            string feedxml = Factory.DownloadXml(feeduri);
        }
    }
}
