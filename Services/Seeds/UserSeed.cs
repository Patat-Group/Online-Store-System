using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Services.Data;

namespace Services.Seeds
{
    public class UserSeed
    {
        public static async Task SeedUserAsync(UserManager<User> userManager,StoreContext context)
        {
            if (!userManager.Users.Any())
            {
                var usersData = File.ReadAllText("../Services/Seeds/Data/Users.json");
                var users = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(usersData);
                foreach (var userData in users)
                {
                    var user = new User
                    {
                        UserName = userData["UserName"],
                        Email = userData["Email"],
                        EmailConfirmed = bool.Parse(userData["EmailConfirmed"]),
                        Description = userData["Description"],
                        PhoneNumber = userData["PhoneNumber"],
                        FirstName = userData["FirstName"],
                        LastName = userData["LastName"],
                        Gender = userData["Gender"],
                        Address = userData["Address"],
                        PictureUrl = userData["PictureUrl"],
                        FacebookUrl = userData["FacebookUrl"],
                        WhatsappUrl = userData["WhatsappUrl"],
                        TelegramUrl = userData["TelegramUrl"],
                        DateCreated = System.DateTime.Now,
                    };
                    await userManager.CreateAsync(user, password: userData["Password"]);
                    var newUserRate = new UserRated
                    {
                        UserId = (await userManager.FindByNameAsync(userData["UserName"])).Id,
                        OneStarCount = 0,
                        TwoStarCount = 0,
                        ThreeStarCount = 0,
                        FourStarCount = 0,
                        FiveStarCount = 0,
                    };
                    await context.UsersRated.AddAsync(newUserRate);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}