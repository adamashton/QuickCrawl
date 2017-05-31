using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Yelp;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        private readonly ApiClient _apiClient;

        public HealthController(ApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        // GET: api/values
        [HttpGet]
        public async Task<string> Get()
        {
            await HealthCheck.TestYelp(_apiClient);

            return "Ok";
        }


    }
}
