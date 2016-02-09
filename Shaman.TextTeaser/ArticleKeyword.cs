using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shaman.TextTeaser
{
    internal struct ArticleKeyword
    {
        public ArticleKeyword(string word, int count)
        {
            this.Word = word;
            this.Count = count;
        }

        public string Word { get; }
        public int Count { get; }
    }
}
