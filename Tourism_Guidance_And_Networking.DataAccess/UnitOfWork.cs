﻿
using Tourism_Guidance_And_Networking.Core.Interfaces.Booking;
using Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface;
using Tourism_Guidance_And_Networking.Core.Models;
using Tourism_Guidance_And_Networking.DataAccess.Repositories.Booking;
using Tourism_Guidance_And_Networking.DataAccess.Repositories.HotelsRepositories;

namespace Tourism_Guidance_And_Networking.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Categories { get; private set; }
        public ITouristPlaceRepository TouristPlaces { get; private set; }
        public IHotelRepository Hotels { get; private set; } 
        public IRoomRepository Rooms { get; private set; }
        public IReservationRepository Reservations { get; private set; }
        public IApplicationUserRepository ApplicationUsers { get; private set; }
        public IBookingHeaderRepository BookingHeaders { get; private set; }
        public IBookingDetailRepository BookingDetails { get; private set; }

        public UnitOfWork(ApplicationDbContext context,IWebHostEnvironment webHost)
        {
            _context = context;
            Categories = new CategoryRepository(_context);
            TouristPlaces = new TouristPlaceRepository(_context, webHost);
            Hotels = new HotelRepository(_context,webHost);
            Rooms = new RoomRepository(_context,webHost);
            Reservations = new ReservationRepository(_context);
            ApplicationUsers = new ApplicationUserRepository(_context);
            BookingHeaders = new BookingHeaderRepository(_context);
            BookingDetails = new BookingDetailRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
