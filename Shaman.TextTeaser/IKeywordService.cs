using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shaman.TextTeaser
{
    public interface IKeywordService
    {
        long GetBlogCount(string blog);
        long GetCategoryCount(string cat);
        long GetBlogScore(string word, string blog);
        long GetCategoryScore(string word, string cat);
        void Add(string word, long count, string summaryId, string blog, string cat);

    }
}
