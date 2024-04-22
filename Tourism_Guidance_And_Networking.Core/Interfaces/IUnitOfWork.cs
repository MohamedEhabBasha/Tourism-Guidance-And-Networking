

using Tourism_Guidance_And_Networking.Core.Interfaces.Booking;
using Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface;
using Tourism_Guidance_And_Networking.Core.Interfaces.SocialMedia;
using Tourism_Guidance_And_Networking.Core.Interfaces.TouristPlacesInterfaces;

namespace Tourism_Guidance_And_Networking.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
       public ICategoryRepository Categories { get; }
       public ITouristPlaceRepository TouristPlaces { get; }
       public IHotelRepository Hotels { get; }
       public IRoomRepository Rooms { get; }
        public ICompanyRepository Companies { get; }
       public IAccommodationRepository Accommodations { get; }
       public IReservationRepository Reservations { get; }
       public IApplicationUserRepository ApplicationUsers { get; }
       public IBookingDetailRepository BookingDetails { get; }
       public IBookingHeaderRepository BookingHeaders { get; }
        public IPrivateChatRepository PrivateChats { get; }
        public IMessageRepository Messages { get; }

        public IUserProfileRepository UserProfiles { get; }

        public IPostRepository Posts { get; }
        public ICommentRepository Comments { get; }
        int Complete();
    }
}
