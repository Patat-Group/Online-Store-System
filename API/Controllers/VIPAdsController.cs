using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs.VIPAdsDtos;
using AutoMapper;
using Core.Entities;
using Interfaces.Core;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/vipads")]
    public class VIPAdsController : ControllerBase
    {
        private readonly IGenericRepository<VIPAd, int> _vipRepo;

        private readonly IMapper _mapper;

        public VIPAdsController(IGenericRepository<VIPAd, int> vipRepo, IMapper mapper)
        {
            _vipRepo = vipRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IReadOnlyList<VIPAdsForRetutrnAdsDto>> GetAll()
        {
            var images = await _vipRepo.GetALl();
            var imagesToReturn = _mapper.Map<IReadOnlyList<VIPAd> , IReadOnlyList<VIPAdsForRetutrnAdsDto>>(images);
            return imagesToReturn;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vipAd = await _vipRepo.GetById(id);
            var imageToReturn =_mapper.Map<VIPAdsForRetutrnAdsDto>(vipAd);
            if (vipAd != null) return Ok(imageToReturn);
            return BadRequest("This Ads is not exist");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (await _vipRepo.Delete(id))
                return Ok();
            return BadRequest("Error happen when remove Ads");
        }

        [HttpPost]
        public async Task<IActionResult> AddAds(VIPAdsForCreationDto entity)
        {

            var vipAd = new VIPAd()
            {
                Name = entity.Name,
                ImageUrl = entity.ImageUrl,
                DateAdded = entity.DateAdded
            };

            if (await _vipRepo.GetById(vipAd.Id) != null)
                return BadRequest("This Ad is exist.");

            if (await _vipRepo.Add(vipAd))
                return Ok(vipAd);
            return BadRequest("Error happen when add Ads");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] VIPAdsForUpdateDto entity)
        {
            var vipAd = await _vipRepo.GetById(id);
            if (vipAd == null) return BadRequest("This Ad is not exist");
            vipAd.Name = entity.Name;
            vipAd.ImageUrl = entity.ImageUrl;
            if (await _vipRepo.Update(vipAd))
                return Ok();
            return BadRequest("Error happen when update Ads");
        }
    }
}