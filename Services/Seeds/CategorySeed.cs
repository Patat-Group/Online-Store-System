using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Services.Data;

namespace Services.Seeds
{
    public class CategorySeed
    {
        public static async Task SeedCategory(StoreContext context)
        {
            if (!context.Categories.Any())
            {
                var categoryData = await File.ReadAllTextAsync("../services/Seeds/Data/Category.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoryData);
                foreach (var category in categories)
                {
                    var categoryForAdd = new Category
                    {
                        Name = category.Name
                    };

                    await context.AddAsync(categoryForAdd);
                    await context.SaveChangesAsync();
                }
            }
        }
        
    }
}