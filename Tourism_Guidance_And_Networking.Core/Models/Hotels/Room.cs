﻿
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
        public int Capicity { get; set; }
        [Required]
        public int HotelId { get; set; }
        [JsonIgnore]
        public Hotel Hotel { get; set; } = default!;
    }
}
