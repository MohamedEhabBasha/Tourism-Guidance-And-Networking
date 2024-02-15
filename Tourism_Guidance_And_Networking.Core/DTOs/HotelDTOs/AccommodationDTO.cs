﻿

using Tourism_Guidance_And_Networking.Core.Attribute;

namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public class AccommodationDTO
    {
        [MaxLength(250)]
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        public double Rating { get; set; }

        public int Reviews { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        public double Taxes { get; set; }
        [Required]
        public string Info { get; set; } = string.Empty;

        [Required(ErrorMessage = "Choose an image"),
        AllowedExtenstions(FileSettings.AllowedExtensions)]
        public IFormFile ImagePath { get; set; } = default!;
        [Required]
        public int Capicity { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public int CompanyId { get; set; }
    }
}
