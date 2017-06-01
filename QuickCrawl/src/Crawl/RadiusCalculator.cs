using Core;

namespace Crawl
{
    public class RadiusCalculator
    {
        public static SearchArea FindSearchArea(Coordinates start, Coordinates end)
        {
            decimal longitudeCentre = FindMidPoint(start.Longitude, end.Longitude);
            decimal latitudeCentre = FindMidPoint(start.Latitude, end.Latitude);
            Coordinates centre = new Coordinates(latitudeCentre, longitudeCentre);
            int radius = FindRadius(start, end, centre);
            return new SearchArea()
            {
                Centre = centre,
                Radius = radius
            };
        }

        private static int FindRadius(Coordinates start, Coordinates end, Coordinates centre)
        {
            // TODO - at the moment we return 3km
            // bit of math needed to discover m from lat/lon https://en.wikipedia.org/wiki/Geographic_coordinate_system#Expressing_latitude_and_longitude_as_linear_units
            return 3000;
        }

        private static decimal FindMidPoint(decimal start, decimal end)
        {
            if (start > end)
                return FindMidPoint(end, start);

            decimal halfOfLength = (end - start)/2;
            return start + halfOfLength;
        }
    }

    public struct SearchArea
    {
        public Coordinates Centre { get; set; }
        public int Radius { get; set; }
    }
}
