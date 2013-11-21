using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace QDFeedParser.Tests.FeedFactory
{
    public abstract class BaseFeedFactoryTests<T> : BaseMultiTestCase where T : IFeed
    {
        protected IFeedFactory Factory;
        protected FeedType FeedType;

        protected BaseFeedFactoryTests(IFeedFactory factory, FeedType feedtype, IEnumerable<TestCaseData> testcases) : base(testcases)
        {
            this.Factory = factory;
            this.FeedType = feedtype;
        }

        [Test, TestCaseSource("TestCases"), Description("Tests whether or not the FeedFactory can successfully determine the feed's type.")]
        public virtual void TestFactoryFeedTypeDetection(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            var testType = Factory.CheckFeedType(feeduri);
            Assert.That(testType == this.FeedType, string.Format("Parsed type was not of expected type for feed {0}.", feeduri.OriginalString));
        }

        [Test, TestCaseSource("TestCases"), Description("Tests whether or not the FeedFactory can successfully parse the feed into an object of the correct type.")]
        public virtual void TestFactoryFeedObjectSynthesis(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            Assert.That(Factory.CreateFeed(feeduri), Is.TypeOf(typeof(T)), string.Format("Expected type for this feed is {0}", typeof(T)));
            Assert.That(Factory.CreateFeed(feeduri, FeedType), Is.TypeOf(typeof (T)),
                        string.Format("Expected type for this feed is {0}", typeof (T)));
        }

        [Test, TestCaseSource("TestCases"), Description("Tests whether or not the FeedFactory can successfully ping the feed's URI.")]
        public virtual void TestFactoryFeedUriPing(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            Assert.That(Factory.PingFeed(feeduri), string.Format("Failed to ping feed at location {0}", feeduri.OriginalString));
        }

        [Test, TestCaseSource("TestCases"), Description("Tests whether or not the FeedFactory can successfully download the feed's XML.")]
        public virtual void TestFactoryFeedXmlDownload(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            var feedxml = Factory.DownloadXml(feeduri);
            Assert.That(feedxml.Length > 0, string.Format("Failed to download XML from feed at location {0}", feeduri.OriginalString));
        }


    }
}
