using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class User :IdentityUser
    {
        public string FirstName { get; set; } = "DefaultFirstName";
        public string LastName { get; set; } = "DefaultLastName";
        public string Description { get; set; }  = "About Me";
        public string Address { get; set; }  = "";
        public string Gender { get; set; } = "Male";
        public string PictureUrl { get; set; } = "/images/defaultProfilePic.jpg";
        public string FacebookUrl { get; set; } = "https://www.facebook.com/";
        public string WhatsappUrl { get; set; } = "https://www.whatsapp.com/";
        public string TelegramUrl { get; set; } = "https://t.me/yaser01";
        public DateTime DateCreated { get; set; }
        public DateTime LastSeen { get; set; } 
        public bool RememberMe { get; set; }
        
        public ICollection<Rating> UsersSourceRating { get; set; }
        public ICollection<Rating> UsersDestinationRating { get; set; }
        public ICollection<Report> UsersSourceReport { get; set; }
        public ICollection<Report> UsersDestinationReport { get; set; }
        public ICollection<Product> Products { get; set; }
        public ICollection<FavoriteProduct> FavoriteProducts { get; set; }
    }
}