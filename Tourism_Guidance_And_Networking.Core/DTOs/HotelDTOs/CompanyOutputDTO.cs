
namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public class CompanyOutputDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int Reviews { get; set; }
        public string ImageURL { get; set; } = string.Empty;
    }
}
