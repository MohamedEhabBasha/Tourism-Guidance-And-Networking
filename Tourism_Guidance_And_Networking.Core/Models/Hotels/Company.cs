﻿

using System.Text.Json.Serialization;

namespace Tourism_Guidance_And_Networking.Core.Models.Hotels
{
    public class Company : BaseEntity
    {
        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty;

        public double Rating { get; set; }

        public int Reviews { get; set; }

        [Required]
        public string Image { get; set; } = string.Empty;
        [JsonIgnore]
        public ICollection<Accommodation> Accommodations { get; set; } = default!;
    }
}