using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace QDFeedParser.Tests
{
    public static class KnownValueTestLoader
    {
        public static IList<TestCaseData> LoadAllKnownValueTestCases()
        {
            var atomCases = LoadRssKnownValueTestCases();
            var rssCases = LoadAtomKnownValueTestCases();
            foreach (var testcase in atomCases)
            {
                rssCases.Add(testcase);
            }
            return rssCases;
        }

        public static IList<TestCaseData> LoadRssKnownValueTestCases()
        {
            IList<TestCaseData> returnList = new List<TestCaseData>();
            var feedtype = FeedType.Rss20;
            var feedobjecttype = typeof(Rss20Feed);
            IFeedKnownValueTest rssTestAaronontheweb = new RssFeedKnownValueTest
                                                          {
                                                              FeedUri =
                                                                  new Uri(TestFileLoader.ValidFileSysRssTestDir +
                                                                          "Aaronontheweb-RSS.xml"),
                                                              FeedObjectType = feedobjecttype,
                                                              FeedType = feedtype,
                                                              Generator = "BlogEngine.NET 1.6.1.0",
                                                              LastUpdated = DateTime.Parse("Mon, 14 Jun 2010 12:26:00 -1200").ToUniversalTime(),
                                                              Title = "Aaronontheweb",
                                                              Link = "http://www.aaronstannard.com/",
                                                              Description = ".NET Development with Social Media APIs",
                                                              Language = "en-GB"
                                                          };

            IFeedKnownValueTest rssTestTechCrunch = new RssFeedKnownValueTest
                                                        {
                                                            FeedUri = new Uri(TestFileLoader.ValidFileSysRssTestDir + "TechCrunch-RSS.xml"),
                                                            FeedObjectType = feedobjecttype,
                                                            FeedType = feedtype,
                                                            Generator = "http://wordpress.com/",
                                                            LastUpdated = DateTime.Parse("Fri, 18 Jun 2010 01:21:15 +0000").ToUniversalTime(),
                                                            Title = "TechCrunch",
                                                            Link = "http://techcrunch.com",
                                                            Description = "TechCrunch is a group-edited blog that profiles the companies, products and events defining and transforming the new web.",
                                                            Language = "en"
                                                        };

            IFeedKnownValueTest rssTestHackerNews = new RssFeedKnownValueTest
                                                        {
                                                            FeedUri = new Uri(TestFileLoader.ValidFileSysRssTestDir + "HackerNews-RSS.xml"),
                                                            FeedObjectType = feedobjecttype,
                                                            FeedType = feedtype,
                                                            Generator = string.Empty,
                                                            LastUpdated = DateTime.UtcNow,
                                                            Title = "Hacker News",
                                                            Link = "http://news.ycombinator.com/",
                                                            Description = "Links for the intellectually curious, ranked by readers.",
                                                            Language = string.Empty
                                                        };

            IFeedKnownValueTest rssTestDelicious = new RssFeedKnownValueTest
            {
                FeedUri = new Uri(TestFileLoader.ValidFileSysRssTestDir + "del.icio.us-RSS.xml"),
                FeedObjectType = feedobjecttype,
                FeedType = feedtype,
                Generator = string.Empty,
                LastUpdated = DateTime.Parse("Wed, 16 Jun 2010 17:45:37 +0000").ToUniversalTime(),
                Title = "Delicious/Aaronontheweb",
                Link = "http://delicious.com/Aaronontheweb",
                Description = "bookmarks posted by Aaronontheweb",
                Language = string.Empty
            };

            returnList.Add(new TestCaseData(rssTestAaronontheweb).SetName("Aaronontheweb-RSS.xml"));
            returnList.Add(new TestCaseData(rssTestTechCrunch).SetName("TechCrunch-RSS.xml"));
            returnList.Add(new TestCaseData(rssTestHackerNews).SetName("HackerNews-RSS.xml"));
            returnList.Add(new TestCaseData(rssTestDelicious).SetName("del.icio.us-RSS.xml"));

            return returnList;
        }

        public static IList<TestCaseData> LoadAtomKnownValueTestCases()
        {
            IList<TestCaseData> returnList = new List<TestCaseData>();
            QDFeedParser.FeedType feedtype = FeedType.Atom10;
            Type feedobjecttype = typeof(Atom10Feed);

            IFeedKnownValueTest atomTestTed = new AtomFeedKnownValueTest
                                                  {
                                                      FeedUri = new Uri(TestFileLoader.ValidFileSysAtomTestDir + "Ted-Atom.xml"),
                                                      FeedObjectType = feedobjecttype,
                                                      FeedType = feedtype,
                                                      Generator = string.Empty,
                                                      LastUpdated = DateTime.Parse("2010-06-13T10:50:57-07:00").ToUniversalTime(),
                                                      Title = "Ted Dziuba",
                                                      Link = "http://teddziuba.com/",
                                                  };

            IFeedKnownValueTest atomTestGoogleNews = new AtomFeedKnownValueTest
            {
                FeedUri = new Uri(TestFileLoader.ValidFileSysAtomTestDir + "GoogleNews-Atom.xml"),
                FeedObjectType = feedobjecttype,
                FeedType = feedtype,
                Generator = "NFE/1.0",
                LastUpdated = DateTime.Parse("2010-06-18T03:12:56Z").ToUniversalTime(),
                Title = "Top Stories - Google News",
                Link = "http://news.google.com?pz=1&ned=us&hl=en",
            };

            IFeedKnownValueTest atomTestYouTube = new AtomFeedKnownValueTest
            {
                FeedUri = new Uri(TestFileLoader.ValidFileSysAtomTestDir + "YouTube-Atom.xml"),
                FeedObjectType = feedobjecttype,
                FeedType = feedtype,
                Generator = "YouTube data API",
                LastUpdated = DateTime.Parse("2010-05-31T22:21:27.381Z").ToUniversalTime(),
                Title = "Uploads by smartdraw",
                Link = "http://gdata.youtube.com/feeds/api/users/smartdraw"
            };

            returnList.Add(new TestCaseData(atomTestTed).SetName("Ted-Atom.xml"));
            returnList.Add(new TestCaseData(atomTestGoogleNews).SetName("GoogleNews-Atom.xml"));
            returnList.Add(new TestCaseData(atomTestYouTube).SetName("YouTube-Atom.xml"));
            return returnList;
        }
    }
}
