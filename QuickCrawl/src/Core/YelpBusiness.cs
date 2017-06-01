using System;

namespace Core
{
    public class YelpBusiness : IEquatable<YelpBusiness>
    {
        public string Id { get; }
        public string Name { get; set; }
        public int ReviewCount { get; set; }
        public Coordinates Coordinates { get; set; }
        public double Rating { get; set; }

        public YelpBusiness(string id)
        {
            this.Id = id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return Name ?? Id;
        }

        public bool Equals(YelpBusiness other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Id, other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((YelpBusiness) obj);
        }

        public static bool operator ==(YelpBusiness left, YelpBusiness right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(YelpBusiness left, YelpBusiness right)
        {
            return !Equals(left, right);
        }
    }
}
