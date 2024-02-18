using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.Booking
{
    public interface IBookingHeaderRepository : IBaseRepository<BookingHeader>
    {
        public Task<bool> UpdateStatus(int id, string BookingStatus, string? paymentStatus = null);
        public Task<bool> UpdateSessionAndPaymentId(int id, string sessionId, string paymentIntentId);
    }
   

}
