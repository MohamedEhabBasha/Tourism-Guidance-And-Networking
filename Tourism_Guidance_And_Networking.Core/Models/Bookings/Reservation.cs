using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace Tourism_Guidance_And_Networking.Core.Models.Bookings
{
    public class Reservation
    {
        public int Id { get; set; }
        [Range(1, 1000, ErrorMessage = "Only value between 1 and 1000 is allowed")]
        public int Count { get; set; }
        public int? AccommodationId { get; set; } = null;
        [ValidateNever]
        public Accommodation Accommodation { get; set; }
        public int? RoomId { get; set; } = null;
        [ValidateNever]
        public Room Room { get; set; }
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public ApplicationUser ApplicationUser { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public double Price { get; set; }
    }
}
