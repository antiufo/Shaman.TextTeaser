using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shaman.TextTeaser
{
    public class Program
    {
        public static void Main()
        {


            var id = "anythingyoulikehere";
            var title = "Astronomic news: the universe may not be expanding after all";
            var text = @"
Now that conventional thinking has been turned on its head in a paper by Prof Christof Wetterich at the University of Heidelberg in Germany. He points out that the tell-tale light emitted by atoms is also governed by the masses of their constituent particles, notably their electrons. The way these absorb and emit light would shift towards the blue part of the spectrum if atoms were to grow in mass, and to the red if they lost it. 

Because the frequency or “pitch” of light increases with mass, Prof Wetterich argues that masses could have been lower long ago. If they had been constantly increasing, the colours of old galaxies would look red-shifted – and the degree of red shift would depend on how far away they were from Earth. “None of my colleagues has so far found any fault [with this],” he says. 

Although his research has yet to be published in a peer-reviewed publication, Nature reports that the idea that the universe is not expanding at all – or even contracting – is being taken seriously by some experts, such as Dr HongSheng Zhao, a cosmologist at the University of St Andrews who has worked on an alternative theory of gravity. 

“I see no fault in [Prof Wetterich’s] mathematical treatment,” he says. “There were rudimentary versions of this idea two decades ago, and I think it is fascinating to explore this alternative representation of the cosmic expansion, where the evolution of the universe is like a piano keyboard played out from low to high pitch.” 

Prof Wetterich takes the detached, even playful, view that his work marks a change in perspective, with two different views of reality: either the distances between galaxies grow, as in the traditional balloon picture, or the size of atoms shrinks, increasing their mass. Or it’s a complex blend of the two. One benefit of this idea is that he is able to rid physics of the singularity at the start of time, a nasty infinity where the laws of physics break down. Instead, the Big Bang is smeared over the distant past: the first note of the ''cosmic piano’’ was long and low-pitched. 

Harry Cliff, a physicist working at CERN who is the Science Museum’s fellow of modern science, thinks it striking that a universe where particles are getting heavier could look identical to one where space/time is expanding. “Finding two different ways of thinking about the same problem often leads to new insights,” he says. “String theory, for instance, is full of 'dualities’ like this, which allow theorists to pick whichever view makes their calculations simpler.” 

If this idea turns out to be right – and that is a very big if – it could pave the way for new ways to think about our universe. If we are lucky, they might even be as revolutionary as Edwin Hubble’s, almost a century ago. 

Roger Highfield is director of external affairs at the Science Museum 

";
            
            var article = new Article(id, title, text);
            var redisConfig = new ConfigurationOptions() { DefaultDatabase = 2 };
            redisConfig.EndPoints.Add("localhost:6379");
            var multiplexer = ConnectionMultiplexer.Connect(redisConfig);
            IKeywordService ks = new RedisKeywordService(multiplexer.GetDatabase());
            var summarizer = new Summarizer("EN", ks);

            var summary = summarizer.Summarize(article.Content, article.Title, article.Id, article.Blog, article.Category);

            foreach (var item in summary.Results)
            {

                Console.WriteLine("-> " + item.Order + " " + item.Score + " " + item.Content);

            }

        }
    }
}
