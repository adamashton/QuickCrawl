using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yelp;

namespace Api
{
    public class HealthCheck
    {
        public static async Task TestYelp(ApiClient apiClient)
        {
            // Test Yelp Access
            Coordinates location = new Coordinates(49.25943m, -123.06973m);
            SearchResponse response = await apiClient.Search(location, 1000, new List<string> { "bars" });
            if (response == null || response.businesses == null || !response.businesses.Any())
                throw new Exception("Null or empty response from Yelp business search");
        }
    }
}
