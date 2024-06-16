
namespace Tourism_Guidance_And_Networking.Core.Interfaces.TouristPlacesInterfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<ICollection<Category>> GetCategoryByNameAsync(string name);
        ICollection<TouristPlaceOutputDTO> GetTouristPlacesById(int categoryId);
        bool ExistByName(string name);
    }
}
