

using Microsoft.AspNetCore.Hosting;
using Tourism_Guidance_And_Networking.Core.Interfaces.TouristPlacesInterfaces;
using Tourism_Guidance_And_Networking.Core.Models.TouristPlaces;
using Tourism_Guidance_And_Networking.DataAccess.Repositories.TouristPlacesRepositories;

namespace Tourism_Guidance_And_Networking.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Categories { get; private set; }
        public ITouristPlaceRepository Tours { get; private set; }
        public UnitOfWork(ApplicationDbContext context,IWebHostEnvironment webHost)
        {
            _context = context;
            Categories = new CategoryRepository(_context);
            Tours = new TouristPlaceRepository(_context, webHost);
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
