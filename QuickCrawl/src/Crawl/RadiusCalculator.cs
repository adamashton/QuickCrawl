using Core;

namespace Crawl
{
    public class RadiusCalculator
    {
        public static SearchArea FindSearchArea(Location start, Location end)
        {
            decimal longitudeCentre = FindMidPoint(start.Longitude, end.Longitude);
            decimal latitudeCentre = FindMidPoint(start.Latitude, end.Latitude);
            Location centre = new Location(latitudeCentre, longitudeCentre);
            int radius = FindRadius(start, end, centre);
            return new SearchArea()
            {
                Centre = centre,
                Radius = radius
            };
        }

        private static int FindRadius(Location start, Location end, Location centre)
        {
            // TODO - at the moment we return 5km
            // bit of math needed to discover m from lat/lon https://en.wikipedia.org/wiki/Geographic_coordinate_system#Expressing_latitude_and_longitude_as_linear_units
            return 5000;
        }

        private static decimal FindMidPoint(decimal start, decimal end)
        {
            if (start > end)
                return FindMidPoint(end, start);

            return (end - start)/2;
        }
    }

    public struct SearchArea
    {
        public Location Centre { get; set; }
        public int Radius { get; set; }
    }
}
