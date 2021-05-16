using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Services.Data;
using Services.Seeds.DTOs;

namespace Services.Seeds
{
    public class UserSeed
    {
        public static async Task SeedUserAsync(UserManager<User> userManager, StoreContext context)
        {
            if (!userManager.Users.Any())
            {
                var usersData = await File.ReadAllTextAsync("../Services/Seeds/Data/Users.json");
                var users = JsonSerializer.Deserialize<List<UserSeedDataDto>>(usersData);
                foreach (var userData in users)
                {
                    Console.WriteLine(userData.Username);
                    var user = new User
                    {
                        UserName = userData.Username,
                        Email = userData.Email,
                        EmailConfirmed = bool.Parse(userData.EmailConfirmed),
                        Description = userData.Description,
                        PhoneNumber = userData.PhoneNumber,
                        FirstName = userData.FirstName,
                        LastName = userData.LastName,
                        Gender = userData.Gender,
                        Address = userData.Address,
                        PictureUrl = userData.PictureUrl,
                        FacebookUrl = userData.FacebookUrl,
                        WhatsappUrl = userData.WhatsappUrl,
                        TelegramUrl = userData.TelegramUrl,
                        DateCreated = DateTime.Now,
                    };
                    await userManager.CreateAsync(user, userData.Password);
                    var newUserRate = new UserRated
                    {
                        UserId = (await userManager.FindByNameAsync(userData.Username)).Id
                    };
                    await context.UsersRated.AddAsync(newUserRate);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}