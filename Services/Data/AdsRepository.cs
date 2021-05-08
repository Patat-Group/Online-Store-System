﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Interfaces.Core;
using Microsoft.EntityFrameworkCore;

namespace Services.Data
{
    public class AdsRepository : IGenericRepository<VIPAd, int>
    {
        private readonly StoreContext _context;

        public AdsRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<VIPAd>> GetAll()
        {
            var vipAds = await _context.VIPAds.ToListAsync();
            return vipAds;
        }

        public async Task<VIPAd> GetById(int id)
        {
            return await _context.VIPAds
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<bool> Delete(int id)
        {
            var vipAd = await GetById(id);
            _context.Remove(vipAd);
            if (await SaveChanges())
                return true;
            return false;
        }

        public async Task<bool> Add(VIPAd entity)
        {
            await _context.AddAsync(entity);
            if (await SaveChanges())
                return true;
            return false;
        }

        public async Task<bool> Update(VIPAd entity)
        {
            if (entity == null) return false;
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