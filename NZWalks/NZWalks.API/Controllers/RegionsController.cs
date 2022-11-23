using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IResionRepository _resionRepository;
        private readonly IMapper _mapper;

        public RegionsController(IResionRepository resionRepository,IMapper mapper)
        {
            _resionRepository = resionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegions()
        {
            var regions = await _resionRepository.GetAllAsync();

            
            //return DTO regions
            /*
            var regionsDTO = new List<Models.DTO.Region>();

            regions.ToList().ForEach(region =>
            {
                var regionDTO = new Models.DTO.Region
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    Area = region.Area,
                    Lat = region.Lat,
                    Long = region.Long,
                    Population = region.Population

                };

                regionsDTO.Add(regionDTO);

            });
            */

            var regionsDTO = _mapper.Map<List<Models.DTO.Region>>(regions);
            return Ok(regionsDTO);

        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionByIdAsync")]
        public async Task<IActionResult> GetRegionByIdAsync([FromRoute]Guid id)
        {
            var region=await _resionRepository.GetAsyncById(id);

            if (region == null)
            {
                return NotFound($"The Region id = {id}, is not found");
            }

            var regionDTO = _mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync([FromBody]AddRegionRequest addRegionRequest)
        {
            //Convert Request to Domain Model

            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };

            // Pass Details to Repository

            var regionResult = await _resionRepository.AddRegionAsync(region);


            //Convert back to DTO

            var regionDTO = new Models.DTO.Region
            {
                Id = regionResult.Id,
                Code = regionResult.Code,
                Area = regionResult.Area,
                Lat = regionResult.Lat,
                Long = regionResult.Long,
                Name = regionResult.Name,
                Population = regionResult.Population
            };

            return CreatedAtAction(nameof(GetRegionByIdAsync), new { id = regionDTO.Id },regionDTO);
              
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
        {
            //Get region from database
            var region = await _resionRepository.GetAsyncById(id);

            //If nulll NotFound

            if(region == null)
            {
                return NotFound($"The Region Id = {id} is not found");
            }

            // Convert Response back to DTO 

            var regionDTO = _mapper.Map<Models.DTO.Region>(region);

            await _resionRepository.DeleteRegionAsync(regionDTO.Id);

            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute]Guid id,[FromBody]UpdateRegionRequest updateRegionRequest)
        {
            //Convert DTO to Domain
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Long = updateRegionRequest.Long,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population
            };

            //Update Region using Repository

            var deletedRegion = await _resionRepository.UpdateRegionAsync(id, region);

            //If null then not found

            if(deletedRegion == null)
            {
                return null;
            }

            //Convert domain back to dto

            var regionDTO = _mapper.Map<Models.DTO.Region>(deletedRegion);

            //Return Ok response

            return Ok(regionDTO);
        }
        
    }
}
