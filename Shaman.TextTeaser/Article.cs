using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shaman.TextTeaser
{
    public class Article
    {
        public string Id { get; }
        public string Title { get; }
        public string Content { get; }
        public string Url { get; }
        public string Blog { get; }
        public string Category { get; }
        public Article(string id, string title, string content, string url = null, string blog = null, string category = null)
        {
            this.Id = id;
            this.Title = title;
            this.Content = content;
            this.Url = url;
            this.Blog = blog;
            this.Category = category;
        }
    }
}
