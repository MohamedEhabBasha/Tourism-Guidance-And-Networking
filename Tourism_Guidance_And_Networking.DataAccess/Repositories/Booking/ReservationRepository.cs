using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Interfaces.Booking;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.Booking
{
    public class ReservationRepository : BaseRepository<Reservation>, IReservationRepository
    {
        public ReservationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<int> Decrement(Reservation item, int count)
        {
            TimeSpan difference = item.EndDate - item.StartDate;
            int numberOfDays = difference.Days;
            numberOfDays++;
            item.Count -= count;

            if (item.RoomId is null)
            {
                double totalPrice = (item.Accommodation.Price + item.Accommodation.Taxes )* numberOfDays;

                item.Price -= (totalPrice * count);
            }

            else
            {
                double totalPrice = (item.Room.Price + item.Room.Taxes) * numberOfDays ;

                item.Price -= (totalPrice * count);

            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Increment(Reservation item, int count)
        {
            item.Count += count;

            TimeSpan difference = item.EndDate - item.StartDate;
            int numberOfDays = difference.Days;
            numberOfDays++;



            if (item.RoomId is null)
            {
                double totalPrice = (item.Accommodation.Price + item.Accommodation.Taxes) * numberOfDays;

                item.Price += (totalPrice * count);
            }

            else
            {
                double totalPrice = (item.Room.Price + item.Room.Taxes) * numberOfDays;

                item.Price += (totalPrice * count);

            }

            return await _context.SaveChangesAsync();
            
        }

       
    }
}
