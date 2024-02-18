using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;

namespace Tourism_Guidance_And_Networking.Core.DTOs.Booking
{
    public class SummaryDto
    {
        public IEnumerable<Reservation> ReservationList { get; set; }
        public BookingHeader BookingHeader { get; set; }
    }
}
