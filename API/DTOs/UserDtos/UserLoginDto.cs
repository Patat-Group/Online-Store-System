using System.ComponentModel.DataAnnotations;
namespace API.DTOs.UserDtos
{
    public class UserLoginDto
    {
        [Required]
        public string LoginString { get; set; }
        [Required]
        public string Password { get; set; }
    }
}