﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
using Microsoft.Extensions.Hosting.Internal;
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
        public async Task<ActionResult<IReadOnlyList<UserToReturnDto>>> GetAllUsers()
        {
            var users = await _userRepo.GetAllUsers();
            var usersToReturn = _mapper.Map<IReadOnlyList<User>, IReadOnlyList<UserToReturnDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpDelete("All")]
        public async Task<ActionResult> DeleteAllUsers()
        {
            var users = await _userRepo.GetAllUsers();
            foreach (var user in users)
                await _userRepo.DeleteByUsername(user.UserName);
            return Ok("Delete All users Succeeded");
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserToReturnDto>> GetCurrentUser()
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return BadRequest("Bad User Token");
            var userForReturn = _mapper.Map<User, UserToReturnDto>(user);
            return userForReturn;
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<UserToReturnDto>> GetUserDetails(string username)
        {
            var user = await _userRepo.GetUserByUsername(username);
            if (user == null) return BadRequest("User Not Found");
            var userForReturn = _mapper.Map<User, UserToReturnDto>(user);
            return userForReturn;
        }

        [HttpPut]
        [Authorize]
        public async Task<ActionResult<bool>> UpdateUserInformation(
            [FromBody] UserUpdateInformationDto userUpdateInformationDto)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return BadRequest("Bad User Token");
            _mapper.Map(userUpdateInformationDto, user);
            var result = await _userRepo.UpdateUser(user);
            if (result)
                return Ok("Update Succeeded");
            return BadRequest("Error Happen When Update");
        }

        [HttpPut("password")]
        [Authorize]
        public async Task<ActionResult<bool>> ChangePassword(
            [FromBody] UserChangePasswordDto userChangePasswordDto)
        {
            var user = await _userRepo.GetUserByUserClaims(HttpContext.User);
            if (user == null) return BadRequest("Bad User Token");
            var checkIfOldPasswordCorrect = await _userRepo.Login(user, userChangePasswordDto.CurrentPassword);
            if (checkIfOldPasswordCorrect == false) return BadRequest("Current Password Is Wrong");
            var checkIfNewPasswordValid = await _userRepo.ValidatePassword(userChangePasswordDto.NewPassword);
            if (checkIfNewPasswordValid == false)
                return BadRequest(
                    "BadPassword\nPassword Must Have at least one:{digit,Uppercase letter,Lowercase letter} and length 6 or more}");
            var result = await _userRepo.ChangePassword(user, userChangePasswordDto.CurrentPassword,
                userChangePasswordDto.NewPassword);
            if (result)
                return Ok("Password Update Succeeded");
            return BadRequest("Error Happen When Updating The Password");
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
        public async Task<ActionResult> DeleteUser(string username)
        {
            if ((await CheckIfUsernameExist(username)).Value == false)
                return BadRequest("User Not Found");
            var result = await _userRepo.DeleteByUsername(username);
            if (result)
                return Ok("user deleting Succeeded");
            return BadRequest("Error Happen When Deleting User");
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

            return BadRequest("Error In Creating User");
        }
    }
}