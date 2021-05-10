using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Data;

namespace Services.Seeds
{
    public class ProductSeed
    {
        public static async Task SeedProductAsync(StoreContext context)
        {
            if (!context.Products.Any())
            {
                var productData = await File.ReadAllTextAsync("../Services/Seeds/Data/Product.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productData);
                foreach (var product in products)
                {
                    var productForAdd = new Product()
                    {
                        Name = product.Name,
                        ShortDescription = product.ShortDescription,
                        LongDescription = product.LongDescription,
                        Price = product.Price,
                        DateAdded = DateTime.UtcNow,
                        CategoryId = product.CategoryId,
                        UserId = product.UserId,
                    };

                    productForAdd.Category = await context.Categories
                        .FirstOrDefaultAsync(x => x.Id == productForAdd.CategoryId);
                    productForAdd.User = await context.Users
                        .FirstOrDefaultAsync(x => x.Id == productForAdd.UserId);

                    await context.AddAsync(productForAdd);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}