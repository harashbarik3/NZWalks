using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkDifficultiesController : ControllerBase
    {
        private readonly IWalkDifficultyRepository _walkDifficultyRepository;
        private readonly IMapper _mapper;

        public WalkDifficultiesController(IWalkDifficultyRepository walkDifficultyRepository,IMapper mapper) 
        {
            _walkDifficultyRepository = walkDifficultyRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalkDifficultiesAsync()
        {

            var walkDifficulties= await _walkDifficultyRepository.GetAllWalkDifficultiesAsync();

            var walkDifficultiesDTO=_mapper.Map<List<Models.DTO.WalkDifficulty>>(walkDifficulties);

            return Ok(walkDifficultiesDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkDifficutlyById")]

        public async Task<IActionResult> GetWalkDifficutlyById([FromRoute]Guid id)
        {
            var walkDifficulty= await _walkDifficultyRepository.GetWalkDifficultyAsync(id);

            if(walkDifficulty == null)
            {
                return NotFound();
            }

            //Convert Domain to DTOs

            var walkDifficultyDTO= _mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkDifficultyAsync([FromBody]AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            //Validate incomming request

            if(!ValidateAddWalkDifficulyAsync(addWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

            //convert dto to domain

            var walkDifficultyDomain = new Models.Domain.WalkDifficulty
            { 
                Code = addWalkDifficultyRequest.Code
            };

            //call repository

            var walkDifficultyRes=await _walkDifficultyRepository.AddWalkDifficultyAsync(walkDifficultyDomain); 

            // Convert Domain to DTO

            var walkDifficultyDTO= _mapper.Map<Models.DTO.WalkDifficulty>(walkDifficultyRes);

            return CreatedAtAction(nameof(GetWalkDifficutlyById),new {Id=walkDifficultyDTO.Id},walkDifficultyDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]

        public async Task<IActionResult> UpdateWalkDifficultyAsync([FromRoute]Guid id, [FromBody]UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            //Validate Incomming request

            if (!ValidateUpdateWalkDifficulyAsync(updateWalkDifficultyRequest))
            {
                return BadRequest(ModelState);
            }

            var walkDifficulty = new Models.Domain.WalkDifficulty()
            {
                Code = updateWalkDifficultyRequest.Code,
            };

            walkDifficulty = await _walkDifficultyRepository.UpdateWalkDifficultyAsync(id, walkDifficulty);

            if(walkDifficulty == null)
            {
                return NotFound();
            }
            var walkDifficultyDTO = _mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);

            return Ok(walkDifficultyDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkDifficultyAsync([FromRoute]Guid id)
        {
            var walkDifficulty = await _walkDifficultyRepository.DeleteWalkDifficultyAsync(id);

            if(walkDifficulty == null)
            {
                return NotFound();
            }

            var walkDifficultyDTO = _mapper.Map<Models.DTO.WalkDifficulty>(walkDifficulty);
            return Ok(walkDifficultyDTO);
        }

        #region Private methods

        private bool ValidateAddWalkDifficulyAsync(Models.DTO.AddWalkDifficultyRequest addWalkDifficultyRequest)
        {
            if(addWalkDifficultyRequest==null)
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest), $"is required");
                return false;
            }

            if(string.IsNullOrWhiteSpace(addWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(addWalkDifficultyRequest),$"{nameof(addWalkDifficultyRequest)} is required");
            }

            if(ModelState.ErrorCount> 0)
            {
                return false;
            }

            return true;
        }

        private bool ValidateUpdateWalkDifficulyAsync(Models.DTO.UpdateWalkDifficultyRequest updateWalkDifficultyRequest)
        {
            if (updateWalkDifficultyRequest == null)
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest), $"is required");
                return false;
            }

            if (string.IsNullOrWhiteSpace(updateWalkDifficultyRequest.Code))
            {
                ModelState.AddModelError(nameof(updateWalkDifficultyRequest.Code), $"{nameof(updateWalkDifficultyRequest.Code)} is required");
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
