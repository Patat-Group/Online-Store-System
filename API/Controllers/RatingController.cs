using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.DTOs.RatingDtos;
using API.DTOs.UserDtos;
using AutoMapper;
using Core.Entities;
using Core.Entities.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/rating")]
    public class RatingController : ControllerBase

    {
        private readonly IUserRepository _userRepo;
        private readonly IRatingRepository _ratingRepo;
        private readonly IMapper _mapper;

        public RatingController(IUserRepository userRepo, IRatingRepository ratingRepository, IMapper mapper)
        {
            _userRepo = userRepo;
            _ratingRepo = ratingRepository;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyList<RatingToReturnDto>>> GetAllRatings()
        {
            var ratings = await _ratingRepo.GetAllRatings();
            var ratingsToReturn = _mapper.Map<IReadOnlyList<Rating>, IReadOnlyList<RatingToReturnDto>>(ratings);
            return Ok(ratingsToReturn);
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> GiveRate([FromBody] RatingForCreationDto ratingForCreationDto)
        {
            var userSourceRate = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (userSourceRate == null) return Unauthorized("User is Unauthorized");

            var userDestinationRate = await _userRepo.GetUserByUsername(ratingForCreationDto.Username);
            if (userDestinationRate == null) return BadRequest("User Not Found");
            if (userSourceRate.Id == userDestinationRate.Id) return BadRequest("You Can't Rate Yourself");
            var resultRemoveOldRate =
                await _ratingRepo.RemoveOldRateIfExists(userSourceRate.Id, userDestinationRate.Id);
            if (resultRemoveOldRate == false) return BadRequest("Error Occured While Removing Old Rate");
            var newUserRate = new Rating
            {
                UserSourceRateId = userSourceRate.Id,
                UserDestinationRateId = userDestinationRate.Id,
                Star = (RatingStar)(ratingForCreationDto.Value - 1),
            };
            var result = await _ratingRepo.GiveRate(newUserRate);
            if (result)
                return Ok("Setting new rate done successfully");
            throw new Exception("Error Occured While Setting New Rate, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpDelete("myrate/{username}")]
        [Authorize]
        public async Task<ActionResult> RemoveRate(string username)
        {
            var userFromRate = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (userFromRate == null) return Unauthorized("User is Unauthorized");

            var userToRate = await _userRepo.GetUserByUsername(username);
            if (userToRate == null) return BadRequest("User Not Found");
            if (userFromRate.Id == userToRate.Id) return BadRequest("You Can't Remove Your Own Rate");
            var result = await _ratingRepo.RemoveOldRateIfExists(userFromRate.Id, userToRate.Id);
            if (result)
                return Ok("Removing rate done successfully");
            throw new Exception("Error Occured While Removing Rate, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpGet("myrate/details")]
        [Authorize]
        public async Task<ActionResult<UserDetailedRateDto>> GetDetailedRate()
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var result = await _ratingRepo.GetDetailedRate(user.Id);
            var detailedRate = _mapper.Map<UserRated, UserDetailedRateDto>(result);
            return detailedRate;
            throw new Exception("Error Occured While Get Detailed Rate, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpGet("myrate")]
        [Authorize]
        public async Task<ActionResult<double>> GetRateByClaims()
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");

            var rate = await GetRate(user);
            if (rate < 0)
                return NotFound("User Not Rated Yet");
            return Ok(rate);

            throw new Exception("Error Occured While Get Rate By Claims, Ahmad Nour hate Exception ):,Exception hate Ahmad Nour ): please don't make any error, i see you *-*");
        }

        [HttpGet("myrate/{username}")]
        [Authorize]
        public async Task<ActionResult> GetMyRateToUser(string username)
        {
            var userFromRate = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (userFromRate == null) return Unauthorized("User is Unauthorized");

            var userToRate = await _userRepo.GetUserByUsername(username);
            if (userToRate == null) return BadRequest("User Not Found");
            if (userFromRate.Id == userToRate.Id) return BadRequest("You Can't Rate Yourself!!");
            var result = await _ratingRepo.GetMyRateToUser(userFromRate.Id, userToRate.Id);
            return result > 0 ? Ok(result) : StatusCode(
                404, "User Not Rated Yet");
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<double>> GetRateByUsername(string username)
        {
            var user = await _userRepo.GetUserByUsername(username);
            if (user == null) return BadRequest("User Not Found");
            var rate = await GetRate(user);
            if (rate < 0)
                return NotFound("User Not Rated Yet");
            return Ok(rate);
        }

        [HttpGet("details/{username}")]
        public async Task<ActionResult<UserDetailedRateDto>> GetDetailedRateByUsername(string username)
        {
            var user = await _userRepo.GetUserByUsername(username);
            if (user == null) return BadRequest("User Not Found");
            var result = await _ratingRepo.GetDetailedRate(user.Id);
            var detailedRate = _mapper.Map<UserRated, UserDetailedRateDto>(result);
            return detailedRate;
        }

        private async Task<double> GetRate(User user)
        {
            var result = await _ratingRepo.GetDetailedRate(user.Id);
            var detailedRate = _mapper.Map<UserRated, UserDetailedRateDto>(result);
            var rateSum = 5 * detailedRate.FiveStarCount +
                          4 * detailedRate.FourStarCount +
                          3 * detailedRate.ThreeStarCount +
                          2 * detailedRate.TwoStarCount +
                          1 * detailedRate.OneStarCount;
            var ratersCount = detailedRate.FiveStarCount +
                              detailedRate.FourStarCount +
                              detailedRate.ThreeStarCount +
                              detailedRate.TwoStarCount +
                              detailedRate.OneStarCount;
            var rate = 0.0;
            if (ratersCount > 0)
                rate = (double)rateSum / ratersCount;
            else
                rate = -1.0;
            return rate;
        }
    }
}