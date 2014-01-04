using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QDFeedParser.Tests.Extensions
{
    public static class QDObjectToString
    {
        public static string ToApprovalString(this IFeedItem feeditem)
        {
            var sb = new StringBuilder();

            sb.AppendLine(string.Format("Title: {0}", feeditem.Title));
            sb.AppendLine(string.Format("Published: {0}", feeditem.DatePublished));
            sb.AppendLine(string.Format("URL: {0}", feeditem.Link));
            sb.AppendLine(string.Format("Author: {0}", feeditem.Author));
            sb.AppendLine(string.Format("Id: {0}", feeditem.Id));
            sb.AppendLine(string.Format("Content: {0}", feeditem.Content));
            var categories = string.Empty;
            foreach (var category in feeditem.Categories)
            {
                categories += category + " ";
            }
            sb.AppendLine(string.Format("Categories: {0}", categories));

            if(feeditem.GetType() == typeof(Rss20FeedItem))
            {
                var temp = (Rss20FeedItem) feeditem;
                sb.AppendLine(string.Format("Comments: {0}",temp.Comments));
            }

            return sb.ToString();
        }
    }
}
