using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.XPath;
using System.Text;

namespace QDFeedParser.Xml
{
    public class XPathFeedXmlParser : FeedXmlParserBase
    {
        #region IFeedXmlParser Members

        public override void ParseFeed(IFeed feed, string xml)
        {
            switch (feed.FeedType)
            {
                case FeedType.Rss20:
                    var rssFeed = feed as Rss20Feed;
                    ParseRss20Header(rssFeed, xml);
                    ParseRss20Items(rssFeed, xml);
                    break;
                case FeedType.Atom10:
                    var atomFeed = feed as Atom10Feed;
                    ParseAtom10Header(atomFeed, xml);
                    ParseAtom10Items(atomFeed, xml);
                    break;
            }
        }
        
        public override FeedType CheckFeedType(string feedxml)
        {
            var doc = new XmlDocument();
            doc.LoadXml(feedxml);
            var xmlRootElement = doc.DocumentElement;
            if (xmlRootElement.Name.Contains(RssRootElementName) && xmlRootElement.GetAttribute(RssVersionAttributeName) == "2.0")
                return FeedType.Rss20;
            else if (xmlRootElement.Name.Contains(AtomRootElementName))
                return FeedType.Atom10;
            else
                throw new InvalidFeedXmlException("Unable to determine feedtype (but was able to parse file) for feed");
        }

        #endregion

        #region Atom 1.0 parsing methods

        private XmlNamespaceManager NsManager;

        private void ParseAtom10Header(Atom10Feed atomFeed, string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            //Initialize our namespace manager.
            NsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            NsManager.AddNamespace("atom", "http://www.w3.org/2005/Atom");

            var titleNode = xmlDoc.SelectSingleNode("/atom:feed/atom:title", NsManager);
            atomFeed.Title = titleNode.InnerText;

            var linkNode = xmlDoc.SelectSingleNode("/atom:feed/atom:link[not(@rel)]/@href", NsManager) ??
                           xmlDoc.SelectSingleNode("/atom:feed/atom:author/atom:uri", NsManager) ??
                           xmlDoc.SelectSingleNode("/atom:feed/atom:link[@rel='alternate']/@href", NsManager);

            atomFeed.Link = linkNode == null ? string.Empty : linkNode.InnerText;

            var dateTimeNode = xmlDoc.SelectSingleNode("/atom:feed/atom:updated", NsManager);

            DateTime timeOut;
            DateTime.TryParse(dateTimeNode.InnerText, out timeOut);
            atomFeed.LastUpdated = timeOut.ToUniversalTime();

            var generatorNode = xmlDoc.SelectSingleNode("/atom:feed/atom:generator", NsManager);
            atomFeed.Generator = generatorNode == null ? string.Empty : generatorNode.InnerText;
        }

        private void ParseAtom10Items(IFeed feed, string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var feedItemNodes = xmlDoc.SelectNodes("/atom:feed/atom:entry", NsManager);
            foreach(XmlNode node in feedItemNodes)
            {
                feed.Items.Add(ParseAtom10SingleItem(node));
            }
        }

        private BaseFeedItem ParseAtom10SingleItem(XmlNode itemNode)
        {
            var titleNode = itemNode.SelectSingleNode("atom:title", NsManager);
            var datePublishedNode = itemNode.SelectSingleNode("atom:updated", NsManager);
            var authorNode = itemNode.SelectSingleNode("atom:author/name", NsManager);
            var idNode = itemNode.SelectSingleNode("atom:id", NsManager);
            var contentNode = itemNode.SelectSingleNode("atom:content", NsManager);
            var linkNode = itemNode.SelectSingleNode("atom:link/@href", NsManager);

            BaseFeedItem item = new Atom10FeedItem
            {
                Title = titleNode == null ? string.Empty : titleNode.InnerText,
                DatePublished = datePublishedNode == null ? DateTime.UtcNow : SafeGetDate(datePublishedNode.InnerText),
                Author = authorNode == null ? string.Empty : authorNode.InnerText,
                Id = idNode == null ? string.Empty : idNode.InnerText,
                Content = contentNode == null ? string.Empty : contentNode.InnerText,
                Link = linkNode == null ? string.Empty : linkNode.InnerText
            };

            var categoryNodes = itemNode.SelectNodes("atom:category/atom:term", NsManager);
            if (categoryNodes != null)
            {
                foreach (XmlNode categoryNode in categoryNodes)
                {
                    item.Categories.Add(categoryNode.InnerText);
                }
            }

            return item;
        }

        #endregion

        #region RSS 2.0 parsing methods

        private void ParseRss20Header(Rss20Feed rssFeed, string xml)
        {

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var titleNode = xmlDoc.SelectSingleNode("/rss/channel/title");
            rssFeed.Title = titleNode.InnerText;

            var descriptionNode = xmlDoc.SelectSingleNode("/rss/channel/description");
            rssFeed.Description = descriptionNode == null ? string.Empty : descriptionNode.InnerText;

            var linkNode = xmlDoc.SelectSingleNode("/rss/channel/link");
            rssFeed.Link = linkNode == null ? string.Empty : linkNode.InnerText;

            var dateTimeNode = xmlDoc.SelectSingleNode("//pubDate[1]");
            if (dateTimeNode == null) //We have to have a date, so we'll use the date/time when we polled the RSS feed as the default.
            {
                rssFeed.LastUpdated = DateTime.UtcNow;
            }
            else
            {
                DateTime timeOut;
                DateTime.TryParse(dateTimeNode.InnerText, out timeOut);
                rssFeed.LastUpdated = timeOut.ToUniversalTime();
            }

            var generatorNode = xmlDoc.SelectSingleNode("/rss/channel/generator");
            rssFeed.Generator = generatorNode == null ? string.Empty : generatorNode.InnerText;

            var languageNode = xmlDoc.SelectSingleNode("/rss/channel/language");
            rssFeed.Language = languageNode == null ? string.Empty : languageNode.InnerText;
        }

        private void ParseRss20Items(IFeed feed, string xml)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);
            var feedItemNodes = xmlDoc.SelectNodes("/rss/channel/item");
            foreach (XmlNode item in feedItemNodes)
            {
                feed.Items.Add(ParseRss20SingleItem(item));
            }
        }

        private BaseFeedItem ParseRss20SingleItem(XmlNode itemNode)
        {
            var titleNode = itemNode.SelectSingleNode("title");
            var datePublishedNode = itemNode.SelectSingleNode("pubDate");
            var authorNode = itemNode.SelectSingleNode("author");
            var commentsNode = itemNode.SelectSingleNode("comments");
            var idNode = itemNode.SelectSingleNode("guid");
            var contentNode = itemNode.SelectSingleNode("description");
            var linkNode = itemNode.SelectSingleNode("link");

            BaseFeedItem item = new Rss20FeedItem
            {
                Title = titleNode == null ? string.Empty : titleNode.InnerText,
                DatePublished = datePublishedNode == null ? DateTime.UtcNow : SafeGetDate(datePublishedNode.InnerText),
                Author = authorNode == null ? string.Empty : authorNode.InnerText,
                Comments = commentsNode == null ? string.Empty : commentsNode.InnerText,
                Id = idNode == null ? string.Empty : idNode.InnerText,
                Content = contentNode == null ? string.Empty : contentNode.InnerText,
                Link = linkNode == null ? string.Empty : linkNode.InnerText
            };

            var categoryNodes = itemNode.SelectNodes("category");
            if (categoryNodes != null)
            {
                foreach (XmlNode categoryNode in categoryNodes)
                {
                    item.Categories.Add(categoryNode.InnerText);
                }
            }

            return item;
        }

        #endregion
    }
}
