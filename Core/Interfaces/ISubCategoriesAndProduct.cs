using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ISubCategoriesAndProduct
    {
        public Task<ProductAndSubCategory> GetById(int id);
        public Task<IReadOnlyList<SubCategory>> GetSubCategoryByProduct(int productId);
        public Task<IReadOnlyList<Product>> GeProductBytSubCategory(int subCategoryId);
        public Task<bool> AddSubCategoryToProduct(ProductAndSubCategory entity);
        public Task<bool> UpdateSubCategoryWithProduct(ProductAndSubCategory entity);
        public Task<bool> DeleteSubCategoryWithProduct(int id);
        public Task<bool> SaveChanges();

    }
}