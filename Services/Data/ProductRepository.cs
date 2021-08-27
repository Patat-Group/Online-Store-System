#nullable enable
using System;
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
                .Include(us => us.User)
                .AsQueryable();

            products = products.OrderByDescending(d => d.DateAdded);
            products = products.Where(p => p.IsSold == false);
            if (productParams.CategoryIdFilter != 0)
            {
                products = _context.Products
                .Include(im => im.Images)
                .Include(us => us.User)
                .AsQueryable();

                if (productParams.CategoryIdFilter != 0)
                {
                    var category = await _context.Categories
                        .FirstOrDefaultAsync(x => x.Id == productParams.CategoryIdFilter);
                    if (category != null)
                    {
                        products = products.Where(c => c.CategoryId == category.Id);
                    }
                }
            }
            if (productParams.SubCategoryIdFilter != 0)
            {
                var subCategory = await _context.SubCategories
                    .FirstOrDefaultAsync(n => n.Id == productParams.SubCategoryIdFilter);
                if (subCategory != null)
                {
                    var productsBySubCategory = _context.productAndSubCategories
                        .Where(i => i.SubCategoryId == subCategory.Id);
                    if (productsBySubCategory != null)
                    {
                        products = products.Where(x => productsBySubCategory.Any(e => e.ProductId == x.Id));
                    }
                    else
                    {
                        products = products.Take(0);
                    }
                }
                else
                {   
                    products = products.Take(0);
                }
            }

            if (!string.IsNullOrEmpty(productParams.Search))
            {
                products = _context.Products
                .Include(im => im.Images)
                .Include(us => us.User)
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


        public async Task<PagedList<Product>> GetProductsByCategory(int categoryId, ProductParams? productParams)
        {
            var products = _context.Products
                .Include(im => im.Images)
                .Include(us => us.User)
                .AsQueryable();

            products = products.Where(c => c.CategoryId == categoryId);
            products = products.OrderByDescending(d => d.DateAdded);
            products = products.Where(p => p.IsSold == false);


            if (productParams.SubCategoryIdFilter != 0)
            {
                var subCategory = await _context.SubCategories
                    .FirstOrDefaultAsync(n => n.Id == productParams.SubCategoryIdFilter);
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
                .Include(us => us.User)
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

        public Task<IReadOnlyList<Product>> GetAllSubCategoriesByProductId(int productId)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> getAllSubCategoriesByCategoryId(int categoryId)
        {
            throw new System.NotImplementedException();
        }
    }
}