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
    public class ProductImagesSeed
    {
        public static async Task SeedProductImages(StoreContext context)
        {
            if (!context.ProductImages.Any())
            {
                var imagesData = await File.ReadAllTextAsync("../services/Seeds/Data/ProductImages.json");
                var images = JsonSerializer.Deserialize<List<ProductImage>>(imagesData);
                foreach (var image in images)
                {
                    var imageForAdd = new ProductImage
                    {
                        ProductId =  image.ProductId,
                        ImageUrl = image.ImageUrl,
                        IsMainPhoto = image.IsMainPhoto,
                        DateAdded = DateTime.UtcNow
                    };

                    await context.AddAsync(imageForAdd);
                    await context.SaveChangesAsync();
                }
            }
        }
        
    }
}