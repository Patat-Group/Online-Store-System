using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Services.Data;

namespace Services.Seeds
{
    public class ProductLinkSubCategory
    {
        public static async Task SeedProductLinkSubCategoriesAsync(StoreContext context)
        {
            if (!context.productAndSubCategories.Any())
            {
                var productAndSubCategoryData = await File.ReadAllTextAsync("../Services/Seeds/Data/LinkProductAndSubCategory.json");
                var data = JsonSerializer.Deserialize<List<ProductAndSubCategory>>(productAndSubCategoryData);
                foreach (var item in data)
                {
                    var productLinkSubCategory = new ProductAndSubCategory()
                    {
                        ProductId = item.ProductId,
                        SubCategoryId = item.SubCategoryId
                    };
                    await context.AddAsync(productLinkSubCategory);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}