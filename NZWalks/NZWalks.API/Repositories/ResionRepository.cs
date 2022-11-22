using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class ResionRepository : IResionRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public ResionRepository(NZWalksDbContext nZWalksDbContext)
        {
           _nZWalksDbContext = nZWalksDbContext;
        }
        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await _nZWalksDbContext.Regions.ToListAsync();
        }
    }
}
