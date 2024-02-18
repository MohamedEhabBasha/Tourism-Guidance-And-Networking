using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Interfaces.Booking;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.Booking
{
    public class BookingHeaderRepository : BaseRepository<BookingHeader>, IBookingHeaderRepository
    {
        public BookingHeaderRepository(ApplicationDbContext context) : base(context)
        {
        }
        public async Task<bool> UpdateSessionAndPaymentId(int id, string sessionId, string paymentIntentId)
        {
            var bookingDb = await GetByIdAsync(id);
            if (bookingDb != null)
            {
                bookingDb.SessionId = sessionId;
                bookingDb.PaymentIntentId = paymentIntentId;
                return true;
            }
            return false;
        }

        public async Task<bool> UpdateStatus(int id, string BookingStatus, string? paymentStatus = null)
        {
            var bookingDb = await GetByIdAsync(id);

            if (bookingDb != null)
            {
                bookingDb.BookingStatus = BookingStatus;
                if (paymentStatus != null)
                {
                    bookingDb.PaymentStatus = paymentStatus;
                }
                return true;
            }
            return false;
        }

    }
}
