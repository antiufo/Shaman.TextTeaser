using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shaman.TextTeaser
{
    public class Summarizer
    {
        private Parser parser;
        private IKeywordService keywordService;
        public Summarizer(string language, IKeywordService keywordService)
        {
            this.parser = new Parser(StopWords.GetStopWords(language));
            this.keywordService = keywordService;
        }

        private int SummarySize { get; set; } = 5;
        private int KeywordsSize { get; set; } = 10;
        private int EnsureSizeDoesNotExceedLimit(int size, int limit)
        {
            return Math.Min(size, limit);
        }

        public Summary Summarize(Article article)
        {
            return Summarize(article.Content, article.Title, article.Url, article.Blog, article.Category);
        }

        public Summary Summarize(string text, string title, string link, string blog, string category)
        {
            var sentences = parser.SplitSentences(text);
            //foreach (var item in sentences)
            //{
            //    Console.WriteLine("-> " + item);
            //}
            var titleWords = parser.SplitWords(title);
            var resKeywords = parser.GetKeywords(text);
            var keywords = resKeywords.Keywords;
            KeywordsSize = EnsureSizeDoesNotExceedLimit(KeywordsSize, keywords.Count);
            var topKeywords = GetTopKeywords(keywords.Take(KeywordsSize), resKeywords.WordCount, link, blog, category);
            var result = ComputeScore(sentences.ToList(), titleWords, topKeywords.ToList());
            SummarySize = EnsureSizeDoesNotExceedLimit(SummarySize, result.Count);
            return new Summary(result);
        }

        private IEnumerable<TopKeyword> GetTopKeywords(IEnumerable<ArticleKeyword> keywords, int articleCount, string link, string blog, string category)
        {
            return keywords.Select(k =>
            {
                var blogCount = keywordService.GetBlogCount(blog) + 1.0;
                var categoryCount = keywordService.GetCategoryCount(category) + 1.0;

                keywordService.Add(k.Word, k.Count, link, blog, category);

                var articleScore = k.Count / articleCount;
                var blogScore = keywordService.GetBlogScore(k.Word, blog) / blogCount;
                var categoryScore = keywordService.GetCategoryScore(k.Word, category) / categoryCount;
                var totalScore = articleScore * 1.5 + blogScore + categoryScore;

                return new TopKeyword(k.Word, totalScore);
            });
        }


        private List<Sentence> ComputeScore(List<string> sentences, List<string> titleWords, List<TopKeyword> topKeywords)
        {
            return Enumerable.Range(0, sentences.Count).Select(i =>
            {
                var sentence = parser.SplitWords(sentences[i]);
                var titleFeature = parser.TitleScore(titleWords, sentence);
                var sentenceLength = parser.SentenceLength(sentence);
                var sentencePosition = parser.SentencePosition(i, sentences.Count);
                var sbsFeature = sbs(sentence, topKeywords);
                var dbsFeature = dbs(sentence, topKeywords);
                var keywordFrequency = (sbsFeature + dbsFeature) / 2.0 * 10.0;
                var totalScore = (titleFeature * 1.5 + keywordFrequency * 2.0 + sentenceLength * 0.5 + sentencePosition * 1.0) / 4.0;

                return new Sentence(sentences[i], totalScore, i);
            }).ToList();
        }


        private double sbs(List<string> words, List<TopKeyword> topKeywords)
        {
            if (words.Count == 0)
                return 0;
            else {
                var summ = words.Sum(word =>
                {
                    var m = topKeywords.FirstOrDefault(x => x.Word == word);
                    if (m.Word == null) return 0;
                    return m.Score;

                });
                return 1.0 / Math.Abs(words.Count) * summ;
            }
        }


        private double dbs(List<string> words, List<TopKeyword> topKeywords)
        {
            if (words.Count == 0)
                return 0;
            else {
                var res = words.Select((word, i) =>
                {
                    var m = topKeywords.FirstOrDefault(x => x.Word == word);
                    if (m.Word == null) return Tuple.Create(0d, i);
                    return Tuple.Create(m.Score, i);
                })
                .Where(x => x.Item1 > 0)
                .ToList();


                var summ = res.Zip(res.Skip(1), (a, b) =>
                    (a.Item1 * b.Item1) / Math.Pow(a.Item2 - b.Item2, 2)
                ).Sum();

                var k = words.Intersect(topKeywords.Select(x => x.Word)).Count() + 1;

                return (1.0 / (k * (k + 1.0))) * summ;
            }
        }
    }

}
