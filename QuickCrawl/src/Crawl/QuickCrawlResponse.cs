using System.Collections.Generic;
using Core;

namespace Crawl
{
    /// <summary>Contains the entire crawl settings and potential crawls ordered by score</summary>
    public class QuickCrawlResponse
    {
        public CrawlSettings Settings { get; set; }
        public SearchArea SearchArea { get; set; }
        public ICollection<YelpBusiness> AllBusinessInArea { get; set; }
        
        /// <summary>Some of the best crawls that were generated</summary>
        public ICollection<PubCrawlWithScore> Crawls { get; set; }
    }
}
