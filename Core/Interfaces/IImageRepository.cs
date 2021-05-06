using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Interfaces.Core
{
    public interface IImageRepository
    {
        public Task<IReadOnlyList<ProductImage>> GetAllImagesForProduct(int productId);
        public Task<ProductImage> GetImageById(int id);
        public Task<bool> AddImage(ProductImage entity);
        public Task<bool> DeleteImage(int id);
        public Task<bool> SetMainImage(int id);
        public Task<bool> SaveChanges();
    }
}