using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using System.Collections;

namespace NZWalks.API.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;

        public WalksRepository(NZWalksDbContext nZWalksDbContext) 
        {
            _nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<IEnumerable<Walk>> AllWalksAsync()
        {
            return await _nZWalksDbContext
                .Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .ToListAsync();            
        }

        public async Task<Walk> GetWalkByIdAsync(Guid id)
        {
            var walk = await _nZWalksDbContext
                .Walks
                .Include(x=>x.Region)
                .Include(x=>x.WalkDifficulty)
                .FirstOrDefaultAsync(x=>x.Id==id);

            return walk;
        }

        public async Task<Walk> AddWalkAsyn(Walk walk)
        {
            walk.Id = new Guid();
            await _nZWalksDbContext.AddAsync(walk);
            await _nZWalksDbContext.SaveChangesAsync();

            return walk;          
        }
        
        public async Task<Walk> UpdateWalkAsync(Guid id, Walk walk)
        {
            var existingWalk= await _nZWalksDbContext.Walks.FirstOrDefaultAsync(x=>x.Id== id);

            if(walk==null) 
            {
                return null;
            }
            existingWalk.Id = id;
            existingWalk.Name= walk.Name;
            existingWalk.Length=walk.Length;
            existingWalk.WalkDifficultyId=walk.WalkDifficultyId;
            existingWalk.RegionId=walk.RegionId;

            await _nZWalksDbContext.SaveChangesAsync();

            return existingWalk;
        }

        public async Task<Walk> DeleteWalkAsync(Guid id)
        {
            var walk = await _nZWalksDbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if(walk == null)
            {
                return null;
            }

            _nZWalksDbContext.Walks.Remove(walk);
            await _nZWalksDbContext.SaveChangesAsync();

            return walk;
        }
    }
}
