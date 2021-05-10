using System.Collections.Generic;
using System.Threading.Tasks;
using API.Helpers;

namespace Interfaces.Core
{
    public interface IGenericRepository<T, TKey>
    {
        public Task<IReadOnlyList<T>> GetALl();
        public Task<PagedList<T>> GetALlWithPaging(ProductParams? productParams);
        public Task<T> GetById(TKey id);
        public Task<bool> Delete(TKey id);
        public Task<bool> Add(T entity);
        public Task<bool> Update(T entity);
        public Task<bool> SaveChanges();
    }
}