using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace QDFeedParser.Tests.FeedFactory.Missing.FileSys
{
    [TestFixture, Description("Tests the FileFeedFactory's behavior when its given a number of missing files.")]
    public class MissingFileSysFeedFactoryTests : BaseMissingFeedFactoryTest
    {
        public MissingFileSysFeedFactoryTests() : base(new FileSystemFeedFactory(), QDFeedParser.FeedType.Rss20, TestFileLoader.LoadMissingTestCases(TestFileLoader.TestFileType.FileSys))
        {
        }
    }
}
