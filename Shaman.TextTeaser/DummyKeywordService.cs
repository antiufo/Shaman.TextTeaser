using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shaman.TextTeaser
{
    public class DummyKeywordService : IKeywordService
    {
        public void Add(string word, long count, string summaryId, string blog, string cat)
        {
        }

        public long GetBlogCount(string blog)
        {
            return 1;
        }

        public long GetBlogScore(string word, string blog)
        {
            return 1;
        }

        public long GetCategoryCount(string cat)
        {
            return 1;
        }

        public long GetCategoryScore(string word, string cat)
        {
            return 1;
        }
    }
}
