using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using API.DTOs.UserDtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

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
        public async Task<ActionResult<IReadOnlyList<UserToReturnDto>>> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsers();
            var usersToReturn = _mapper.Map<IReadOnlyList<User>, IReadOnlyList<UserToReturnDto>>(users);
            return Ok(usersToReturn);
        }

        // We not have admin now for this method .
        // [HttpDelete("All")]
        // [Authorize]
        // public async Task<ActionResult> DeleteAllUsers()
        // {
        //     var users = await _userRepo.GetAllUsers();
        //     foreach (var user in users)
        //         await _userRepo.DeleteByUsername(user.UserName);
        //     return Ok("Delete All users Succeeded");
        // }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserToReturnDto>> GetCurrentUser()
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");
            var userForReturn = _mapper.Map<User, UserToReturnDto>(user);
            return userForReturn;
            throw new Exception("Error Occured when Get Current User ");
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserToReturnDto>> GetUserDetails(string username)
        {
            var user = await _userRepo.GetUserByUsername(username);
            if (user == null) return BadRequest("User Not Found");
            var userForReturn = _mapper.Map<User, UserToReturnDto>(user);
            return userForReturn;
            throw new Exception("Error Occured when Get User Details ");
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult> UpdateUserInformation(
            [FromBody] UserUpdateInformationDto userUpdateInformationDto)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized((new {message="User is Unauthorized"}));
            _mapper.Map(userUpdateInformationDto, user);
            var result = await _userRepo.UpdateUser(user);
            if (result) return Ok(new {message="Update Succeeded"});

        throw new Exception("Error Happen When Update User ");
        }

        [HttpPut("password")]
        [Authorize]
        public async Task<ActionResult> ChangePassword(
            [FromBody] UserChangePasswordDto userChangePasswordDto)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized(new {message="User is Unauthorized"});
            var checkIfOldPasswordCorrect = await _userRepo.Login(user, userChangePasswordDto.CurrentPassword);
            if (checkIfOldPasswordCorrect == false) return BadRequest("Current Password Is Wrong");
            var checkIfNewPasswordValid = await _userRepo.ValidatePassword(userChangePasswordDto.NewPassword);
            if (checkIfNewPasswordValid == false)
                return BadRequest(
                    "BadPassword\nPassword Must Have at least one:{digit,Uppercase letter,Lowercase letter} and length 6 or more}");
            var result = await _userRepo.ChangePassword(user, userChangePasswordDto.CurrentPassword,
                userChangePasswordDto.NewPassword);
            if (result)
                return Ok(new {message="Password Update Succeeded"});

            throw new Exception("Error Happen When Updating The Password ");
        }

        [HttpGet("emailExist")]
        public async Task<ActionResult<bool>> CheckIfEmailExist([FromQuery] string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return BadRequest("Empty Email String");
            var user = await _userRepo.GetUserByEmail(email);
            return Ok(user != null);
        }

        [HttpGet("usernameExist")]
        public async Task<ActionResult<bool>> CheckIfUsernameExist([FromQuery] string username)
        {
            var user = await _userRepo.GetUserByUsername(username);
            return user != null;
        }

        [HttpDelete("{username}")]
        [Authorize]
        public async Task<ActionResult> DeleteUser(string username)
        {
            if ((await CheckIfUsernameExist(username)).Value == false)
                return BadRequest("User Not Found");
            var result = await _userRepo.DeleteByUsername(username);
            if (result)
                return Ok("user deleting Succeeded");
            return BadRequest("Error Happen When Deleting User");
            throw new Exception("Error Happen When Deleting User ");

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserSessionDto>> Login([FromBody] UserLoginDto loginDto)
        {
            var user = await _userRepo.GetUserByUsername(loginDto.LoginString) ??
                       await _userRepo.GetUserByEmail(loginDto.LoginString);
            if (user == null)
                return BadRequest("Bad Login Credentials"); //User Not Found Actually
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

            return BadRequest("Bad Login Credentials");
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserSessionDto>> Register([FromBody] UserRegisterDto registerDto)
        {
            var checkIfUsernameExists = await _userRepo.GetUserByUsername(registerDto.Username);
            if (checkIfUsernameExists != null)
                return BadRequest("Username Already Exists");
            var checkIfEmailExists = await _userRepo.GetUserByEmail(registerDto.Email);
            if (checkIfEmailExists != null)
                return BadRequest("Email Already Exists");
            var checkIfPasswordValid = await _userRepo.ValidatePassword(registerDto.Password);
            if (checkIfPasswordValid == false)
                return BadRequest(
                    "BadPassword\nPassword Must Have at least one:{digit,Uppercase letter,Lowercase letter} and length 6 or more}");

            var user = await _userRepo.Register(registerDto.Username, registerDto.Email, registerDto.Password);
            if (user != null)
            {
                return new UserSessionDto
                {
                    Username = user.UserName,
                    Email = user.Email,
                    Token = _tokenService.CreateToken(user)
                };
            }

            throw new Exception("Error In Creating User ");
        }
        
        [HttpPut("photo")]
        [Authorize]
        public async Task<IActionResult> UpdateImage([FromForm] UserImageUpdateDto entity)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return Unauthorized("User is Unauthorized");
            
            var file = entity.File;
            if (file == null)
                return BadRequest("Please add photo to your product.");

            var path = Path.Combine("wwwroot/images/", user.UserName+"_profile_photo"+file.FileName);
            var stream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(stream);
            await stream.DisposeAsync();
            user.PictureUrl=path.Substring(7);
            var result = await _userRepo.UpdateUser(user);
            if (result) return Ok(new {message="Update Succeeded"});
            throw new Exception("Error happen when update user photo");
        }
    }
}