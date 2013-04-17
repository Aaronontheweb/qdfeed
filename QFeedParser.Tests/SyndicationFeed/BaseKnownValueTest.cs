using System;
using System.Collections.Generic;
using System.Linq;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using QDFeedParser.Tests.Extensions;

namespace QDFeedParser.Tests.SyndicationFeed
{
    public abstract class BaseKnownValueTest : BaseMultiTestCase
    {
        protected IFeedFactory Factory;

        public BaseKnownValueTest(IFeedFactory factory, IEnumerable<TestCaseData> testcases) : base(testcases)
        {
            this.Factory = factory;
        }

        [Test, TestCaseSource("TestCases")]
        public virtual void TestFeedParseCorrectness(IFeedKnownValueTest testcase)
        {
            IFeed resultantItem = this.Factory.CreateFeed(testcase.FeedUri);
            this.TestFeedParseCorrectness(testcase, resultantItem);
        }

        protected virtual void TestFeedParseCorrectness(IFeedKnownValueTest testcase, IFeed resultantItem)
        {
            //Assert that the resultant item and the test case both originate from the same URI
            Assert.That(resultantItem.FeedUri.Equals(testcase.FeedUri.OriginalString));

            //Assert that the resultant item matches the expected output type of the test case
            Assert.That(resultantItem.GetType() == testcase.FeedObjectType);

            //Assert that the feedtypes of the two objects match
            Assert.That(resultantItem.FeedType == testcase.FeedType);

            //Feeds with no updated date/time are assigned DateTime.UtcNow - the expected value and actual for this test case
            //will be off by a minute or so, and shouldn't fail.
            if(!(testcase.LastUpdated - new TimeSpan(0,0,5) < resultantItem.LastUpdated))
            {
                //Assert that the parsed date matches the expected date
                Assert.AreEqual(testcase.LastUpdated, resultantItem.LastUpdated);
            }

            //Assert that the parsed title matches the expected title
            Assert.AreEqual(testcase.Title, resultantItem.Title);

            //Assert that the generator parsed from the feed matches what's supplied in the test case
            Assert.AreEqual(testcase.Generator, resultantItem.Generator);

            //Assert that the link parsed from the feed matches what's supplied in the test case
            Assert.AreEqual(testcase.Link, resultantItem.Link);
        }

    }
}
