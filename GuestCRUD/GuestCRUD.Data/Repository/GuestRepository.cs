using GuestCRUD.Data.EF;
using GuestCRUD.Data.Models;
using GuestCRUD.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace GuestCRUD.Data.Repository
{
    public class GuestRepository : IGuestRepository
    {
        private readonly IGuestCRUDDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;
        public GuestRepository(IDbContextFactory<GuestCRUDDbContext> dbContextFactory)
        {
            this._dbContext = dbContextFactory.CreateDbContext();
            this._memoryCache = new MemoryCache(
                new MemoryCacheOptions
                {
                });
            }   

        public async Task<GuestDto> Get(int id)
        {
            if (_memoryCache.TryGetValue(id, out var guestDto))
            {
                return (GuestDto)guestDto;
            }

            var entity = await _dbContext.Guest.AsNoTracking().FirstOrDefaultAsync(g => g.Id == id);
            if (entity == null)
            {
                return null;
            }

            _memoryCache.CreateEntry(id);
            _memoryCache.Set(id, entity.ToDto());

            return entity.ToDto();
        }

        public async Task<GuestDto> Upsert(GuestDto guest)
        {
            var entityId = guest.Id;
            if (guest.Id != 0 && _dbContext.Guest.AsNoTracking().Any(g => g.Id == guest.Id))
            {
                _dbContext.Guest.Update(guest.FromDto());
                _memoryCache.Remove(guest.Id);

                await _dbContext.SaveChangesAsync();
                _memoryCache.Set(entityId, guest);
                return guest;
            }
            else
            {
                var entity = await _dbContext.Guest.AddAsync(guest.FromDto());
                entityId = entity.Entity.Id;
                _memoryCache.CreateEntry(entityId);

                await _dbContext.SaveChangesAsync();
                _memoryCache.Set(entityId, guest);
                return entity.Entity.ToDto();
            }
        }

        public async Task<bool> Delete(int guestId)
        {
            var entity = _dbContext.Guest.AsNoTracking().FirstOrDefault(g => g.Id == guestId);
            if (entity != null)
            {
                _dbContext.Guest.Remove(entity);
                await _dbContext.SaveChangesAsync();
                _memoryCache.Remove(guestId);
                return true;
            }
            return false;
        }

        public async Task<List<GuestDto>> Search(string firstName, string lastName, string email, string phone)
        {
            var guestQuery = _dbContext.Guest.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(firstName))
            {
                guestQuery = guestQuery.Where(g => g.FirstName == firstName);
            }

            if (!string.IsNullOrWhiteSpace(lastName))
            {
                guestQuery = guestQuery.Where(g => g.LastName == lastName);
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                guestQuery = guestQuery.Where(g => g.EmailAddress == email);
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                guestQuery = guestQuery.Where(g => g.PhoneNumber == phone);
            }

            return await guestQuery.Select(g => g.ToDto()).ToListAsync();
        }
    }
}
