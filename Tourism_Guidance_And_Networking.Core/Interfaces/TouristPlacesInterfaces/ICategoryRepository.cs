
namespace Tourism_Guidance_And_Networking.Core.Interfaces.TouristPlacesInterfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category> GetCategoryByNameAsync(string name);
        Task<ICollection<TouristPlace>> GetTouristPlacesByIdAsync(int categoryId);
        ICollection<TouristPlace> GetTouristPlacesById(int categoryId);
        Task<ICollection<TouristPlace>> GetTouristPlacesByName(string name);
        bool ExistByName(string name);
    }
}
