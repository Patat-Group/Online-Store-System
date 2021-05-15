using System.ComponentModel.DataAnnotations;

namespace API.DTOs.UserDtos
{
    public class UserChangePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
}