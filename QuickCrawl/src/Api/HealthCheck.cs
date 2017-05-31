using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Yelp;
using Core;

namespace Api
{
    public class HealthCheck
    {
        public static async Task TestYelp(ApiClient apiClient)
        {
            // Test Yelp Access

            Location location = new Location(49.25943m, -123.06973m);
            await apiClient.Search(location, 1000);
        }
    }
}
