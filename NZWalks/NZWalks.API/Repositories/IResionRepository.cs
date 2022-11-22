using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IResionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
        Task<Region> GetAsyncById(Guid id);

        Task<Region> AddRegionAsync(Region region);

        Task<Region> DeleteRegionAsync(Guid id);

        Task<Region> UpdateRegionAsync(Guid id,Region region);
    }
}
