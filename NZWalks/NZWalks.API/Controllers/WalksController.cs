using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class WalksController : ControllerBase
    {
        private readonly IWalksRepository _walksRepository;
        public readonly IMapper _mapper;

        public WalksController(IWalksRepository walksRepository, IMapper mapper)
        {
            _walksRepository = walksRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            var walks = await _walksRepository.AllWalksAsync();

            //Return DTO WALKS

            //var walksDTO = new List<Models.DTO.Walk>();

            //walks.ToList().ForEach(walk =>
            //{
            //    var walkDTO = new Models.DTO.Walk()
            //    {
            //        Id = walk.Id,
            //        Name = walk.Name,
            //        Length = walk.Length,
            //        RegionId = walk.RegionId,
            //        WalkDifficultyId = walk.WalkDifficultyId
            //    };
            //    walksDTO.Add(walkDTO);
            //});

            var walksDTO = _mapper.Map<List<Models.DTO.Walk>>(walks);

            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync([FromRoute]Guid id)
        {
            var walk = await _walksRepository.GetWalkByIdAsync(id);

            if(walk == null)
            {
                return NotFound($"The walk id = {id} is not found");
            }

            var walkDTO= _mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody]AddWalkRequest addWalkRequest)
        {
            var walk = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            var addwWalk = await _walksRepository.AddWalkAsyn(walk);

            var walkDTO = new Models.DTO.Walk()
            {
                Id = addwWalk.Id,
                Name = addwWalk.Name,
                Length = addwWalk.Length,
                RegionId = addwWalk.RegionId,
                WalkDifficultyId = addwWalk.WalkDifficultyId
            };

            return CreatedAtAction(nameof(GetWalkAsync),new {id= walkDTO.Id},walkDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute]Guid id, [FromBody]UpdateWalkRequest updateWalkRequest)
        {
            var walk = new Models.Domain.Walk()
            {
                Id = id,
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId

            };

            var updatewalk=await _walksRepository.UpdateWalkAsync(id, walk);

            if(updatewalk == null)
            {
                return null;
            }

            var walkDTO= _mapper.Map<Models.DTO.Walk>(updatewalk);
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync([FromRoute]Guid id)
        {
            var walk = await _walksRepository.GetWalkByIdAsync(id);

            if(walk == null)
            {
                return NotFound($"The id = {id} is not found");
            }

            var walkDTO = _mapper.Map<Models.DTO.Walk>(walk);


            await _walksRepository.DeleteWalkAsync(walkDTO.Id);

            return CreatedAtAction(nameof(GetWalkAsync),new {Id=walkDTO.Id},walkDTO);
        }
    }
}
