using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Enums;
using Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Services.Data
{
    public class UserRepository  :IUserRepository
    {
        private readonly StoreContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManger;

        public UserRepository(StoreContext context,UserManager<User> userManager,SignInManager<User> signInManager)
        {
            _context = context;
            _userManger = userManager;
            _signInManager = signInManager;
        }

        public async Task<IReadOnlyList<User>> GetAll()
        { 
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u=>u.Id==id);
            return user;
        }

        public async Task<User> GetByUsername(string username)
        {
            var user = await _userManger.FindByNameAsync(username);
            return user;
        }

        public async Task<User> GetByUserClaims(ClaimsPrincipal userClaimsPrincipal)
        {
            var username =userClaimsPrincipal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            var user = await _userManger.FindByNameAsync(username);
            return user;
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await _userManger.FindByEmailAsync(email);
            return user;
        }

        public async Task<bool> Login(User user,string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user,password,false);
            return result.Succeeded;
        }

        public async Task<User> Register(string username,string email,string password)
        {
            var user = new User
            {
                UserName = username,
                Email = email,
                DateCreated = DateTime.Now
            };
            var result = await _userManger.CreateAsync(user, password);
            if (!result.Succeeded) return null;
            var newUserRate = new UserRated
            {
                UserId = (await GetById(username)).Id,
                OneStarCount = 0,
                TwoStarCount = 0,
                ThreeStarCount = 0,
                FourStarCount = 0,
                FiveStarCount = 0,
            };
            await _context.UsersRated.AddAsync(newUserRate);
            var finalResult = await SaveChanges();
            return finalResult ? user : null;
        }

        public async Task<bool> DeleteByUsername(string username)
        {
            var user = await GetByUsername(username);
            if (user == null)
                return false;
            var result = await _userManger.DeleteAsync(user);
            var userTotalRating = await _context.UsersRated.SingleOrDefaultAsync(x =>
                x.UserId == user.Id);
            if (userTotalRating == null) return result.Succeeded;
            _context.UsersRated.Remove(userTotalRating);
            await SaveChanges();
            return result.Succeeded;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var result = await _userManger.UpdateAsync(user);
            return result.Succeeded;
        }

        private async Task<bool> SaveChanges()
        {
            var result = await _context.SaveChangesAsync();
            return result > 0 ? true : false;
        }
    }
}