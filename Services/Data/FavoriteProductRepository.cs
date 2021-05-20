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
    public class FavoriteProductRepository : IFavoriteProductRepository
    {
        private readonly StoreContext _context;

        public FavoriteProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<FavoriteProduct>> GetAll()
        {
            var favoriteProduct = await _context.FavoriteProducts
                .Include(p=>p.Product)
                .Include(img=>img.Product.Images)
                .ToListAsync();
            return favoriteProduct;
        }

        public async Task<IReadOnlyList<FavoriteProduct>> GetAllByUserId(string userId)
        {
            var favoriteProduct = await _context.FavoriteProducts
                .Where(u=>u.UserId==userId)
                .Include(p=>p.Product)
                .Include(img=>img.Product.Images)
                .ToListAsync();
            return favoriteProduct;
        }

        public async Task<PagedList<FavoriteProduct>> GetAllWithSpec(ProductParams? productParams)
        {
            var favoriteProduct = _context.FavoriteProducts
                .Include(p=>p.Product)
                .Include(img=>img.Product.Images)
                .AsQueryable();

            return await PagedList<FavoriteProduct>.CreatePagingListAsync(favoriteProduct, productParams.PageNumber,
                productParams.PageSize);
        }

        public async Task<PagedList<FavoriteProduct>> GetAllByUserIdWithSpec(string userId, ProductParams? productParams)
        {
            var favoriteProduct = _context.FavoriteProducts
                .Where(u=>u.UserId==userId)
                .Include(p=>p.Product)
                .Include(img=>img.Product.Images)
                .AsQueryable();

            return await PagedList<FavoriteProduct>.CreatePagingListAsync(favoriteProduct, productParams.PageNumber,
                productParams.PageSize);
        }

        public async Task<FavoriteProduct?> Get(string userId, int productId)
        {
            var result = await _context.FavoriteProducts.SingleOrDefaultAsync(x=>x.UserId==userId && x.ProductId==productId);
            return result;
                
        }

        public async Task<bool> Delete(FavoriteProduct favoriteProduct)
        {
            _context.FavoriteProducts.Remove(favoriteProduct);
            return await SaveChanges();
            
        }

        public async Task<bool> Add(FavoriteProduct favoriteProduct)
        {
            await _context.FavoriteProducts.AddAsync(favoriteProduct);
            return await SaveChanges();
        }
        private async Task<bool> SaveChanges()
        {
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}