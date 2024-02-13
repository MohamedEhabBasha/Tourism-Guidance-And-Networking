

using Tourism_Guidance_And_Networking.Core.Models.Bookings;

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
    }
}
