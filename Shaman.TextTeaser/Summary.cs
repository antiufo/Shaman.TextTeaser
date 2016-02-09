using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shaman.TextTeaser
{
    public class Summary
    {
        internal Summary(List<Sentence> results)
        {
            this.results = results;
        }
        private List<Sentence> results;

        public IEnumerable<Sentence> Results => results;
        //public Summary TakeCharacters(int limitCharCount)
        //{
        //    var count = 0;
        //    var newSentences = results.TakeWhile(sentence =>
        //    {
        //        count += sentence.Content.Length;
        //        return limitCharCount >= count;
        //    });
        //    return new Summary(newSentences.ToList());
        //}
    }
}
