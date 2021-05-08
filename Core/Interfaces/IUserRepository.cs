using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        public  Task<IReadOnlyList<User>> GetAll();
        public  Task<User> GetById(string id);
        public  Task<User> GetByUsername(string username);
        public  Task<User> GetByUserClaims(ClaimsPrincipal userClaimsPrincipal);
        public  Task<User> GetByEmail(string email);
        public  Task<bool> Login(User user, string password);
        public  Task<User> Register(string username, string email, string password);
        public  Task<bool> DeleteByUsername(string username);
        public  Task<bool> UpdateUser(User user);
    }
}