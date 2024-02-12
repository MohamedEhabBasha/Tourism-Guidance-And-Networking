
using Tourism_Guidance_And_Networking.Core.Attribute;

namespace Tourism_Guidance_And_Networking.Core.DTOs
{
    public class TouristPlaceDTO 
    {
        [Required]
        [MaxLength(250)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(2500)]
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Choose an image"),
        AllowedExtenstions(FileSettings.AllowedExtensions)]
        public IFormFile ImagePath { get; set; } = default!;

        [Required]
        public int CategoryId { get; set; }
    }
}
