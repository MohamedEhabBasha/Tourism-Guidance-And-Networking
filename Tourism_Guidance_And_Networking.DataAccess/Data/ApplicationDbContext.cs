


using Tourism_Guidance_And_Networking.Core.Models.TouristPlaces;

namespace Tourism_Guidance_And_Networking.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TouristPlace> Tourists { get; set; }
    }
}
