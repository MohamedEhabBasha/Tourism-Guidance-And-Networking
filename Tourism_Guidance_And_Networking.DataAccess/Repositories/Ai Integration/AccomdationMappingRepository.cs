using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Interfaces.Ai_Inegration;
using Tourism_Guidance_And_Networking.Core.Interfaces.Booking;
using Tourism_Guidance_And_Networking.Core.Models.AI_Integration;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.Ai_Integration
{
    public class RoomMappingRepository : BaseRepository<RoomMapping>, IRoomMappingRepository
    {
        public RoomMappingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
