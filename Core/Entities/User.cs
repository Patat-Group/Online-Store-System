using System;
using System.Collections.Generic;

namespace Core.Entities
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string PictureUrl { get; set; }
        public string FacebookUrl { get; set; }
        public string WhatsappUrl { get; set; }
        public string TelegramUrl { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime LastSeen { get; set; }
        public bool RememberMe { get; set; }
        public ICollection<Rating> UsersGetRating { get; set; }
        public ICollection<Rating> UsersSetRating { get; set; }
        public ICollection<Report> UsersGetReport { get; set; }
        public ICollection<Report> UsersSetReport { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}