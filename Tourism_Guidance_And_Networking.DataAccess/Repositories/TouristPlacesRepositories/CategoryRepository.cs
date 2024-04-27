
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
        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            return await _context.Categories
                    .AsNoTracking()
                    .SingleAsync(c => c.Name == name);
        }
        public async Task<ICollection<TouristPlaceOutputDTO>> GetTouristPlacesByIdAsync(int categoryId)
        {
            return await _context.Tourists
                    .Where(c => c.CategoryId == categoryId)
                    .Select(t => new TouristPlaceOutputDTO
                    {
                        Name = t.Name,
                        Description = t.Description ?? "",
                        CategoryId = t.CategoryId,
                        ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{t.Image}"
                    })
                    .AsNoTracking()
                    .ToListAsync();
        }
        public ICollection<TouristPlaceOutputDTO> GetTouristPlacesById(int categoryId)
        {
            return  _context.Tourists
                    .Where(c => c.CategoryId == categoryId)
                    .Select(t => new TouristPlaceOutputDTO
                    {
                        Name = t.Name,
                        Description = t.Description??"",
                        CategoryId = t.CategoryId,
                        ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{t.Image}"
                    })
                    .AsNoTracking()
                    .ToList();
        }
        public async Task<ICollection<TouristPlaceOutputDTO>> GetTouristPlacesByName(string name)
        {
            Category category = await GetCategoryByNameAsync(name);
            return await _context.Tourists
                .Where(c => c.CategoryId == category.Id)
                .Select(t => new TouristPlaceOutputDTO
                {
                    Name = t.Name,
                    Description = t.Description ?? "",
                    CategoryId = t.CategoryId,
                    ImageURL = $"{FileSettings.RootPath}/{_imagesPath}/{t.Image}"
                })
                .AsNoTracking()
                .ToListAsync();
        }
        public bool ExistByName(string name)
        {
            return _context.Categories.SingleOrDefault(c => c.Name.Trim().ToLower() == name) != null;
        }
    }
}
