using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NUnit.Framework;

namespace QDFeedParser.Tests.SyndicationFeed
{
    public abstract class BaseSyndicationFeedTest<T> : BaseMultiTestCase where T : IFeed
    {
        protected IFeedFactory Factory;

        public BaseSyndicationFeedTest(IFeedFactory factory, IEnumerable<TestCaseData> testcases)
            : base(testcases)
        {
            this.Factory = factory;
        }

        [Test, TestCaseSource("TestCases"), Description("Tests whether or not the FeedFactory can successfully determine the feed's type.")]
        public void CanDetermineFeedType(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            var feed = Factory.CreateFeed(feeduri);

            //Assert that the feed is of the correct type
            Assert.That(feed, Is.TypeOf(typeof (T)));
        }

        [Test, TestCaseSource("TestCases"), Description("Tests whether or not the FeedFactory can successfully determine the feed's type.")]
        public void TestCommonFeedProperties(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            var feed = Factory.CreateFeed(feeduri);

            //Assert that the feed is of the correct type
            Assert.That(feed, Is.TypeOf(typeof(T)));

            //Assert that the URI for both feeds are equal
            Assert.That(feed.FeedUri.Equals(feeduri.OriginalString));

            //Assert that the title for this feed is not null
            Assert.That(feed.Title, Is.Not.Null);

            //Assert that the last updated date for this feed is not null (or in this case, has a date value of 1/1/1955 or higher since
            //DateTime objects are never actually null.
            Assert.That(feed.LastUpdated.Date > new DateTime(1955,1,1));
        }

        [Test, TestCaseSource("TestCases"), Description("Tests whether or not the IFeed object can successfully iterate through its collection of entries.")]
        public void TestFeedEntryParsing(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            var feed = Factory.CreateFeed(feeduri);

            //Assert that the feed is of the correct type
            Assert.That(feed, Is.TypeOf(typeof(T)));

            var count = feed.Items.Count();
            Assert.That(count > 0, "There should have been more than one item in this feed.");

            var itemsViaExplicitCount = feed.Items.Take(count);
            Assert.That(itemsViaExplicitCount.Count() == count);
        }

        [Test, TestCaseSource("TestCases"), Description("Tests whether or not the IFeed object can iterate through its collection of entries more than once.")]
        public void TestCanGoThroughFeedMultipleTimes(string rsslocation)
        {
            var feeduri = new Uri(rsslocation);
            var feed = Factory.CreateFeed(feeduri);

            //Assert that the feed is of the correct type
            Assert.That(feed, Is.TypeOf(typeof(T)));

            var count = feed.Items.Count();
            Assert.That(count > 0, "There should have been more than one item in this feed.");

            var count2 = 0;
            foreach(var item in feed.Items)
            {
                Assert.IsNotNullOrEmpty(item.Title);
                Assert.IsNotNull(item.Content);
                Assert.IsNotNullOrEmpty(item.Link);
                count2++;
            }

            Assert.AreEqual(count, count2, "The counts should have been equal.");

        }
    }
}
