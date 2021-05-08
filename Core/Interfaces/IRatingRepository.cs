using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IRatingRepository
    {
        public Task<bool> GiveRate(Rating rate);
        public Task<int> GetMyRateToUser(string userSourceRateId, string userDestinationRateId);
        public Task<bool> RemoveOldRateIfExists(string userSourceRateId, string userDestinationRateId);
        public Task<UserRated> GetDetailedRate(string userId);
    }
}