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
    public class UserRepository : IUserRepository
    {
        private readonly StoreContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public UserRepository(StoreContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IReadOnlyList<User>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        public async Task<User> GetUserById(string id)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == id);
            return user;
        }

        public async Task<User> GetUserByUsername(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return user;
        }

        public async Task<User> GetUserByUserClaims(ClaimsPrincipal userClaimsPrincipal)
        {
            var username = userClaimsPrincipal?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
            var user = await _userManager.FindByNameAsync(username);
            return user;
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user;
        }

        public async Task<bool> Login(User user, string password)
        {
            var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
            return result.Succeeded;
        }
        
        public async Task<User> Register(string username, string email, string password)
        {
            var user = new User
            {
                UserName = username,
                Email = email,
                DateCreated = DateTime.Now
            };
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded) return null;
            Console.WriteLine(result.Succeeded);
            var newUserRate = new UserRated
            {
                UserId = (await GetUserByUsername(username)).Id
            };
            await _context.UsersRated.AddAsync(newUserRate);
            var finalResult = await SaveChanges();
            return finalResult ? user : null;
        }

        public async Task<bool> ValidatePassword(string password)
        {
            var validators = _userManager.PasswordValidators;
            foreach(var validator in validators)
            {
                var result = await validator.ValidateAsync(_userManager, null,password);

                if (!result.Succeeded)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> DeleteByUsername(string username)
        {
            var user = await GetUserByUsername(username);
            if (user == null)
                return false;
            if (await CleanUserData(user.Id) == false)
                return false;
            await SaveChanges();
            var result = await _userManager.DeleteAsync(user);
            await SaveChanges();
            return result.Succeeded;
        }

        public async Task<bool> ChangePassword(User user, string currentPassword,string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            return result.Succeeded;
        }

        private async Task<bool> CleanUserData(string userId)
        {
            var userTotalRating = await _context.UsersRated.SingleOrDefaultAsync(x =>
                x.UserId == userId);
            Console.WriteLine(userTotalRating);
            if (userTotalRating != null)
            {
                var usersRatingsList =
                    await _context.Ratings.Where(x => x.UserSourceRateId == userId || x.UserDestinationRateId == userId)
                        .ToListAsync();
                Console.WriteLine(usersRatingsList.Count);
                foreach (var rate in usersRatingsList)
                {
                    if ((await RemoveOldRateIfExists(rate.UserSourceRateId, rate.UserDestinationRateId) == false))
                        return false;
                }

                userTotalRating = await _context.UsersRated.SingleOrDefaultAsync(x =>
                    x.UserId == userId);
                _context.UsersRated.Remove(userTotalRating);
                var result = await SaveChanges();
                if (result == false)
                    return false;
            }

            var userReportsList = await _context.Reports
                .Where(x => x.UserSourceReportId == userId || x.UserDestinationReportId == userId).ToListAsync();
            Console.WriteLine(userReportsList.Count);
            if (userReportsList.Count <= 0) return true;
            foreach (var report in userReportsList)
            {
                if (await DeleteReport(report.Id) == false)
                    return false;
            }

            return await SaveChanges();
        }

        private async Task<bool> DeleteReport(int id)
        {
            var reportToDelete = await _context.Reports.SingleOrDefaultAsync(x => x.Id == id);
            if (reportToDelete == null) return false;
            _context.Reports.Remove(reportToDelete);
            return await SaveChanges();
        }

        private async Task<bool> RemoveOldRateIfExists(string userSourceRateId, string userDestinationRateId)
        {
            var userTotalRating = await _context.UsersRated.SingleOrDefaultAsync(x =>
                x.UserId == userDestinationRateId);
            var oldUserRate = await _context.Ratings.SingleOrDefaultAsync(x =>
                x.UserSourceRateId == userSourceRateId && x.UserDestinationRateId == userDestinationRateId);
            if (oldUserRate == null) return true;
            var oldUserRateValue = oldUserRate.Star;
            switch (oldUserRateValue)
            {
                case RatingStar.One:
                    userTotalRating.OneStarCount--;
                    break;
                case RatingStar.Two:
                    userTotalRating.TwoStarCount--;
                    break;
                case RatingStar.Three:
                    userTotalRating.ThreeStarCount--;
                    break;
                case RatingStar.Four:
                    userTotalRating.FourStarCount--;
                    break;
                case RatingStar.Five:
                    userTotalRating.FiveStarCount--;
                    break;
                default:
                    return false;
            }

            _context.Ratings.Remove(oldUserRate);
            _context.UsersRated.Update(userTotalRating);
            var result = await SaveChanges();
            return result;
        }

        public async Task<bool> UpdateUser(User user)
        {
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }

        private async Task<bool> SaveChanges()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}