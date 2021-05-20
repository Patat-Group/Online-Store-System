using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Services.Data
{
    public class SubCategoriesAndProduct : ISubCategoriesAndProduct
    {

        private readonly StoreContext _context;

        public SubCategoriesAndProduct(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Product>> GeProductBytSubCategory(int subCategoryId)
        {
            var productsId = await _context.productAndSubCategories
                .Where(sc => sc.SubCategoryId == subCategoryId).Select(p => p.ProductId).ToListAsync();

            var products = await _context.Products.ToListAsync();

            var productsToReturn = new List<Product>();
            foreach (var product in products)
            {
                bool yes = false;
                foreach (var id in productsId)
                {
                    if (product.Id == id)
                        yes = true;
                }
                if (yes)
                    productsToReturn.Add(product);
            }
            return productsToReturn;
        }

        public async Task<IReadOnlyList<SubCategory>> GetSubCategoryByProduct(int productId)
        {
            var subCategoriesId = await _context.productAndSubCategories
                .Where(sc => sc.ProductId == productId).Select(p => p.SubCategoryId).ToListAsync();

            var subCategories = await _context.SubCategories.ToListAsync();

            var subCategoriesToReturn = new List<SubCategory>();
            foreach (var subCategory in subCategories)
            {
                bool yes = false;
                foreach (var id in subCategoriesId)
                {
                    if (subCategory.Id == id)
                        yes = true;
                }
                if (yes)
                    subCategoriesToReturn.Add(subCategory);
            }
            return subCategoriesToReturn;
        }

        public async Task<bool> AddSubCategoryToProduct(ProductAndSubCategory entity)
        {
            await _context.AddAsync(entity);
            return await SaveChanges();
        }

        public async Task<bool> UpdateSubCategoryWithProduct(ProductAndSubCategory entity)
        {
            _context.Update(entity);
            return await SaveChanges();
        }

        public async Task<bool> DeleteSubCategoryWithProduct(int id)
        {
            var subCategoryWithProucts = await _context.productAndSubCategories
                .FirstOrDefaultAsync(sc => sc.Id == id);
            if (subCategoryWithProucts == null)
                return false;

            _context.Remove(subCategoryWithProucts);
            return await SaveChanges();
        }

        public async Task<ProductAndSubCategory> GetById(int id)
        {
            return await _context.productAndSubCategories
                .FirstOrDefaultAsync(c => c.Id == id);
        }
        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}