﻿#nullable enable
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Helpers;

namespace Core.Interfaces
{
    public interface IGenericRepository<T, TKey>
    {
        public Task<IReadOnlyList<T>> GetAll();
        public Task<PagedList<T>> GetAllWithPaging(ProductParams? productParams);
        public Task<T> GetById(TKey id);
        public Task<bool> Delete(TKey id);
        public Task<bool> Add(T entity);
        public Task<bool> Update(T entity);
        public Task<bool> SaveChanges();
    }
}