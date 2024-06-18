

using Tourism_Guidance_And_Networking.Core.Models.Bookings;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia.POST;

namespace Tourism_Guidance_And_Networking.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserMatrix>().HasKey(u => new { u.UserID, u.ItemID, u.Action});
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
        public DbSet<Friend> Friends { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Post> Posts {  get; set; }
        public DbSet<PostLikes> PostLikes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<CommentLikes> CommentLikes { get; set; }
        public DbSet<TouristProfileImage> TouristProfilesImages { get; set; }
        public DbSet<UserMatrix> UserMatrices { get; set; }
    }
}
