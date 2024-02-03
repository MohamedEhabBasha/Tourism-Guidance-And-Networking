namespace Tourism_Guidance_And_Networking.Core.Models.TouristPlaces
{
    public class TouristPlace : BaseEntity
    {
        [MaxLength(2500)]
        public string? Description { get; set; }

        [MaxLength(500)]
        public string Image { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; } = default!;
    }
}
