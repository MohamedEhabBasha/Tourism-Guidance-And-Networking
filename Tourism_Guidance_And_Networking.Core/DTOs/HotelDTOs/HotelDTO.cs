

namespace Tourism_Guidance_And_Networking.Core.DTOs.HotelDTOs
{
    public class HotelDTO 
    {
        [MaxLength(250)]
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty;
        public double Rating { get; set; }

        public int Reviews { get; set; }

        [Required(ErrorMessage = "Choose an image")]
        [NotMapped]
        public IFormFile ImagePath { get; set; } = default!;
        
        public string? ApplicationUserId { get; set; }
    }
}
