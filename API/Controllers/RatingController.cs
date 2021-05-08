using System.Threading.Tasks;
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

        [HttpPost("give_rate")]
        [Authorize]
        public async Task<ActionResult> GiveRate([FromQuery] string username, int value)
        {
            var userSourceRate = await _userRepo.GetByUserClaims(HttpContext.User);
            if (userSourceRate == null) return BadRequest("Bad Token");
            var userDestinationRate = await _userRepo.GetByUsername(username);
            if (userDestinationRate == null) return BadRequest("User Not Found");
            if (userSourceRate.Id == userDestinationRate.Id) return BadRequest("You Can't Rate Yourself");
            var resultRemoveOldRate =
                await _ratingRepo.RemoveOldRateIfExists(userSourceRate.Id, userDestinationRate.Id);
            if (resultRemoveOldRate == false) return BadRequest("Error Occured While Removing Old Rate");
            var newUserRate = new Rating
            {
                UserSourceRateId = userSourceRate.Id,
                UserDestinationRateId = userDestinationRate.Id,
                Star = (RatingStar) (value - 1),
            };
            var result = await _ratingRepo.GiveRate(newUserRate);
            if (result)
                return Ok("Setting new rate done successfully");
            return BadRequest("Error Occured While Setting New Rate");
        }
        [HttpPost("remove_rate")]
        [Authorize]
        public async Task<ActionResult>RemoveRate([FromQuery] string username)
        {
            var userFromRate = await _userRepo.GetByUserClaims(HttpContext.User);
            if (userFromRate == null) return BadRequest("Bad Token");
            var userToRate = await _userRepo.GetByUsername(username);
            if (userToRate == null) return BadRequest("User Not Found");
            if (userFromRate.Id == userToRate.Id) return BadRequest("You Can't Remove Your Own Rate");
            var result = await _ratingRepo.RemoveOldRateIfExists(userFromRate.Id, userToRate.Id);
            if (result)
                return Ok("Removing rate done successfully");
            return BadRequest("Error Occured While Removing Rate");
        }
        [HttpPost("get_detailed_rate")]
        [Authorize]
        public async Task<ActionResult<UserDetailedRateDto>>GetDetailedRate()
        {
            var user = await _userRepo.GetByUserClaims(HttpContext.User);
            if (user == null) return BadRequest("Bad Token");
            var result = await _ratingRepo.GetDetailedRate(user.Id);
            var detailedRate=_mapper.Map<UserRated,UserDetailedRateDto>(result);
            return detailedRate;
        }
        [HttpPost("get_rate")]
        [Authorize]
        public async Task<ActionResult<double>>GetRateByClaims()
        {
            var user = await _userRepo.GetByUserClaims(HttpContext.User);
            if (user == null) return BadRequest("Bad Token");
            var rate= await GetRate(user);
            if (rate < 0)
                return NotFound("User Not Rated Yet");
            return Ok(rate);
        }
        [HttpPost("get_my_rate_to_user")]
        [Authorize]
        public async Task<ActionResult>GetMyRateToUser([FromQuery] string username)
        {
            var userFromRate = await _userRepo.GetByUserClaims(HttpContext.User);
            if (userFromRate == null) return BadRequest("Bad Token");
            var userToRate = await _userRepo.GetByUsername(username);
            if (userToRate == null) return BadRequest("User Not Found");
            if (userFromRate.Id == userToRate.Id) return BadRequest("You Can't Rate Yourself!!");
            var result = await _ratingRepo.GetMyRateToUser(userFromRate.Id, userToRate.Id);
            return result>0 ? Ok(result) : StatusCode(
                404,"User Not Rated Yet");
        }
        [HttpPost("get_rate_by_username")]
        public async Task<ActionResult<double>>GetRateByUsername([FromQuery]string username)
        {
            var user = await _userRepo.GetByUsername(username);
            if (user == null) return BadRequest("User Not Found");
            var rate= await GetRate(user);
            if (rate < 0)
                return NotFound("User Not Rated Yet");
            return Ok(rate);
        }
        [HttpPost("get_detailed_rate_by_username")]
        public async Task<ActionResult<UserDetailedRateDto>>GetDetailedRateByUsername([FromQuery]string username)
        {
            var user = await _userRepo.GetByUsername(username);
            if (user == null) return BadRequest("User Not Found");
            var result = await _ratingRepo.GetDetailedRate(user.Id);
            var detailedRate=_mapper.Map<UserRated,UserDetailedRateDto>(result);
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
            if (ratersCount>0)
                 rate = (double) rateSum / ratersCount;
            else 
                 rate = -1.0;
            return rate;
        }
    }
}