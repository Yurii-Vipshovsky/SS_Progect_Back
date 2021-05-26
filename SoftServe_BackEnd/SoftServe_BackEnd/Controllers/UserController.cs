using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoftServe_BackEnd.Database;
using SoftServe_BackEnd.Models;
using SoftServe_BackEnd.Services;

namespace SoftServe_BackEnd.Controllers
{
    [Route("/api/[controller]")]
    public class UserController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly IConfiguration _configuration;

        public UserController(DatabaseContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var emailOfCurrentUser = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var currentUser = _context.Clients.FirstOrDefault(
            clientModel => clientModel.Email == emailOfCurrentUser
            );
        return Ok(new Response<Client>(currentUser));
    }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUser userModel)
        {
            var emailIsBusy = FindClientByEmail(userModel.Email);
            if (emailIsBusy != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new Response<Client>
                {
                    Message = "Error",
                    Errors = new[] {"Email is already used"},
                    Succeeded = false,
                    Data = null
                });
            }

            var user = new Client
            {
                Login = userModel.NickName,
                Email = userModel.Email,
                Name = userModel.FullName,
                Birthday = userModel.Birthday,
                City = userModel.City,
                PhoneNumber = userModel.PhoneNumber,
                IsOrganization = userModel.IsOrganization,
                Site = userModel.SiteUrl,
                Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password)
            };
            
            await _context.Clients.AddAsync(user);
            await _context.SaveChangesAsync();
            return Ok(new Response<Client>
            {
                Message = "User created successfully",
                Data = user
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUser userModel)
        {
            var currentUser = FindClientByEmail(userModel.Email);
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status404NotFound, new Response<Client>
                {
                    Message = "Error",
                    Errors = new[] {"Incorrect data"},
                    Succeeded = false,
                    Data = null
                });

            if (currentUser == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response<Client>
                {
                    Message = "Error",
                    Errors = new[] {"User is not found"},
                    Succeeded = false,
                    Data = null
                });
            }

            var signInResult = BCrypt.Net.BCrypt.Verify(userModel.Password, currentUser.Password);

            if (!signInResult)
                return StatusCode(StatusCodes.Status404NotFound, new Response<Client>
                {
                    Message = "Error",
                    Errors = new[] {"Email or password wrong"},
                    Succeeded = false,
                    Data = null
                });

            var tokenString = new JwtSecurityTokenHandler().WriteToken(GenerateJsonWebToken(userModel));

            return Ok(new
            {
                Token = tokenString,
                Message = "Success"
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            return StatusCode(StatusCodes.Status200OK, new Response<Client>
            {
                Message = "Successful logout",
                Succeeded = true,
                Data = null
            });
        }

        private SecurityToken GenerateJsonWebToken(LoginUser userInfo)
        {
            var secretKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration.GetSection("Authentication:JWT:SecurityKey").Value));
            var signIn = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokenOptions = new JwtSecurityToken(
                _configuration.GetSection("Authentication:JWT:Issuer").Value,
                _configuration.GetSection("Authentication:JWT:Audience").Value,
                new List<Claim>{
                    new(JwtRegisteredClaimNames.Sub, userInfo.Email)
                },
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signIn
            );

            return tokenOptions;
        }
        
        private Client FindClientByEmail(string email)
        {
            var client = _context.Clients.FirstOrDefault(clientModel => clientModel.Email == email);
            return client;
        }
    }
}