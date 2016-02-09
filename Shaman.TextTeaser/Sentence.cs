using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shaman.TextTeaser
{
    public class Sentence
    {
        internal Sentence(string content, double score, int order)
        {
            this.Content = content;
            this.Score = score;
            this.Order = order;
        }

        public int Order { get; }
        public double Score { get; }
        public string Content { get; }

    }
}
