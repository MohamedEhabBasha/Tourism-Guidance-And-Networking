

namespace Tourism_Guidance_And_Networking.Core.Interfaces.TouristPlacesInterfaces
{
    public interface ITouristPlaceRepository : IBaseRepository<TouristPlace>
    {
        Task<ICollection<TouristPlaceOutputDTO>> GetTouristPlacesAsync();
        Task<ICollection<TouristPlaceOutputDTO>> GetTouristPlacesByCategoryIdAsync(int categoryId);
        Task<ICollection<TouristPlaceOutputDTO>> GetTouristPlacesByCategoryName(string name);
        Task<TouristPlace> CreateTouristPlace(TouristPlaceDTO touristPlaceDTO);
        Task<ICollection<TouristPlaceOutputDTO>> SearchByName(string name);
        TouristPlaceOutputDTO UpdateTouristPlace(int touristId, TouristPlaceDTO touristPlaceDTO);
        bool DeleteTouristPlace(int id);
        static abstract TouristPlaceOutputDTO ToTouristPlaceOutputDto(TouristPlace touristPlace);
    }
}
