using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface IUserRepository
    {
        public  Task<IReadOnlyList<User>> GetAllUsers();
        public  Task<User> GetUserById(string id);
        public  Task<User> GetUserByUsername(string username);
        public  Task<User> GetUserByUserClaims(ClaimsPrincipal userClaimsPrincipal);
        public  Task<User> GetUserByEmail(string email);
        public  Task<bool> Login(User user, string password);
        public  Task<User> Register(string username, string email, string password);
        public  Task<bool> ValidatePassword(string password);
        public  Task<bool> DeleteByUsername(string username);
        public  Task<bool> ChangePassword(User user,string currentPassword,string newPassword);
        public  Task<bool> UpdateUser(User user);
    }
}