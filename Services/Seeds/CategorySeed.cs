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
        public static async Task SeedCategoryAsync(StoreContext context)
        {
            if (!context.Categories.Any())
            {
                var categoriesData = await File.ReadAllTextAsync(@"../Services/Seeds/Data/Category.json");
                var categories = JsonSerializer.Deserialize<List<Category>>(categoriesData);
                foreach (var category in categories)
                {
                    var categoryForAdd = new Category
                    {
                        Name = category.Name,
                        ImageUrl = category.ImageUrl
                    };
                    await context.AddAsync(categoryForAdd);
                    await context.SaveChangesAsync();
                }
            }
        }

    }
}