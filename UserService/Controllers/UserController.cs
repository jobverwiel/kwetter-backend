using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using UserService.Messaging;
using UserService.Models;
using UserService.Models.ViewModels;
using BC = BCrypt.Net.BCrypt;


namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserServiceDatabaseContext _context;
        private readonly IMessageService _messageService;
        public UserController(UserServiceDatabaseContext context, IMessageService messageService)
        {
            _context = context;
            _messageService = messageService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            registerViewModel.Username = "@" + registerViewModel.Username;
            if (registerViewModel == null)
            {
                return BadRequest("Fill in the required fields");
            }
            if (string.IsNullOrWhiteSpace(registerViewModel.Username))
            {
                return BadRequest("Fill in a username");
            }
            if (string.IsNullOrWhiteSpace(registerViewModel.Email))
            {
                return BadRequest("Fill in a email");
            }
            if (string.IsNullOrWhiteSpace(registerViewModel.Password))
            {
                return BadRequest("Fill in a password");
            }
            if (string.IsNullOrWhiteSpace(registerViewModel.ConfirmPassword))
            {
                return BadRequest("Fill in a confirm password");
            }
            if (string.IsNullOrWhiteSpace(registerViewModel.DisplayName))
            {
                return BadRequest("Fill in a display name");
            }
            User foundUser = await _context.Users.Where(x => x.Username == registerViewModel.Username).FirstOrDefaultAsync();
            if (foundUser != null)
            {
                return BadRequest("This username is already in use");
            }
            foundUser = await _context.Users.Where(x => x.Email == registerViewModel.Email).FirstOrDefaultAsync();
            if (foundUser != null)
            {
                return BadRequest("This email is already in use");
            }
            if (registerViewModel.Password != registerViewModel.ConfirmPassword)
            {
                return BadRequest("Password and confirm password must match");
            }
            User user = new()
            {
                Username = registerViewModel.Username,
                DisplayName = registerViewModel.DisplayName,
                Password = BC.HashPassword(registerViewModel.Password, BC.GenerateSalt(12)),
                Email = registerViewModel.Email,
                VerificationToken = await generateRandomString(8)
            };
            if (!BC.Verify(registerViewModel.Password, user.Password))
            {
                return BadRequest("Something went wrong please try again!");
            }
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
        private async Task<string> generateRandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var randomString =  new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            if(await _context.Users.FirstOrDefaultAsync(x => x.VerificationToken == randomString) != null)
            {
                return await generateRandomString(9);
            }
            else
            {
                return randomString;
            }
        }
        [HttpGet("allUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var Users = await _context.Users.ToListAsync();
            var json  = JsonSerializer.Serialize(Users);
            return Ok(json);
        }
        [HttpGet("test")]
        public async Task<IActionResult> test()
        {
            _messageService.Enqueue("Hello queue");
            //User user = new User() { Email = "xD@gmail.com" };
            return Ok();
        }
        [HttpGet("GetUsers")]
        [Authorize]
        public async Task<IActionResult> GetUsers()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                var username = claims
                .Where(x => x.Type == ClaimTypes.NameIdentifier)
                .FirstOrDefault().Value;

            }
            var users = await _context.Users.ToListAsync();
            var jsonUsers = JsonSerializer.Serialize(users);
            return Ok(jsonUsers);
        }
    }
}
