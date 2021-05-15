using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Enums;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Services.Data
{
    public class RatingRepository :IRatingRepository
    {
        private readonly StoreContext _context;

        public RatingRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Rating>> GetAllRatings()
        {
            var ratings = await _context.Ratings.ToListAsync();
            return ratings;
        }

        public async Task<bool> GiveRate(Rating rate)
        {
            var userDestinationTotalRating = await _context.UsersRated.SingleOrDefaultAsync(x =>
                x.UserId == rate.UserDestinationRateId);
            switch (rate.Star)
            {
                case RatingStar.One:
                    userDestinationTotalRating.OneStarCount++;
                    break;
                case RatingStar.Two:
                    userDestinationTotalRating.TwoStarCount++;
                    break;
                case RatingStar.Three:
                    userDestinationTotalRating.ThreeStarCount++;
                    break;
                case RatingStar.Four:
                    userDestinationTotalRating.FourStarCount++;
                    break;
                case RatingStar.Five:
                    userDestinationTotalRating.FiveStarCount++;
                    break;
                default:
                    return false;
            }
            await _context.Ratings.AddAsync(rate);
            _context.UsersRated.Update(userDestinationTotalRating);
            var result= await SaveChanges();
            return result;
        }
        

        public async Task<int> GetMyRateToUser(string userSourceRateId, string userDestinationRateId)
        {
            var userDestinationRate = await _context.Ratings.SingleOrDefaultAsync(x =>
                x.UserSourceRateId == userSourceRateId && x.UserDestinationRateId == userDestinationRateId);
            if (userDestinationRate == null) return -1;
            return (int) userDestinationRate.Star + 1;
        }

        public async Task<bool> RemoveOldRateIfExists(string userSourceRateId, string userDestinationRateId)
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

        public async Task<UserRated> GetDetailedRate(string userId)
        {
            var detailedRate=await _context.UsersRated.SingleOrDefaultAsync(x =>
                x.UserId == userId);
            return detailedRate;
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