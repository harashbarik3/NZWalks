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
        private readonly IResionRepository _resionRepository;
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;

        public WalksController(IWalksRepository walksRepository, IMapper mapper,IResionRepository resionRepository,IWalkDifficultyRepository walkDifficultyRepository)
        {
            _walksRepository = walksRepository;
            _mapper = mapper;
            _resionRepository = resionRepository;
            _walkDifficultyRepository = walkDifficultyRepository;
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
            //Validate the incomming request

            if(!await ValidateAddWalkAsync(addWalkRequest))
            {
                return BadRequest(ModelState);
            }


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
            //Validate incomming request

            if(!(await ValidateUpdateWalkAsync(updateWalkRequest)))
            {
                return BadRequest(ModelState);
            }

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

        #region Private methods

        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            //if (addWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest), $"Add Walk Data is required");
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(addWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Name), $"{nameof(addWalkRequest.Name)} is required");
            //}

            //if(addWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(addWalkRequest.Length), $"{nameof(addWalkRequest.Length)} Should be greater than zeero");

            //}

            var resion = await _resionRepository.GetAsyncById(addWalkRequest.RegionId);

            if(resion == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), $"{nameof(addWalkRequest.RegionId)} is Invalid");

            }

            var walkDifficulty = await _walkDifficultyRepository.GetWalkDifficultyAsync(addWalkRequest.WalkDifficultyId);

            if(walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.WalkDifficultyId), $"{nameof(addWalkRequest.WalkDifficultyId)} is Invalid");

            }

            if(ModelState.ErrorCount> 0)
            {
                return false;
            }


            return true;

        }

        private async Task<bool> ValidateUpdateWalkAsync(Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //if (updateWalkRequest == null)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest), $"Add Walk Data is required");
            //    return false;
            //}

            //if (string.IsNullOrWhiteSpace(updateWalkRequest.Name))
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Name), $"{nameof(updateWalkRequest.Name)} is required");
            //}

            //if (updateWalkRequest.Length <= 0)
            //{
            //    ModelState.AddModelError(nameof(updateWalkRequest.Length), $"{nameof(updateWalkRequest.Length)} Should be greater than zeero");

            //}

            var resion = await _resionRepository.GetAsyncById(updateWalkRequest.RegionId);

            if (resion == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.RegionId), $"{nameof(updateWalkRequest.RegionId)} is Invalid");

            }

            var walkDifficulty = await _walkDifficultyRepository.GetWalkDifficultyAsync(updateWalkRequest.WalkDifficultyId);

            if (walkDifficulty == null)
            {
                ModelState.AddModelError(nameof(updateWalkRequest.WalkDifficultyId), $"{nameof(updateWalkRequest.WalkDifficultyId)} is Invalid");

            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }


            return true;

        }

        #endregion
    }
}
