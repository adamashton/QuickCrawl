using System;
using System.Collections.Generic;
using System.Linq;
using Core;

namespace Crawl
{
    public class CrawlGenerator
    {
        private readonly CrawlSettings _crawlSettings;
        private readonly ICollection<YelpBusiness> _yelpBusinesses;
        private Dictionary<YelpBusiness, double> _distanceFromStart;
        private Dictionary<YelpBusiness, double> _distanceToEnd;

        private List<PubCrawl> _generatedCrawls;

        public CrawlGenerator(CrawlSettings crawlSettings, ICollection<YelpBusiness> yelpBusinesses)
        {
            _crawlSettings = crawlSettings;
            _yelpBusinesses = yelpBusinesses;
            GenerateDistancesFromStart();
            GenerateDistancesToEnd();
        }

        public List<PubCrawl> Generate()
        {
            if (_crawlSettings.Size == 0)
                throw new Exception("Crawl Size is 0");

            if (_crawlSettings.Size > _yelpBusinesses.Count)
                throw new Exception("Size of pub crawl larger than business available");

            _generatedCrawls = new List<PubCrawl>();

            // slightly better to use sorted pubs in generation
            YelpBusiness[] businessForGraphTraverse = _yelpBusinesses.OrderBy(x => _distanceFromStart[x]).ToArray();

            // generate the list of crawls
            GenerateCrawls(businessForGraphTraverse, new Stack<YelpBusiness>());

            return _generatedCrawls;
        }

        private void GenerateCrawls(YelpBusiness[] availablePubs, Stack<YelpBusiness> currentWalk)
        {
            // base case 1: there are no more steps to take
            bool finishedWalk = false;
            if (availablePubs.Length == 0)
                finishedWalk = true;

            // base case 2: the size of the pub crawl has been reached
            if (currentWalk.Count == _crawlSettings.Size)
                finishedWalk = true;

            if (finishedWalk)
                AddGeneratedPubCrawl(currentWalk);
            else
            {
                // otherwise, let's keep creating crawls...
                YelpBusiness[] validNextSteps;
                if (currentWalk.Count == 0)
                {
                    // first step - all pubs are valid at this point
                    validNextSteps = availablePubs.ToArray();
                }
                else
                {
                    var lastStep = currentWalk.Peek();
                    validNextSteps = availablePubs
                        .Where(nextStep => IsValidNextBusiness(lastStep, nextStep))
                        .ToArray();
                }

                foreach (var validStep in validNextSteps)
                {
                    currentWalk.Push(validStep);

                    var remainingAvailablePubs = new YelpBusiness[availablePubs.Length - 1];
                    Array.Copy(availablePubs, 1, remainingAvailablePubs, 0, remainingAvailablePubs.Length);

                    GenerateCrawls(availablePubs, currentWalk);

                    currentWalk.Pop();
                }
            }
        }

        private void AddGeneratedPubCrawl(IEnumerable<YelpBusiness> currentWalk)
        {
            // reverse list to get the correct order
            List<YelpBusiness> businessInOrder = currentWalk.Reverse().ToList();

            PubCrawl pubCrawl = new PubCrawl
            {
                Businesses = businessInOrder
            };
            _generatedCrawls.Add(pubCrawl);
        }

        private bool IsValidNextBusiness(YelpBusiness a, YelpBusiness b)
        {
            if (a == b)
                return false;

            // b is valid after a only if b is farther from the start than a
            bool furtherFromStart = _distanceFromStart[b] >= _distanceFromStart[a];

            // or if b is closer to end than a
            bool closerToEnd = _distanceToEnd[b] <= _distanceToEnd[a];

            return furtherFromStart || closerToEnd;
        }

        private void GenerateDistancesToEnd()
        {
            _distanceToEnd = new Dictionary<YelpBusiness, double>();
            foreach (var business in _yelpBusinesses)
            {
                double distance = CalculateDistance(business.Coordinates, _crawlSettings.End);
                _distanceToEnd.Add(business, distance);
            }
        }

        private void GenerateDistancesFromStart()
        {
            _distanceFromStart = new Dictionary<YelpBusiness, double>();
            foreach (var business in _yelpBusinesses)
            {
                double distance = CalculateDistance(_crawlSettings.Start, business.Coordinates);
                _distanceFromStart.Add(business, distance);
            }
        }

        private double CalculateDistance(Coordinates a, Coordinates b)
        {
            //d = Sqtr((a.lat-b.lat)^2 + (a.lon-b.lon)^2)
            var latDistSquared = Math.Pow((double) (a.Latitude - b.Latitude), 2.0d);
            var lonDistSquared = Math.Pow((double) (a.Longitude - b.Longitude), 2.0d);
            double result = Math.Sqrt(latDistSquared + lonDistSquared);
            return result;
        }
    }
}