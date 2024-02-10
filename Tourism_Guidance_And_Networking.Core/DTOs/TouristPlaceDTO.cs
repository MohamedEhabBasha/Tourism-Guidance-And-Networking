﻿
namespace Tourism_Guidance_And_Networking.Core.DTOs
{
    public class TouristPlaceDTO : BaseDTO
    {
        [MaxLength(2500)]
        [Required]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Choose an image"),
        FileExtensions(Extensions =FileSettings.AllowedExtensions, ErrorMessage = $"Must be {FileSettings.AllowedExtensions}")]
        public IFormFile ImagePath { get; set; } = default!;

        [Required]
        public int CategoryId { get; set; }
    }
}
