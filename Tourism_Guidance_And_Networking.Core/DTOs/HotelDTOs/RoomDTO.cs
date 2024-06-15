
using Tourism_Guidance_And_Networking.Core.Attribute;

namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public class RoomDTO
    {

        [MaxLength(250)]
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public double Taxes { get; set; }
        [Required]
        public string Info { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Choose an image"),
         AllowedExtenstions(FileSettings.AllowedExtensions)]
        public IFormFile ImagePath { get; set; } = default!;
        [Required]
        public int Capicity { get; set; }
        [Required]
        public int HotelId { get; set; }
    }
}
