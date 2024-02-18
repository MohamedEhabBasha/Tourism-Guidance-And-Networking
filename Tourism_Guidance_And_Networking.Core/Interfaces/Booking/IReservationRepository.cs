using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;
using Tourism_Guidance_And_Networking.Core.Models.Hotels;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.Booking
{
    public interface IReservationRepository : IBaseRepository<Reservation>
    {

        public  Task<int> Decrement(Reservation item, int count);
        public  Task<int> Increment(Reservation item, int count);
    }
   

}
