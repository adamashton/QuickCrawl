using Core;

namespace Yelp
{
    public class YelpBusinessConverter
    {
        public static YelpBusiness Convert(Business business)
        {
            YelpBusiness result = new YelpBusiness(business.id)
            {
                Coordinates = business.coordinates,
                Name = business.name,
                Rating = business.rating,
                ReviewCount = business.review_count
            };
            return result;
        }
    }
}
