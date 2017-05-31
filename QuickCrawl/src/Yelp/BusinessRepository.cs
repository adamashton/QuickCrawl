using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yelp
{
    public class BusinessRepository
    {
        private readonly ApiClient _client;

        public BusinessRepository(ApiClient client)
        {
            _client = client;
        }

        public void GetBusinesses(decimal latitude, decimal longitude)
        {
            
        }
    }
}
