using System;

namespace API.DTOs.UserDtos
{
    public class UserToReturnDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string PictureUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string WhatsappUrl { get; set; }
        public string TelegramUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public string LastSeen { get; set; }
        public string Token { get; set; }
    }
}