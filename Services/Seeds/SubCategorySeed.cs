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
    public class SubCategorySeed
    {
        public static async Task SeedSubCategory(StoreContext context)
        {
            if (!context.SubCategories.Any())
            {
                var subCategoriesData = await File.ReadAllTextAsync("../services/Seeds/Data/SubCategory.json");
                var subCategories = JsonSerializer.Deserialize<List<SubCategory>>(subCategoriesData);
                foreach (var subCategory in subCategories)
                {
                    var subCategoryForAdd = new SubCategory
                    {
                        Name = subCategory.Name.ToLower()
                    };

                    await context.AddAsync(subCategoryForAdd);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}