using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Interfaces.Core;
using Microsoft.EntityFrameworkCore;

namespace Services.Data
{
    public class ProductImageRepository : IImageRepository
    {
        public readonly StoreContext _context;

        public ProductImageRepository(StoreContext context)
        {
            _context = context;
        }

        /// <summary>
        /// This Function For Get All Images For Product 
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Images</returns>
        public async Task<IReadOnlyList<ProductImage>> GetAllImagesForProduct(int productId)
        {
            var product = await _context.Products
                .Where(p => p.Id == productId).ToListAsync();
            if (product == null)
                return null;
            var images = await _context.ProductImages
                .Where(x => x.ProductId == productId).ToListAsync();
            return images;
        }

        public async Task<ProductImage> GetImageById(int id)
        {
            var image = await _context.ProductImages
                .FirstOrDefaultAsync(x => x.Id == id);
            return image;
        }

        /// <summary>
        /// This Function For Adding Image
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>True if Success</returns>
        public async Task<bool> AddImage(ProductImage entity)
        {
            await _context.ProductImages.AddAsync(entity);
            return await SaveChanges();
        }

        public async Task<bool> DeleteImage(int id)
        {
            var image = await GetImageById(id);
            if (image == null)
                return false;
            _context.Remove(image);

            return await SaveChanges();
        }

        /// <summary>
        /// This function for set main photo for product
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true if the process successfully</returns>
        public async Task<bool> SetMainImage(int id)
        {
            var image = await GetImageById(id);
            if (image == null) return false;

            var images = await GetAllImagesForProduct(image.ProductId);
            var mainImage = images.FirstOrDefault(x => x.IsMainPhoto);
            if (mainImage !=null && mainImage.Id == image.Id)
                return true;
            if (mainImage != null) mainImage.IsMainPhoto = false;
            image.IsMainPhoto = true;
            return await SaveChanges();
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}