using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Crawl;
using Core;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class CrawlsController : Controller
    {
        private readonly QuickCrawlEngine _quickCrawlEngine;

        public CrawlsController(QuickCrawlEngine quickCrawlEngine)
        {
            _quickCrawlEngine = quickCrawlEngine;
        }

        // GET: api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            Coordinates start = new Coordinates(49.259508m, -123.069917m);
            Coordinates end = new Coordinates(49.274805m, -123.069541m);
            CrawlSettings crawlSettings = new CrawlSettings()
            {
                Start = start,
                End = end,
                Size = 3
            };

            _quickCrawlEngine.Generate(crawlSettings);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
