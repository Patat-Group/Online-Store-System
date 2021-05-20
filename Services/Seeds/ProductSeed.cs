using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Seeds
{
    public class ProductSeed
    {
        public static async Task SeedProductAsync(StoreContext context, UserManager<User> userManager)
        {
            int counter = 0;
            List<string> userNames = new List<string>();
            userNames.Add("Yaser01");
            userNames.Add("Yaser02");
            userNames.Add("Yaser03");

            if (!context.Products.Any())
            {
                var productData = await File.ReadAllTextAsync("../Services/Seeds/Data/Product.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                foreach (var product in products)
                {
                    var productForAdd = new Product()
                    {
                        Name = product.Name.ToLower(),
                        ShortDescription = product.ShortDescription,
                        LongDescription = product.LongDescription,
                        Price = product.Price,
                        DateAdded = DateTime.UtcNow,
                        CategoryName = product.CategoryName.ToLower(),
                    };

                    var user = await userManager.FindByNameAsync(userNames[counter]);
                    counter++;
                    productForAdd.UserId = user.Id;

                    productForAdd.User = await context.Users
                        .FirstOrDefaultAsync(x => x.Id == productForAdd.UserId);

                    await context.AddAsync(productForAdd);
                    await context.SaveChangesAsync();
                    if (counter == userNames.Count)
                        counter = 0;
                }
            }
        }
    }
}