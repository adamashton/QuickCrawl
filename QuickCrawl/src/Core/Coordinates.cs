namespace Core
{
    public class Coordinates
    {
        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public Coordinates(decimal latitude, decimal longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
