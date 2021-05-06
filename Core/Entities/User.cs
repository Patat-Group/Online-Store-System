using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class User :IdentityUser
    {
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
        public DateTime LastSeen { get; set; }
        public bool RememberMe { get; set; }
        
        public ICollection<Rating> UsersSourceRating { get; set; }
        public ICollection<Rating> UsersDestinationRating { get; set; }
        public ICollection<Report> UsersSourceReport { get; set; }
        public ICollection<Report> UsersDestinationReport { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}