using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDbContext _nZWalksDbContext;
        private readonly IMapper _mapper;

        public WalkDifficultyRepository(NZWalksDbContext nZWalksDbContext,IMapper mapper) 
        {
            _nZWalksDbContext = nZWalksDbContext;
            _mapper = mapper;
        }

        public async Task<WalkDifficulty> AddWalkDifficultyAsync(WalkDifficulty walkDifficulty)
        {
             walkDifficulty.Id=Guid.NewGuid();
            _nZWalksDbContext.Add(walkDifficulty);
            await _nZWalksDbContext.SaveChangesAsync();

            return walkDifficulty; 
        }

        public async Task<WalkDifficulty> DeleteWalkDifficultyAsync(Guid id)
        {
             var walkDifficulty= await _nZWalksDbContext.WalkDifficulties.FindAsync(id);

            if(walkDifficulty == null)
            {
                return null;
            }

            _nZWalksDbContext.WalkDifficulties.Remove(walkDifficulty);
            await _nZWalksDbContext.SaveChangesAsync();

            return walkDifficulty;

        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllWalkDifficultiesAsync()
        {
            var walkDifficulties = await _nZWalksDbContext.WalkDifficulties.ToListAsync();

             return walkDifficulties;
        }

        public async Task<WalkDifficulty> GetWalkDifficultyAsync(Guid id)
        {
            var walkDifficulty = await _nZWalksDbContext.WalkDifficulties.FirstOrDefaultAsync(x=>x.Id==id);

            return walkDifficulty;
        }

        public async Task<WalkDifficulty> UpdateWalkDifficultyAsync([FromRoute]Guid id, [FromBody]WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await _nZWalksDbContext.WalkDifficulties.FindAsync(id);

            if (existingWalkDifficulty == null) {
                return null;
            }

            existingWalkDifficulty.Code=walkDifficulty.Code;
            await _nZWalksDbContext.SaveChangesAsync();
            return existingWalkDifficulty;
        }
    }
}
