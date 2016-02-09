using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Shaman.TextTeaser
{
    public class RedisKeywordService : IKeywordService
    {
        private IDatabase db;
        public RedisKeywordService(IDatabase db)
        {
            this.db = db;
        }

        public void Add(string word, long count, string summaryId, string blog, string cat)
        {
            var tasks = new List<Task>();
            var sc = SynchronizationContext.Current;
            SynchronizationContext.SetSynchronizationContext(null);
            try
            {
                var b = db.CreateBatch();

                tasks.Add(b.HashIncrementAsync("bs:" + blog, word));
                tasks.Add(b.StringIncrementAsync("bc:" + blog));
                tasks.Add(b.StringIncrementAsync("cc:" + cat));
                tasks.Add(b.HashIncrementAsync("cs:" + cat, word));

                b.Execute();
                Task.WhenAll(tasks).GetAwaiter().GetResult();
            }
            finally
            {
                SynchronizationContext.SetSynchronizationContext(sc);
            }
       
        }

        public long GetBlogScore(string word, string blog)
        {
            var m = db.HashGet("bs:" + blog, word);
            long value = 0;
            m.TryParse(out value);
            return value;
        }

        public long GetBlogCount(string blog)
        {
            var m = db.StringGet("bc:" + blog);
            long value;
            m.TryParse(out value);
            return value;
        }

        public long GetCategoryCount(string cat)
        {
            var m = db.StringGet("cc:" + cat);
            long value;
            m.TryParse(out value);
            return value;
        }

        public long GetCategoryScore(string word, string cat)
        {
            var m = db.HashGet("cs:" + cat, word);
            long value = 0;
            m.TryParse(out value);
            return value;
        }
    }
}
