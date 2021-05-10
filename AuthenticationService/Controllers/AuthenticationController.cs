using AuthenticationService.Database_context;
using AuthenticationService.Helper;
using AuthenticationService.Models;
using AuthenticationService.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;


namespace AuthenticationService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : Controller
    {
        private readonly AuthenticationServiceDatabaseContext _context;
        public AuthenticationController(AuthenticationServiceDatabaseContext context)
        {
            _context = context;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (string.IsNullOrWhiteSpace(loginViewModel.Username))
            {
                return BadRequest("Fill in your username");
            }
            if (string.IsNullOrWhiteSpace(loginViewModel.Password))
            {
                return BadRequest("Fill in your password");
            }
            loginViewModel.Username = "@" + loginViewModel.Username;
            User user = await _context.Users.FirstOrDefaultAsync(x => x.Username == loginViewModel.Username);
            if (user == null)
            {
                return BadRequest("There was no user found with these credentials!");
            }
            if (!BC.Verify(loginViewModel.Password, user.Password))
            {
                return BadRequest("Something went wrong during logging in, try again");
            }
            if (user.VerificationToken != null)
            {
                return Unauthorized("No verification code filled in!");
            }
            TokenBuilder tknBuilder = new TokenBuilder();
            var token = tknBuilder.BuildToken(loginViewModel.Username);
            //var serializedToken = JsonSerializer.Serialize(token);
            TokenViewModel tknViewModel = new TokenViewModel();
            tknViewModel.Token = token;
            return Ok(tknViewModel);
        }

        [HttpPost("VerifyToken")]
        public async Task<IActionResult> VerifyToken(VerificationTokenViewModel verificationTokenViewModel)
        {
            if (verificationTokenViewModel == null)
            {
                return BadRequest();
            }
            if (verificationTokenViewModel.loginViewModel == null)
            {
                return BadRequest();
            }

            User user = await _context.Users.FirstOrDefaultAsync(x => x.Username == "@" + verificationTokenViewModel.loginViewModel.Username && x.Password == verificationTokenViewModel.loginViewModel.Password);
            if (user == null || string.IsNullOrWhiteSpace(user.VerificationToken))
            {
                return BadRequest();
            }
            if (user.VerificationToken != verificationTokenViewModel.VerificationToken)
            {
                return BadRequest("This is not the token linked to your account try again!");
            }
            user.VerificationToken = null;
            await _context.SaveChangesAsync();
            return await Login(verificationTokenViewModel.loginViewModel);
        }

        [HttpGet]
        [Authorize]
        public async Task<string> GetUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string username = "";
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                // or
                username = claims
                .Where(x => x.Type == ClaimTypes.NameIdentifier)
                .FirstOrDefault().Value;
            }

            return username;
        }
    }
}
