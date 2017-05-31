using Yelp;

namespace Api
{
    public class LogLevel
    {
        public string Default { get; set; }
        public string System { get; set; }
        public string Microsoft { get; set; }
    }

    public class Logging
    {
        public bool IncludeScopes { get; set; }
        public LogLevel LogLevel { get; set; }
    }

    public class AppSettings
    {
        public YelpSettings YelpSettings { get; set; }
        public Logging Logging { get; set; }
    }
}
