using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.DTOs.Booking
{
    public class BookingsOfHotelDTO
    {
        public int Id { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int BookingHeaderId { get; set; }
        public Room Room { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }
}
