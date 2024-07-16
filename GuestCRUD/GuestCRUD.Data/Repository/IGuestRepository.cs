using GuestCRUD.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuestCRUD.Data.Repository
{
    public interface IGuestRepository
    {
        Task<GuestDto> Get(int id);
        Task<GuestDto> Upsert(GuestDto guest);
        Task<bool> Delete(int guestId);
        Task<List<GuestDto>> Search(string firstName, string lastName, string email, string phone);
    }
}
