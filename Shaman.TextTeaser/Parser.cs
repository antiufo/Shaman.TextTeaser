using Shaman.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shaman.TextTeaser
{
    internal class Parser
    {
        public Parser(HashSet<string> stopWords)
        {
            this.stopWords = stopWords;
        }
        private HashSet<string> stopWords;
        internal List<string> SplitSentences(string str)
        {
            var valstr = str.AsValueString();

            var arr = new List<string>();
            var pos = 0;
            var searchStartPos = pos;
            while (pos < str.Length)
            {
                var idx = str.IndexOfAny(StopChars, searchStartPos);
                if (idx == -1)
                {
                    idx = str.Length;
                }


                var length = idx != -1 ? idx - pos : str.Length - pos;

                var z = valstr.Substring(pos, length);
                var next = valstr.Substring(pos + length);

                bool skip = false;
                int more = 0;
                var ns = next.Substring(1);
                if (ns.StartsWith("’’")) { more = 2; }
                else if (ns.StartsWith("”")) { more = 1; }
                else if (ns.StartsWith("\"")) { more = 1; }
                else if (ns.StartsWith(")")) { more = 1; }
                else if (ns.StartsWith("]")) { more = 1; }
                else if (ns.StartsWith("}")) { more = 1; }



                for (int i = 0; i < next.Length; i++)
                {
                    var ch = next[i];
                    if (ch == '\n')
                    {
                        break;
                    }
                    if (char.IsLetterOrDigit(ch))
                    {
                        if (char.IsLower(ch)) skip = true;
                        break;
                    }
                }

                if (skip)
                {
                    searchStartPos = idx + 1;
                    continue;
                }


                if (more != 0)
                {
                    if (ns.Length >= more)
                    {
                        if (!char.IsWhiteSpace(ns[more])) more = 0;
                    }
                }

                if (more != 0)
                {
                    z = valstr.Substring(pos, length + 1 + more);
                    idx += more;
                }


                var trimmed = z.Trim();
                if (trimmed.Length != 0)
                {
                    arr.Add(trimmed.ToClrString() + (idx == str.Length || more != 0 ? null : str[idx].ToString()));
                }

                pos = idx + 1;
                searchStartPos = pos;
            }
            return arr;
        }
        readonly static char[] StopChars = new[] { '.', '!', '?' };

        internal List<string> SplitWords(string title)
        {
            return Shaman.StringUtils.GetWords(title, false);
        }

        internal KeywordList GetKeywords(string text)
        {
            var keyWords = SplitWords(text);
            var sizeWithRepeatingWords = keyWords.Count;
            return new KeywordList(
              keyWords.Where(w => !stopWords.Contains(w))
              .GroupBy(w => w)
              .Select(w => new ArticleKeyword(w.Key, w.Count()))
              .OrderByDescending(w => w.Count)
              .ToList(),
              sizeWithRepeatingWords);
        }

        internal int SentenceLength(IEnumerable<string> sentence)
        {
            return sentence.Sum(x => x.Length);
        }

        internal double TitleScore(List<string> titleWords, IEnumerable<string> sentence)
        {
            return sentence.Count(w => !stopWords.Contains(w) && titleWords.Contains(w)) / (double)titleWords.Count;
        }

        internal double SentencePosition(int ctr, double sentenceCount)
        {
            var normalized = ctr / sentenceCount;

            if (normalized > 1.0)
                return 0d;
            else if (normalized > 0.9)
                return 0.15;
            else if (normalized > 0.8)
                return 0.04;
            else if (normalized > 0.7)
                return 0.04;
            else if (normalized > 0.6)
                return 0.06;
            else if (normalized > 0.5)
                return 0.04;
            else if (normalized > 0.4)
                return 0.05;
            else if (normalized > 0.3)
                return 0.08;
            else if (normalized > 0.2)
                return 0.14;
            else if (normalized > 0.1)
                return 0.23;
            else if (normalized > 0)
                return 0.17;
            return 0d;
        }
    }
}
