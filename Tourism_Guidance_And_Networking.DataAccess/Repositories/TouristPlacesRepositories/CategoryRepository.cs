

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.TouristPlacesRepositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private new readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ICollection<TouristPlace>> GetTouristPlacesByIdAsync(int categoryId)
        {
            return await _context.Tourists
                    .Where(c => c.CategoryId == categoryId)
                    .AsNoTracking()
                    .ToListAsync();
        }
        public ICollection<TouristPlace> GetTouristPlacesById(int categoryId)
        {
            return  _context.Tourists
                    .Where(c => c.CategoryId == categoryId)
                    .AsNoTracking()
                    .ToList();
        }
        public async Task<ICollection<TouristPlace>> GetTouristPlacesByName(string name)
        {
            return await _context.Tourists
                .Where(c => c.Name == name)
                .AsNoTracking()
                .ToListAsync();
        }
        public bool ExistByName(string name)
        {
            return _context.Categories.SingleOrDefault(c => c.Name.Trim().ToLower() == name) != null;
        }
    }
}
