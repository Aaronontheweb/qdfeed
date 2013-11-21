using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace QDFeedParser.Tests.SyndicationFeed.Valid
{
    [TestFixture, Description("All basic tests for ensuring that high-level Atom feed parsing actually works.")]
    public class AtomFeedTest : BaseSyndicationFeedTest<Atom10Feed>
    {
        public AtomFeedTest()
            : base(new FileSystemFeedFactory(), TestFileLoader.LoadValidAtomTestCases(TestFileLoader.TestFileType.FileSys))
        {
        }
    }
}

