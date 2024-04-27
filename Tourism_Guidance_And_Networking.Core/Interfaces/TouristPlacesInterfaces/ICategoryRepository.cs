
namespace Tourism_Guidance_And_Networking.Core.Interfaces.TouristPlacesInterfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category> GetCategoryByNameAsync(string name);
        Task<ICollection<TouristPlaceOutputDTO>> GetTouristPlacesByIdAsync(int categoryId);
        ICollection<TouristPlaceOutputDTO> GetTouristPlacesById(int categoryId);
        Task<ICollection<TouristPlaceOutputDTO>> GetTouristPlacesByName(string name);
        bool ExistByName(string name);
    }
}
