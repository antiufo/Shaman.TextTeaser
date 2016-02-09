using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shaman.TextTeaser
{
    internal struct TopKeyword
    {
        public TopKeyword(string word, double score)
        {
            this.Word = word;
            this.Score = score;
        }

        public string Word { get; }
        public double Score { get; }
    }
}
