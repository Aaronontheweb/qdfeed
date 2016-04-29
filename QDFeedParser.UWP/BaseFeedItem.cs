using System;
using System.Collections.Generic;

using System.Text;

namespace QDFeedParser
{
    public abstract class BaseFeedItem : IFeedItem
    {
        #region Constructor

        protected BaseFeedItem() : this(new List<string>())
        {

        }

        protected BaseFeedItem(IList<string> categories)
        {
            this._categories = categories;
        }

        #endregion

        #region data members

        protected string _title;
        public string Title
        {
            get { return this._title; }
            set { this._title = value; }
        }

        protected string _author;
        public string Author
        {
            get { return this._author; }
            set { this._author = value; }
        }

        protected string _id;
        public string Id
        {
            get { return this._id; }
            set { this._id = value; }
        }

        protected string _link;
        public string Link
        {
            get { return this._link; }
            set { this._link = value; }
        }

        protected DateTime _date_published;
        public DateTime DatePublished
        {
            get { return this._date_published; }
            set { this._date_published = value; }
        }

        protected string _content;
        public string Content
        {
            get { return this._content; }
            set { this._content = value; }
        }

        protected IList<string> _categories;
        public IList<string> Categories
        {
            get { return this._categories; }
        }

        #endregion
    }
}
