using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shaman.TextTeaser
{
    internal class KeywordList
    {
        public List<ArticleKeyword> Keywords;
        public int WordCount;
        
        

        public KeywordList(List<ArticleKeyword> keywords, int wordCount)
        {
            this.Keywords = keywords;
            this.WordCount = wordCount;
        }
    }
}
