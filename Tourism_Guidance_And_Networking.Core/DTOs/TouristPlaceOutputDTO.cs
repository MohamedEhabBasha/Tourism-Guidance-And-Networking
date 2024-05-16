namespace Tourism_Guidance_And_Networking.Core.DTOs
{
    public class TouristPlaceOutputDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public int CategoryId { get; set; }
    }
}
