using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crawl
{
    public class CrawlGenerator
    {
        private readonly CrawlSettings _crawlSettings;

        public CrawlGenerator(CrawlSettings crawlSettings)
        {
            _crawlSettings = crawlSettings;
        }

        public Crawl Generate()
        {
            Crawl result = new Crawl()
            {
                Settings = _crawlSettings
            };

            SearchArea searchArea = RadiusCalculator.FindSearchArea(_crawlSettings.Start, _crawlSettings.End);



            return result;
        }
    }
}
