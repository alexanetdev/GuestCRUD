
using GuestCRUD.Data.Repository;
using GuestCRUD.Dto;
using GuestCRUD.Service.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GuestCRUD.Service.Controllers
{
    [Route("api/[controller]")]
    public class GuestController : Controller
    {
        private readonly IGuestRepository _guestRepository;
        public GuestController(IGuestRepository guestRepo)
        {
            this._guestRepository = guestRepo;
        }

        [Route("{id}")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await _guestRepository.Get(id));
        }

        [Route("")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody]GuestDto guest)
        {
            var validationExceptions = new List<string>();
            if (!guest.EmailAddress.IsValidEmail())
            {
                validationExceptions.Add("Email address is not valid.");
            }
            //TODO: Continue validating of phone, address, etc. 
            if (validationExceptions.Count > 0)
            {
                return BadRequest(string.Join(',', validationExceptions));
            }

            return Ok(await _guestRepository.Upsert(guest));
        }

        [Route("{id}/update")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody]GuestDto guest)
        {
            guest.Id = id;
            return Ok(await _guestRepository.Upsert(guest));
        }

        [Route("{id}/delete")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _guestRepository.Delete(id));
        }
    }
}
