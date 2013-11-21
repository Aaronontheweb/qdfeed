using NUnit.Framework;

namespace QDFeedParser.Tests.FeedFactory.Valid.Http
{
    [TestFixture, Description("Runs all of the routine tests on the HttpFeedFactory object for Atom 1.0 feeds.")]
    public class HttpFeedFactoryTestsAtom : BaseFeedFactoryTests<Atom10Feed>
    {

        public HttpFeedFactoryTestsAtom()
            : base(new HttpFeedFactory(), QDFeedParser.FeedType.Atom10, TestFileLoader.LoadValidAtomTestCases(TestFileLoader.TestFileType.Http))
        {
        }
    }
}
