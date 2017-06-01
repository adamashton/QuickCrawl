using System.Collections.Generic;
using Core;

namespace Crawl
{
    public class PubCrawl
    {
        public List<YelpBusiness> Businesses { get; set; }
    }


    /// <summary>A pub crawl with a score. The score is only relevant against other crawls in it's class.</summary>
    public class PubCrawlWithScore : PubCrawl
    {
        public double Score { get; set; }
    }
}
