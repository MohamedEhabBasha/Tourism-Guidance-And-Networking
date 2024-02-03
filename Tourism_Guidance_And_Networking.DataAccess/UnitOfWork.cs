
namespace Tourism_Guidance_And_Networking.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public ICategoryRepository Categories { get; private set; }
        public ITouristPlaceRepository TouristPlaces { get; private set; }
        public UnitOfWork(ApplicationDbContext context,IWebHostEnvironment webHost)
        {
            _context = context;
            Categories = new CategoryRepository(_context);
            TouristPlaces = new TouristPlaceRepository(_context, webHost);
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
