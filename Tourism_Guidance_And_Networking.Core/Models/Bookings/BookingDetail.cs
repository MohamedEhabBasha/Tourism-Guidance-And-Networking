using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Models.Bookings
{
    public class BookingDetail
    {
        public int Id { get; set; }
        [Required]
        public int BookingHeaderId { get; set; }
        [ForeignKey("BookingHeaderId")]
        [ValidateNever]
        [JsonIgnore]
        public BookingHeader BookingHeader { get; set; }
        public int? AccommodationId { get; set; } = null;
        [ValidateNever]
        [JsonIgnore]
        public Accommodation Accommodation { get; set; }
        public int? RoomId { get; set; } = null;
        [ValidateNever]
        [JsonIgnore]
        public Room Room { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}
