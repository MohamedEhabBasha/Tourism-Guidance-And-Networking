

using Tourism_Guidance_And_Networking.Core.Interfaces.HotelInterface;
using Tourism_Guidance_And_Networking.Core.Interfaces.TouristPlacesInterfaces;

namespace Tourism_Guidance_And_Networking.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
       public ICategoryRepository Categories { get; }
       public ITouristPlaceRepository TouristPlaces { get; }
       public IHotelRepository Hotels { get; }
       public IRoomRepository Rooms { get; }
        int Complete();
    }
}
