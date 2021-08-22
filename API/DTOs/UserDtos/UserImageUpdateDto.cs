using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
namespace API.DTOs.UserDtos
{
    public class UserImageUpdateDto
    {
        [Required]
        public IFormFile File { get; set; }
    }
}