
using Tourism_Guidance_And_Networking.Core.Attribute;

namespace Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs
{
    public class PostInputDTO
    {
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Choose an image")]
        [NotMapped]
        public IFormFile ImagePath { get; set; } = default!;
        [Required]
        public string UserId { get; set; } = string.Empty;
    }
}
