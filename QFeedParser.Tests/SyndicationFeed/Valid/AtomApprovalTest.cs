using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApprovalTests;
using ApprovalTests.Reporters;
using NUnit.Framework;
using QDFeedParser.Tests.Extensions;

namespace QDFeedParser.Tests.SyndicationFeed.Valid
{
    [TestFixture]
    public class AtomApprovalTest
    {
        protected IFeedFactory Factory;
        protected Uri feeduri;
        protected int itemCount;

        [SetUp]
        public void SetUp()
        {
            this.Factory = new FileSystemFeedFactory();
            feeduri = new Uri(TestFileLoader.LoadValidAtomTestCases(TestFileLoader.TestFileType.FileSys).Last().Arguments[0].ToString());
            itemCount = 3;
        }

        [UseReporter(typeof(DiffReporter))]
        [Test, Description("Uses approvals to determine if the output from the feed entry parsing is correct or not.")]
        public virtual void ApproveAtomFeedItemParse()
        {
            IFeed feed = Factory.CreateFeed(feeduri);
            var strFeedItems = from feeditem in feed.Items
                               select feeditem.ToApprovalString();
            Approvals.Approve(strFeedItems, "feed items");
        }

        [UseReporter(typeof(DiffReporter))]
        [Test, Description("Uses approvals to determine if the GetItems function produces the correct amount of feed entries.")]
        public virtual void ApproveAtomFeedItemExplicitCountParse()
        {
            var feed = Factory.CreateFeed(feeduri);
            var strFeedItems = from feeditem in feed.Items.Take(itemCount)
                               select feeditem.ToApprovalString();
            Approvals.Approve(strFeedItems, "feed items");
        }

    }
}
