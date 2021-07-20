using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Services.Data
{
    public class SubCategoryRepository : IGenericRepository<SubCategory, int>
    {
        private readonly StoreContext _context;

        public SubCategoryRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<SubCategory>> GetAll()
        {
            return await _context.SubCategories.ToListAsync();
        }

        public Task<PagedList<SubCategory>> GetProductsByCategory(int categoryId, ProductParams productParams)
        {
            throw new System.NotImplementedException();
        }
        public async Task<IReadOnlyList<SubCategory>> GetAllSubCategoriesByProductId(int productId)
        {
            var subCategoriesIds = await _context.productAndSubCategories
                .Where(i => i.ProductId == productId).Select(sc => sc.SubCategoryId).ToListAsync();
            var list = new List<SubCategory>();
            foreach (var id in subCategoriesIds)
            {
                var SubCategories = await _context.SubCategories
                    .FirstOrDefaultAsync(i => i.Id == id);
                if (SubCategories != null)
                {
                    list.Add(SubCategories);
                }
            }
            return list;
        }
        public Task<PagedList<SubCategory>> GetAllWithSpec(ProductParams? productParams)
        {
            throw new System.NotImplementedException();
        }

        public async Task<SubCategory> GetById(int id)
        {
            return await _context.SubCategories
                .FirstOrDefaultAsync(sc => sc.Id == id);
        }

        public async Task<bool> Delete(int id)
        {
            var subCategory = await GetById(id);
            _context.Remove(subCategory);
            return await SaveChanges();
        }

        public async Task<bool> Add(SubCategory entity)
        {
            await _context.AddAsync(entity);
            return await SaveChanges();
        }

        public async Task<bool> Update(SubCategory entity)
        {
            _context.Update(entity);
            return await SaveChanges();
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}