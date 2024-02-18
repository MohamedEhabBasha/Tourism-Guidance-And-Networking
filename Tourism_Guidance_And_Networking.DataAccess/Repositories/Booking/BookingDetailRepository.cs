using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Interfaces.Booking;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.Booking
{
    public class BookingDetailRepository : BaseRepository<BookingDetail>, IBookingDetailRepository
    {
        public BookingDetailRepository(ApplicationDbContext context) : base(context)
        {
        }

        
    }
}
