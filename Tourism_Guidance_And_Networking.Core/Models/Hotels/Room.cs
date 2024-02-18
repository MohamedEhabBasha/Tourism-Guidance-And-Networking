
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace Tourism_Guidance_And_Networking.Core.Models.Hotels
{
    public class Room 
    {
        public int Id { get; set; }

        [MaxLength(250)]
        public string Type { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        public double Taxes { get; set; }
        [Required]
        public string Info { get; set; } = string.Empty;
        [Required]
        public string Image { get; set; } = string.Empty;
        [Required]
        public int Capicity { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public int CountOfReserved { get; set; } = 0;
        [Required]
        public int HotelId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public Hotel Hotel { get; set; } = default!;
    }
}
