using GuestCRUD.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestCRUD.Data.EF
{
    public class GuestCRUDDbContext : DbContext,IGuestCRUDDbContext
    {
        public GuestCRUDDbContext() { }

        public GuestCRUDDbContext(DbContextOptions<GuestCRUDDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Guest> Guest { get; set; }
        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Guest>(entity =>
            {
                entity.HasKey(c => c.Id);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}