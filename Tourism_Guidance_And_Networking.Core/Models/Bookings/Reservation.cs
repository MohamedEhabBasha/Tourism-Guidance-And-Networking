using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Models.Bookings
{
    public class Reservation
    {
        public int Id { get; set; }
        [Range(1, 1000, ErrorMessage = "Only value between 1 and 1000 is allowed")]
        public int Count { get; set; }
        public int AcomdationId { get; set; } = 0;
        [ValidateNever]
        public Accommodation Accommodation { get; set; }
        public int RoomId { get; set; } = 0;
        [ValidateNever]
        public Room Room { get; set; }
        public int HotelId { get; set; } = 0;
        [ValidateNever]
        public Hotel Hotel { get; set; }
        public string ApplicationUserId { get; set; }
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        [NotMapped]
        public double Price { get; set; }
    }
}
