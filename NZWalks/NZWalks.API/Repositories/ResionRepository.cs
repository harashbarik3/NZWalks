using Microsoft.AspNetCore.Mvc;
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

        public async Task<Region> GetAsyncById(Guid id)
        {
            var region = await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x=>x.Id== id);

            return region;
        }

        public async Task<Region> AddRegionAsync(Region region)
        {
            region.Id= Guid.NewGuid();
            await _nZWalksDbContext.AddAsync(region);
            await _nZWalksDbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region> DeleteRegionAsync(Guid id)
        {
            var region=await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x=>x.Id== id); 
            
            if(region == null)
            {
                return null;
            }

            //Delete the region

            _nZWalksDbContext.Remove(region);
            await _nZWalksDbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region> UpdateRegionAsync(Guid id, Region region)
        {
            var existingRegion= await _nZWalksDbContext.Regions.FirstOrDefaultAsync(x=>x.Id== id);

            if(region == null)
            {
                return null;
            }

            existingRegion.Code= region.Code;
            existingRegion.Name= region.Name;
            existingRegion.Area= region.Area;
            existingRegion.Lat= region.Lat;
            existingRegion.Long= region.Long;
            existingRegion.Population= region.Population;

            await _nZWalksDbContext.SaveChangesAsync();

            return existingRegion;
        }
    }
}
