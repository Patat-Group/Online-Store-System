using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.DTOs.UserDtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Services.Data;

namespace API.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserController(IUserRepository userRepo, IMapper mapper, ITokenService tokenService)
        {
            _userRepo = userRepo;
            _mapper = mapper;
            _tokenService = tokenService;
        }
        [HttpGet("All")]
        public async Task<IReadOnlyList<UserToReturnDto>> GetAllUsers()
        {
            var users = await _userRepo.GetAll();
            var usersToReturn = _mapper.Map<IReadOnlyList<User>, IReadOnlyList<UserToReturnDto>>(users);
            return usersToReturn;
        }
        [HttpPost("deleteAll")]
        public async Task<ActionResult> DeleteAllUsers()
        {
            var users = await _userRepo.GetAll();
            foreach (var user in users)
                await _userRepo.DeleteByUsername(user.UserName);
            return Ok("Delete All users Succeeded");
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserToReturnDto>> GetCurrentUser()
        {
            var user = await _userRepo.GetByUserClaims(HttpContext.User);
            var userForReturn = _mapper.Map<User, UserToReturnDto>(user);
            return userForReturn;

        }
        [HttpGet("get_user_info")]
        public async Task<ActionResult<UserToReturnDto>> GetUserDetails([FromQuery]string username)
        {
            var user = await _userRepo.GetByUsername(username);
            if (user == null) return BadRequest("User Not Found");
            var userForReturn = _mapper.Map<User, UserToReturnDto>(user);
            return userForReturn;

        }
        [HttpPut("update_info")]
        [Authorize]
        public async Task<ActionResult<bool>> UpdateUserInformation(
            [FromQuery] UserUpdateInformationDto userUpdateInformationDto)
        {
            var user = await _userRepo.GetByUserClaims(HttpContext.User);
            _mapper.Map(userUpdateInformationDto,user);
            Console.Write(user.WhatsappUrl);
            var result = await _userRepo.UpdateUser(user);
            if (result)
                return Ok("Update Succeeded");
            return BadRequest("Error Happen When Update");

        }

        [HttpPost("email_exist")]
        public async Task<ActionResult<bool>> CheckIfEmailExist([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Empty Email String");
            var user = await _userRepo.GetByEmail(email);
            return Ok(user != null);


        }
    
        [HttpPost("username_exist")]
        public async Task<ActionResult<bool>>CheckIfUsernameExist([FromQuery] string username)
        {
            var user =await _userRepo.GetByUsername(username);
            return user != null;
        }


        [HttpPost("delete/{username}")]
        public async Task<ActionResult> DeleteUser(string username)
        {
            var result = await _userRepo.DeleteByUsername(username);
            if (result)
                return Ok("user deleting Succeeded");
            return BadRequest();
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserSessionDto>> Login([FromQuery]UserLoginDto loginDto)
        {
            var user = await _userRepo.GetByUsername(loginDto.Username) ?? await _userRepo.GetByEmail(loginDto.Email);
                if (user == null)
                    return BadRequest();
                var result = await _userRepo.Login(user, loginDto.Password);
                if (result)
                {
                    return new UserSessionDto
                    {
                        Username = user.UserName,
                        Email = user.Email,
                        Token = _tokenService.CreateToken(user)
                    };
                }
                return BadRequest();
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserSessionDto>> Register([FromQuery] UserRegisterDto registerDto)
        {
            var user = await _userRepo.Register(registerDto.Username,registerDto.Email,registerDto.Password);
            if (user!=null)
            {
                return new UserSessionDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token =  _tokenService.CreateToken(user)
                };
            }
            return BadRequest();
        }
    }

}