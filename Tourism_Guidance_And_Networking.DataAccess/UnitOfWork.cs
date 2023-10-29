

namespace Tourism_Guidance_And_Networking.DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;


        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
             _context.Dispose();
        }
    }
}
