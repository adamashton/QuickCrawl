using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core;

namespace Crawl
{
    public class QuickCrawlEngine
    {
        private const int NumberCrawlsReturn = 10;

        private readonly BusinessFinder _businessFinder;
        

        public QuickCrawlEngine(BusinessFinder businessFinder)
        {
            _businessFinder = businessFinder;
        }

        public async Task<QuickCrawlResponse> Generate(CrawlSettings crawlSettings)
        {
            QuickCrawlResponse result = new QuickCrawlResponse()
            {
                Settings = crawlSettings
            };

            // get search area
            SearchArea searchArea = RadiusCalculator.FindSearchArea(crawlSettings.Start, crawlSettings.End);
            result.SearchArea = searchArea;

            // find all available businesses
            ICollection<YelpBusiness> yelpBusinesses = await _businessFinder.Find(result.SearchArea);
            var filteredBusinesses = FilterBusinesses(crawlSettings, yelpBusinesses);
            result.AllBusinessInArea = filteredBusinesses;

            ICollection<PubCrawlWithScore> pubCrawlWithScores = GenerateAndScoreCrawls(crawlSettings, yelpBusinesses);

            // we only want the top x crawls to send back
            var pubCrawlsToReturn = pubCrawlWithScores
                .OrderByDescending(x => x.Score)
                .Take(NumberCrawlsReturn)
                .ToList();

            result.Crawls = pubCrawlsToReturn;

            return result;
        }

        private ICollection<YelpBusiness> FilterBusinesses(CrawlSettings crawlSettings, ICollection<YelpBusiness> yelpBusinesses)
        {
            //TODO filter by distance from straight line crawl - any too far away we need to disregard
            return yelpBusinesses;
        }

        private static ICollection<PubCrawlWithScore> GenerateAndScoreCrawls(CrawlSettings crawlSettings, ICollection<YelpBusiness> yelpBusinesses)
        {
            CrawlGenerator crawlGenerator = new CrawlGenerator(crawlSettings, yelpBusinesses);
            List<PubCrawl> pubCrawls = crawlGenerator.Generate();
            ICollection<PubCrawlWithScore> pubCrawlWithScores = PubCrawlScorer.Score(crawlSettings, pubCrawls);
            return pubCrawlWithScores;
        }
    }
}
