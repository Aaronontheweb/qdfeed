using QDFeedParser.Examples.Web.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace QDFeedParser.Examples.Web.View
{
    public partial class FeedReader : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            HttpFeedFactory factory = new HttpFeedFactory(new Extended.Xml.PodcastFeedXmlParser(), new PodcastFeedInstanceProvider());
            Uri url = new Uri(string.Format("http://feed.scicast.com.br/")); //http://feed.nerdcast.com.br  //http://feed.scicast.com.br/
            //Uri url = new Uri(string.Format("http://{0}/Examples/{1}.xml", HttpContext.Current.Request.Url.Authority, "nerdcast")); 

            if (factory.PingFeed(url)) {
                PodcastRss20Feed feed = (PodcastRss20Feed) factory.CreateFeed(url);
                TitleLink.HRef = feed.Link;
                TitleLink.Title = feed.Description;
                TitleLink.InnerHtml = feed.Title;

                LogoImage.Src = feed.Image.Url;
                LogoImage.Alt = feed.Image.Title;

                DescriptionLiteral.Text = feed.Description;
                ManagingEditorLiteral.Text = feed.ManagingEditor;

                SiteLink.HRef = feed.Link;
                SiteLink.Title = feed.Description;
                SiteLink.InnerHtml = feed.Link;

                CategoryLiteral.Text = feed.Category;
                LastDateLiteral.Text = feed.LastUpdated.ToLongDateString();

                for (int i = 0; i < feed.Items.Count; i++)
                {
                    IFeedItem item = feed.Items[i];
                    string modelo = string.Format(@"<div class=""panel panel-default"">
                                                    <div class=""panel-heading"">
                                                      <h4 class=""panel-title"">
                                                        <a data-toggle=""collapse"" data-parent=""#EpisodesList"" href=""#{0}"">
                                                            {1}
                                                        </a>
                                                      </h4>
                                                    </div>
                                                    <div id=""{0}"" class=""panel-collapse collapse"">
                                                      <div class=""panel-body"">{2}</div>
                                                    </div>
                                                  </div>", "collapse" + i, item.Title, item.Content);

                    LiteralControl collapseControl = new LiteralControl(modelo);
                    EpisodesList.Controls.Add(collapseControl);
                }
            }
        }
    }
}