using NZWalks.API.Models.Domain;
using System.Collections;

namespace NZWalks.API.Repositories
{
    public interface IWalksRepository
    {
        Task<IEnumerable<Walk>> AllWalksAsync();

        Task<Walk> GetWalkByIdAsync(Guid id);

        Task<Walk> AddWalkAsyn(Walk walk);

        Task<Walk> UpdateWalkAsync(Guid id,Walk walk);

        Task<Walk> DeleteWalkAsync(Guid id);
    }
}
