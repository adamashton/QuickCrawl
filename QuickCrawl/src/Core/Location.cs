namespace Core
{
    public struct Location
    {
        public decimal Latitude { get; private set; }

        public decimal Longitude { get; private set; }

        public Location(decimal latitude, decimal longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
    }
}
