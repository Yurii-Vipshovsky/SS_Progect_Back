using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SoftServe_BackEnd.Models;
using SoftServe_BackEnd.Services;

namespace SoftServe_BackEnd.Controllers
{
    [Route("/[controller]")]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signinManager;
        private readonly IConfiguration _configuration;
        
        public UserController(UserManager<User> userManager, SignInManager<User> signinManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signinManager = signinManager;
            _configuration = configuration;
        }
        
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUser userModel)
        {
            var emailIsBusy = await _userManager.FindByEmailAsync(userModel.Email);
            if (emailIsBusy != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new Response<User>
                {
                    Message = "Error",
                    Errors = new[] {"Email is already used"},
                    Succeeded = false,
                    Data = null
                });
            }

            var nickIsBusy = await _userManager.FindByNameAsync(userModel.NickName);
            if (nickIsBusy != null)
            {
                return StatusCode(StatusCodes.Status409Conflict, new Response<User>
                {
                    Message = "Error",
                    Errors = new[] {"Login name is already used"},
                    Succeeded = false,
                    Data = null
                });
            }
            
            var user = new User
            {
                UserName = userModel.NickName,
                Email = userModel.Email,
                FullName = userModel.FullName,
                Birthday =  userModel.Birthday,
                City =  userModel.City,
                PhoneNumber = userModel.PhoneNumber,
                IsOrganization = userModel.IsOrganization,
                SiteUrl = userModel.SiteUrl,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response<User>
                {
                    Message = "Error",
                    Errors = new[] {"Something wrong"},
                    Succeeded = false,
                    Data = null
                });
            }

            await _signinManager.SignInAsync(user, false);
            return Ok(new Response<User>
            {
                Message = "User created successfully",
                Data = user
            });
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUser userModel)
        {
            var checkByEmail = await _userManager.FindByEmailAsync(userModel.LoginString);
            var checkByNickName = await _userManager.FindByNameAsync(userModel.LoginString);
            if (!ModelState.IsValid)
                return StatusCode(StatusCodes.Status404NotFound, new Response<User>
                {
                    Message = "Error",
                    Errors = new[] {"Incorrect data"},
                    Succeeded = false,
                    Data = null
                });

            if (checkByEmail == null && checkByNickName == null)
            {
                return StatusCode(StatusCodes.Status404NotFound, new Response<User>
                {
                    Message = "Error",
                    Errors = new[] {"User is not found"},
                    Succeeded = false,
                    Data = null
                });
            }

            var currentUser = checkByEmail ?? checkByNickName;
            var signInResult =
                await _signinManager.PasswordSignInAsync(currentUser.UserName, userModel.Password, true, false);
            
            if (!signInResult.Succeeded)
                return StatusCode(StatusCodes.Status404NotFound, new Response<User>
                {
                    Message = "Error",
                    Errors = new[] {"Email/NickName or password wrong"},
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
            await _signinManager.SignOutAsync();
            return StatusCode(StatusCodes.Status200OK, new Response<User>
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
                issuer: _configuration.GetSection("Authentication:JWT:Issuer").Value,
                audience: _configuration.GetSection("Authentication:JWT:Audience").Value,
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signIn
            );

            return tokenOptions;
        }
    }
}