using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace QDFeedParser.Tests.SyndicationFeed.Valid
{
    [TestFixture, Description("The other RSS tests are just used to ensure that errors aren't thrown during parsing. These test cases are used to ensure that the correct values are extracted from the XML parse.")]
    public class RssKnownValuesTests : BaseKnownValueTest
    {
        public RssKnownValuesTests() : base(new FileSystemFeedFactory(), KnownValueTestLoader.LoadRssKnownValueTestCases())
        {
        }

        protected override void TestFeedParseCorrectness(IFeedKnownValueTest testcase, IFeed resultantItem)
        {
            Rss20Feed rssItem = (Rss20Feed) resultantItem;
            RssFeedKnownValueTest rssTest = (RssFeedKnownValueTest) testcase;
            base.TestFeedParseCorrectness(testcase, resultantItem);

            //Assert that the two descriptions are equivalent
            Assert.That(rssItem.Description == rssTest.Description);

            //Assert that the two languages are equivalent
            Assert.That(rssItem.Language == rssTest.Language);
        }
    }
}
