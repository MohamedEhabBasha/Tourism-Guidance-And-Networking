
namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public class HotelDTO : BaseDTO
    {
        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        public double Rating { get; set; }

        public int Reviews { get; set; }

        [Required(ErrorMessage = "Choose an image"),
        FileExtensions(Extensions = FileSettings.AllowedExtensions, ErrorMessage = $"Must be {FileSettings.AllowedExtensions}")]
        public IFormFile ImagePath { get; set; } = default!;
    }
}
