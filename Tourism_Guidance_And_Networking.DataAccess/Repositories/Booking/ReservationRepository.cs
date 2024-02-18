using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Interfaces.Booking;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.Booking
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> Decrement(Reservation item, int count)
        {
            item.Count -= count;
            await _context.SaveChangesAsync();
            return item.Count;
        }

        public async Task<int> Increment(Reservation item, int count)
        {
            item.Count += count;
            await _context.SaveChangesAsync();
            return item.Count;
        }
    }
}
