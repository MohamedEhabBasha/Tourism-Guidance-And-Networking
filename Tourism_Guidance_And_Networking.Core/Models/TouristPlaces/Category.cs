namespace Tourism_Guidance_And_Networking.Core.Models.TouristPlaces
{
    public class Category : BaseEntity
    {
        public ICollection<TouristPlace>? TouristPlaces { get; set; }
    }
}
