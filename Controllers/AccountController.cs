using System;
using System.Linq;
using DatingApp.Api.DataAccess;
using DatingApp.Api.DataAccess.Entities;
using DatingApp.Api.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatingApp.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DatingAppDbContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DatingAppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            try
            {
                using var hmac = new HMACSHA512();
                var newUser = new AppUser
                {
                    UserName = registerDto.UserName.ToLower(),
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                    PasswordSalt = hmac.Key
                };
                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return Created("/register",new UserDto
                {
                    UserName = newUser.UserName,
                    Token = await _tokenService.CreateToken(newUser)
                });
            }
            catch (DbUpdateException)
            {
                return BadRequest($"{registerDto.UserName} already taken! Please try another user name.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName.ToLower() == loginDto.UserName);
            if (user == null)
                return Unauthorized("Invalid username or password!");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            if (!computedHash.SequenceEqual(user.PasswordHash))
                return Unauthorized("Invalid username or password!");
            return Ok(new UserDto
            {
                UserName = user.UserName,
                Token = await _tokenService.CreateToken(user)
            });
        }
    }
}
