
namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public class AccommodationOutputDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int Reviews { get; set; }
        public string Type { get; set; } = string.Empty;
        public double Price { get; set; }
        public double Taxes { get; set; }
        public string Info { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public int Capicity { get; set; }
        public int Count { get; set; }
        public int CompanyId { get; set; }
    }
}
