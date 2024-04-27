

namespace Tourism_Guidance_And_Networking.Core.Interfaces.TouristPlacesInterfaces
{
    public interface ITouristPlaceRepository : IBaseRepository<TouristPlace>
    {
        Task<TouristPlaceOutputDTO> CreateTouristPlace(TouristPlaceDTO touristPlaceDTO);
        Task<ICollection<TouristPlaceOutputDTO>> SearchByName(string name);
        Task<TouristPlaceOutputDTO?> UpdateTouristPlace(int touristId, TouristPlaceDTO touristPlaceDTO);
        bool DeleteTouristPlace(int id);
    }
}
