#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Helpers;

namespace Core.Interfaces
{
    public interface IFavoriteProductRepository
    {
        public Task<IReadOnlyList<FavoriteProduct>> GetAll();
        public Task<IReadOnlyList<FavoriteProduct>> GetAllByUserId(string userId);
        public Task<PagedList<FavoriteProduct>> GetAllWithSpec(ProductParams? productParams);
        public Task<PagedList<FavoriteProduct>> GetAllByUserIdWithSpec(string userId,ProductParams? productParams);
        public Task<FavoriteProduct?> Get(string userId, int productId);
        public Task<bool> Delete(FavoriteProduct favoriteProduct);
        public Task<bool> Add(FavoriteProduct favoriteProduct);
    }
}