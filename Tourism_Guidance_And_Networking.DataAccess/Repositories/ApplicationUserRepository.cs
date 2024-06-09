using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tourism_Guidance_And_Networking.Core.Interfaces.Booking;
using Tourism_Guidance_And_Networking.Core.Models.Bookings;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.Booking
{
    public class ApplicationUserRepository : BaseRepository<ApplicationUser>, IApplicationUserRepository
    {
        private new readonly ApplicationDbContext _context;
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ApplicationUser> GetApplicationUserByUserName(string userName)
        {
            return await _context.ApplicationUsers.FirstAsync(x => x.UserName == userName);
        }
        public ApplicationUser? GetApplicationUserById(string userId) => _context.ApplicationUsers.SingleOrDefault(a => a.Id == userId);
    }
}
