using Tourism_Guidance_And_Networking.Core.Models.TouristPlaces;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.TouristPlacesInterfaces
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<ICollection<TouristPlace>> GetTouristPlacesByIdAsync(int categoryId);
        ICollection<TouristPlace> GetTouristPlacesById(int categoryId);
        Task<ICollection<TouristPlace>> GetTouristPlacesByName(string name);
        bool ExistByName(string name);
    }
}
