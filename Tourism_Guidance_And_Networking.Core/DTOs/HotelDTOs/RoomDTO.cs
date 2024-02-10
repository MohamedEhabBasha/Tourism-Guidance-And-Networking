﻿
namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public class RoomDTO
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Type { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        public double Taxes { get; set; }
        [Required]
        public string Info { get; set; } = string.Empty;

        [Required(ErrorMessage = "Choose an image"),
        FileExtensions(Extensions = FileSettings.AllowedExtensions, ErrorMessage = $"Must be {FileSettings.AllowedExtensions}")]
        public IFormFile ImagePath { get; set; } = default!;
        public int Capicity { get; set; }
        [Required]
        public int HotelId { get; set; }
    }
}
