#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Interfaces.Core;
using Microsoft.EntityFrameworkCore;

namespace Services.Data
{
    public class CategoryRepository : IGenericRepository<Category, int>
    {
        private readonly StoreContext _context;

        public CategoryRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Category>> GetAll()
        {
            return await _context.Categories.ToListAsync();
        }

        public Task<PagedList<Category>> GetAllWithPaging(ProductParams? productParams)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Category> GetById(int id)
        {
            return await _context.Categories
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> Delete(int id)
        {
            var category = await GetById(id);
            _context.Remove<Category>(category);
            return await SaveChanges();
        }

        public async Task<bool> Add(Category entity)
        {
            await _context.AddAsync(entity);
            return await SaveChanges();
        }

        public async Task<bool> Update(Category entity)
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