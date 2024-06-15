
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;

namespace Tourism_Guidance_And_Networking.Core.Models.Hotels
{
    public class Hotel : BaseEntity
    {
        [Required,MaxLength(250)]
        public string Address { get; set; } = string.Empty;
        [Required, MaxLength(250)]
        public string Location { get; set; } = string.Empty;
        public double Rating { get; set; }

        public int Reviews { get; set; }
        [Required, MaxLength(250)]
        public string Governorate { get; set; } = string.Empty;

        [Required]
        public string Image { get; set; } = string.Empty;
        [JsonIgnore]
        [ValidateNever]
        public ICollection<Room> Rooms { get; set; } = default!;

        [Required]
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; }

    }
}
