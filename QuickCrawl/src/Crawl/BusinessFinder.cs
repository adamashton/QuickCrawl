using System.Collections.Generic;
using System.Threading.Tasks;
using Core;
using Yelp;

namespace Crawl
{
    public class BusinessFinder
    {
        private const int BusinessLimit = 20;
        private readonly List<string> _categories = new List<string> { "bars" };
        private readonly ApiClient _apiClient;

        public BusinessFinder(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<ICollection<YelpBusiness>> Find(SearchArea searchArea)
        {
            SearchResponse response = await _apiClient.Search(searchArea.Centre, searchArea.Radius, _categories, 0, BusinessLimit);
            List<YelpBusiness> result = new List<YelpBusiness>();
            foreach (Business business in response.businesses)
            {
                YelpBusiness yelpBusiness = YelpBusinessConverter.Convert(business);
                result.Add(yelpBusiness);
            }
            return result;
        }
    }
}
