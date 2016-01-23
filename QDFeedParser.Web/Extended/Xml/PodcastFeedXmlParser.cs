﻿using QDFeedParser.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace QDFeedParser.Examples.Web.Extended.Xml
{
    public class PodcastFeedXmlParser : FeedXmlParserBase
    {
        #region Overrides of FeedXmlParserBase

        public override void ParseFeed(IFeed feed, string xml)
        {
            switch (feed.FeedType)
            {
                case FeedType.Rss20:
                    var rssFeed = feed as PodcastRss20Feed;
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
            var doc = XDocument.Parse(feedxml);
            var xmlRootElement = doc.Root;
            if (xmlRootElement.Name.LocalName.Contains(RssRootElementName) && xmlRootElement.Attribute(RssVersionAttributeName).Value == "2.0")
            {
                return FeedType.Rss20;
            }

            if (xmlRootElement.Name.LocalName.Contains(AtomRootElementName))
                return FeedType.Atom10;
            else
                throw new InvalidFeedXmlException("Unable to determine feedtype (but was able to parse file) for feed");
        }

        #region Atom 1.0 Parsing Methods

        protected readonly XNamespace Atom10Namespace = "http://www.w3.org/2005/Atom";

        protected virtual void ParseAtom10Header(Atom10Feed atomFeed, string xml)
        {
            var document = XDocument.Parse(xml);
            var channel = document.Root;

            var titleNode = channel.Element(Atom10Namespace + "title");
            atomFeed.Title = titleNode.Value;

            var linkNode = channel.Element(Atom10Namespace + "author") != null ? channel.Element(Atom10Namespace + "author").Element(Atom10Namespace + "uri") : null;

            if (linkNode == null)
            {
                linkNode = channel.Elements(Atom10Namespace + "link").SingleOrDefault(x => x.HasAttributes && x.Attribute("rel") == null) ??
                           channel.Elements(Atom10Namespace + "link").SingleOrDefault(x => x.HasAttributes && x.Attribute("rel") != null && x.Attribute("rel").Value == "alternate");

                atomFeed.Link = linkNode == null ? string.Empty : linkNode.Attribute("href").Value;
            }
            else
            {
                atomFeed.Link = linkNode == null ? string.Empty : linkNode.Value;
            }

            var dateTimeNode = channel.Element(Atom10Namespace + "updated");

            if (dateTimeNode != null)
            {
                DateTime timeOut;
                DateTime.TryParse(dateTimeNode.Value, out timeOut);
                atomFeed.LastUpdated = timeOut.ToUniversalTime();
            }

            var generatorNode = channel.Element(Atom10Namespace + "generator");
            atomFeed.Generator = generatorNode == null ? string.Empty : generatorNode.Value;

        }

        private void ParseAtom10Items(Atom10Feed atomFeed, string xml)
        {
            var document = XDocument.Parse(xml);
            var feedItemNodes = document.Root.Elements(Atom10Namespace + "entry");
            foreach (var item in feedItemNodes)
            {
                atomFeed.Items.Add(ParseAtom10SingleItem(item));
            }
        }

        protected virtual Atom10FeedItem CreateAtom10FeedItem()
        {
            return new Atom10FeedItem();
        }

        protected virtual BaseFeedItem ParseAtom10SingleItem(XElement itemNode)
        {
            var titleNode = itemNode.Element(Atom10Namespace + "title");
            var datePublishedNode = itemNode.Element(Atom10Namespace + "updated");
            var authorNode = itemNode.Element(Atom10Namespace + "author") == null ? null : itemNode.Element(Atom10Namespace + "author").Element(Atom10Namespace + "name");
            var idNode = itemNode.Element(Atom10Namespace + "id");
            var contentNode = itemNode.Element(Atom10Namespace + "content");
            var linkNode = itemNode.Element(Atom10Namespace + "link") == null ? null : itemNode.Element(Atom10Namespace + "link").Attribute("href");

            //create the new item
            Atom10FeedItem item = CreateAtom10FeedItem();

            item.Title = titleNode == null ? string.Empty : titleNode.Value;
            item.DatePublished = datePublishedNode == null ? DateTime.UtcNow : SafeGetDate(datePublishedNode.Value);
            item.Author = authorNode == null ? string.Empty : authorNode.Value;
            item.Id = idNode == null ? string.Empty : idNode.Value;
            item.Content = contentNode == null ? string.Empty : contentNode.Value;
            item.Link = linkNode == null ? string.Empty : linkNode.Value;

            var categoryNode = itemNode.Element(Atom10Namespace + "category");

            if (categoryNode != null)
            {
                var categoryNodes = categoryNode.Elements(Atom10Namespace + "term");
                foreach (var termNode in categoryNodes)
                {
                    item.Categories.Add(termNode.Value);
                }
            }

            return item;
        }

        #endregion

        #region RSS 2.0 Parsing Methods

        protected virtual void ParseRss20Header(PodcastRss20Feed rssFeed, string xml)
        {
            var document = XDocument.Parse(xml);
            var channel = document.Root.Element("channel");

            rssFeed.Title = channel.Element("title").Value;
            rssFeed.Description = channel.Element("description").Value;

            var linkNode = channel.Element("link");
            rssFeed.Link = linkNode == null ? string.Empty : linkNode.Value;

            var dateTimeNode = (from dateSelector in channel.Elements("lastBuildDate")
                                select dateSelector).FirstOrDefault();
            if (dateTimeNode == null)
            {
                rssFeed.LastUpdated = DateTime.UtcNow;
            }
            else
            {
                DateTime timeOut;
                DateTime.TryParse(dateTimeNode.Value, out timeOut);
                rssFeed.LastUpdated = timeOut.ToUniversalTime();
            }

            var generatorNode = channel.Element("generator");
            rssFeed.Generator = generatorNode == null ? string.Empty : generatorNode.Value;

            var languageNode = channel.Element("language");
            rssFeed.Language = languageNode == null ? string.Empty : languageNode.Value;



            var categoryNode = channel.Element("category");
            rssFeed.Category = categoryNode == null ? string.Empty : categoryNode.Value;

            var copyrightNode = channel.Element("copyright");
            rssFeed.Copyright = copyrightNode == null ? string.Empty : copyrightNode.Value;

            var managingEditorNode = channel.Element("managingEditor");
            rssFeed.ManagingEditor = managingEditorNode == null ? string.Empty : managingEditorNode.Value;

            var webMasterNode = channel.Element("webMaster");
            rssFeed.WebMaster = webMasterNode == null ? string.Empty : webMasterNode.Value;

            var imageNode = channel.Element("image");
            if (imageNode != null) {
                var urlNode = imageNode.Element("url");
                var titleNode = imageNode.Element("title");
                var link2Node = imageNode.Element("link");
                var widthNode = imageNode.Element("width");
                var heightNode = imageNode.Element("height");

                rssFeed.Image = new PodcastRss20FeedImage();

                rssFeed.Image.Url = urlNode == null ? string.Empty : urlNode.Value;
                rssFeed.Image.Title = titleNode == null ? string.Empty : titleNode.Value;
                rssFeed.Image.Link = link2Node == null ? string.Empty : link2Node.Value;
                rssFeed.Image.Width = widthNode == null ? string.Empty : widthNode.Value;
                rssFeed.Image.Height = heightNode == null ? string.Empty : heightNode.Value;
            }
        }

        private void ParseRss20Items(PodcastRss20Feed rssFeed, string xml)
        {
            var document = XDocument.Parse(xml);
            var feedItemNodes = document.Root.Element("channel").Elements("item");
            foreach (var item in feedItemNodes)
            {
                rssFeed.Items.Add(ParseRss20SingleItem(item));
            }
        }

        protected virtual PodcastRss20FeedItem CreatePodcastRss20FeedItem()
        {
            return new PodcastRss20FeedItem();
        }

        protected virtual BaseFeedItem ParseRss20SingleItem(XElement itemNode)
        {
            var titleNode = itemNode.Element("title");
            var datePublishedNode = itemNode.Element("pubDate");
            var authorNode = itemNode.Element("author");
            var commentsNode = itemNode.Element("comments");
            var idNode = itemNode.Element("guid");
            var contentNode = itemNode.Element("description");
            var linkNode = itemNode.Element("link");

            var dcCreatorNode = itemNode.Element(itemNode.GetNamespaceOfPrefix("dc") + "creator");  // itemNode.Element("dc:creator");
            var contentEncodedNode = itemNode.Element(itemNode.GetNamespaceOfPrefix("content") + "encoded"); //itemNode.Element("content:encoded");
            var enclosureNode = itemNode.Element("enclosure");

            PodcastRss20FeedItem item = CreatePodcastRss20FeedItem();

            item.Title = titleNode == null ? string.Empty : titleNode.Value;
            item.DatePublished = datePublishedNode == null ? DateTime.UtcNow : SafeGetDate(datePublishedNode.Value);
            item.Author = authorNode == null ? string.Empty : authorNode.Value;
            item.Comments = commentsNode == null ? string.Empty : commentsNode.Value;
            item.Id = idNode == null ? string.Empty : idNode.Value;
            item.Content = contentNode == null ? string.Empty : contentNode.Value;
            item.Link = linkNode == null ? string.Empty : linkNode.Value;

            item.DcCreator = dcCreatorNode == null ? string.Empty : dcCreatorNode.Value;
            //item.ContentEncoded = contentEncodedNode == null ? string.Empty : contentEncodedNode.Value;
            item.Enclosure = enclosureNode == null ? string.Empty : enclosureNode.Value;

            var categoryNodes = itemNode.Elements("category");
            foreach (var categoryNode in categoryNodes)
            {
                item.Categories.Add(categoryNode.Value);
            }

            return item;
        }

        #endregion

        #endregion
    }
}
