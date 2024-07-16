using GuestCRUD.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestCRUD.Data.EF
{
    public interface IGuestCRUDDbContext
    {
        DbSet<Guest> Guest { get; set; }
        Task<int> SaveChangesAsync();
    }
}
