using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace QDFeedParser.Tests.FeedFactory.Missing.Http
{
    [TestFixture, Description("Tests the FileFeedFactory's behavior when its given a number of missing files.")]
    public class MissingHttpFeedFactoryTests : BaseMissingFeedFactoryTest
    {
        public MissingHttpFeedFactoryTests()
            : base(new HttpFeedFactory(), QDFeedParser.FeedType.Rss20, TestFileLoader.LoadMissingTestCases(TestFileLoader.TestFileType.Http))
        {
        }
    }
}
