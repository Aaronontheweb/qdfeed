using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace QDFeedParser.Tests.SyndicationFeed.Valid
{
    [TestFixture]
    public class AtomKnownValuesTests : BaseKnownValueTest
    {
        public AtomKnownValuesTests() : base(new FileSystemFeedFactory(), KnownValueTestLoader.LoadAtomKnownValueTestCases())
        {
        }
    }
}
