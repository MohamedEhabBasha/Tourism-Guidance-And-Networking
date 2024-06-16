
namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.TouristPlacesRepositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private new readonly ApplicationDbContext _context;
        private readonly string _imagesPath;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
            _imagesPath = FileSettings.touristplaceImagesPath;
        }
        public async Task<ICollection<Category>> GetCategoryByNameAsync(string name)
        {
            return await _context.Categories
                    .Where(c => c.Name.Trim().ToLower().Contains(name))
                    .AsNoTracking()
                    .ToListAsync();
        }

        public ICollection<TouristPlaceOutputDTO> GetTouristPlacesById(int categoryId)
        {
            return  _context.Tourists
                    .Where(c => c.CategoryId == categoryId)
                    .Select(t => new TouristPlaceOutputDTO
                    {
                        Id = t.Id,
                        Name = t.Name,
                        Description = t.Description??"",
                        CategoryId = t.CategoryId,
                        ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{t.Image}"
                    })
                    .AsNoTracking()
                    .ToList();
        }

        public bool ExistByName(string name)
        {
            return _context.Categories.SingleOrDefault(c => c.Name.Trim().ToLower().Contains(name)) != null;
        }
    }
}
