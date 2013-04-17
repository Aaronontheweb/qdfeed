using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using ApprovalTests;

namespace QDFeedParser.Tests.SyndicationFeed.Valid
{
    [TestFixture, Description("All basic tests for ensuring that high-level RSS feed parsing actually works.")]
    public class RssFeedTest : BaseSyndicationFeedTest<Rss20Feed>
    {
        public RssFeedTest() : base(new FileSystemFeedFactory(), TestFileLoader.LoadValidRssTestCases(TestFileLoader.TestFileType.FileSys))
        {
        }
    }
}
