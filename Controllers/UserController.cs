using DatingApp.Api.DataAccess;
using DatingApp.Api.DataAccess.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatingApp.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly DatingAppDbContext _datingAppDbContext;
        public UserController(DatingAppDbContext datingAppDbContext)
        {
            _datingAppDbContext = datingAppDbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAll()
        {
            var users = await _datingAppDbContext.Users.ToListAsync();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetId(int id)
        {
            var user = await _datingAppDbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(user);
        }
    }
}
