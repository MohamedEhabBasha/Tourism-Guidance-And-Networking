
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;

namespace Tourism_Guidance_And_Networking.Core.Models.Hotels
{
    public class Accommodation : BaseEntity
    {
        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty; 
        [Required, MaxLength(250)]
        public string Location { get; set; } = string.Empty;
        [Required, MaxLength(250)]
        public string Governorate { get; set; } = string.Empty;
        public string PropertyType {  get; set; } = string.Empty;
        public double Rating { get; set; }
        public int Reviews { get; set; }
        [Required]
        public string Type { get; set; } = string.Empty;
        [Required]
        public double Price { get; set; }
        public double Taxes { get; set; }
        [Required]
        public string Info { get; set; } = string.Empty; 
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string Image { get; set; } = string.Empty;
        [Required]
        public int Capicity { get; set; }
        [Required]
        public int Count { get; set; }
        [Required]
        public int CountOfReserved { get; set; } = 0;
        [Required]
        public int CompanyId { get; set; }
        [JsonIgnore]
        [ValidateNever]
        public Company Company { get; set; } = default!;
    }
}
