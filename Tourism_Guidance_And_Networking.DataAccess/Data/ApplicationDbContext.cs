

using Tourism_Guidance_And_Networking.Core.Models.Bookings;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia;

namespace Tourism_Guidance_And_Networking.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TouristPlace> Tourists { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<BookingDetail> BookingDetails { get; set; }
        public DbSet<BookingHeader> BookingHeaders { get; set; }
        public DbSet<Accommodation> Accommodations { get; set;}
        public DbSet<PrivateChat> PrivateChats { get; set; }
        public DbSet<Message> Messages { get; set; }
    }
}
