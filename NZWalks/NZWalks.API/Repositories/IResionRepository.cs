using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IResionRepository
    {
        Task<IEnumerable<Region>> GetAllAsync();
    }
}
