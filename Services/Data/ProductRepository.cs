#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
using Interfaces.Core;
using Microsoft.EntityFrameworkCore;

namespace Services.Data
{
    public class ProductRepository : IGenericRepository<Product, int>
    {
        private readonly StoreContext _context;

        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<Product>> GetAll()
        {
            var products = await _context.Products
                .Include(im => im.Images)
                .ToListAsync();
            return products;
        }

        public async Task<PagedList<Product>> GetAllWithPaging(ProductParams? productParams)
        {
            var products = _context.Products
                .Include(im => im.Images)
                .AsQueryable();

            return await PagedList<Product>.CreatePagingListAsync(products, productParams.PageNumber,
                productParams.PageSize);
        }

        public async Task<Product> GetById(int id)
        {
            var product = await _context.Products
                .Include(im => im.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            return product;
        }

        public async Task<bool> Delete(int id)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return false;
            _context.Products.Remove(product);
            if (await SaveChanges())
                return true;
            return false;
        }

        public async Task<bool> Add(Product entity)
        {
            await _context.AddAsync(entity);
            if (await SaveChanges())
                return true;
            return false;
        }

        public async Task<bool> Update(Product entity)
        {
            _context.Update(entity);
            if (await SaveChanges())
                return true;
            return false;
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0 ? true : false;
        }
    }
}