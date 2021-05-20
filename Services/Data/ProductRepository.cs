#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Helpers;
using Core.Interfaces;
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

        public async Task<PagedList<Product>> GetAllWithSpec(ProductParams? productParams)
        {
            var products = _context.Products
                .Include(im => im.Images)
                .AsQueryable();

            products = products.OrderByDescending(d => d.DateAdded);

            if (!string.IsNullOrEmpty(productParams.CategoryNameFilter))
            {
                products = _context.Products
                .Include(im => im.Images)
                .AsQueryable();

                products = products.Where(c => c.CategoryName == productParams.CategoryNameFilter);


                if (!string.IsNullOrEmpty(productParams.SubCategoryNameFilter))
                {
                    var subCategory = await _context.SubCategories
                        .FirstOrDefaultAsync(n => n.Name == productParams.SubCategoryNameFilter.ToLower());
                    if (subCategory != null)
                    {
                        var productsBySubCategory = _context.productAndSubCategories
                            .Where(i => i.SubCategoryId == subCategory.Id);

                        if (productsBySubCategory != null)
                        {
                            products = products.Where(x => productsBySubCategory.Any(e => e.ProductId == x.Id));
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(productParams.Search))
            {
                products = _context.Products
                .Include(im => im.Images)
                .AsQueryable();

                products = products.Where(p => p.Name.Contains(productParams.Search.ToLower()));
            }

            if (productParams.SortByLowerPrice)
            {
                products = products.OrderBy(p => p.Price);
            }

            if (productParams.SortByNewest)
            {
                products = products.OrderByDescending(d => d.DateAdded);
            }

            if (productParams.SortByHigerPrice)
            {
                products = products.OrderByDescending(p => p.Price);
            }

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