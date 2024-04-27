
namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public class RoomOutputDTO
    {
        public string Type { get; set; } = string.Empty;
        public double Price { get; set; }
        public int Count { get; set; }
        public double Taxes { get; set; }
        public string Info { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public int Capicity { get; set; }
        public int HotelId { get; set; }
    }
}
