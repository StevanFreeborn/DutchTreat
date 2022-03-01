﻿using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace DutchTreat.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<StoreUser> _signInManager;
        private readonly UserManager<StoreUser> _userManager;
        private readonly IConfiguration _config;

        public AccountController(
            ILogger<AccountController> logger, 
            SignInManager<StoreUser> signInManager,
            UserManager<StoreUser> userManager,
            IConfiguration config)
        {
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        public IActionResult Login()
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "App");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to login.");
                return View();
            }

            var result = await _signInManager
                .PasswordSignInAsync(
                    model.Username, 
                    model.Password, 
                    model.RememberMe, 
                    false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Failed to login.");
                return View();
            }

            if (Request.Query.Keys.Contains("ReturnUrl"))
            {
                return Redirect(Request.Query["ReturnUrl"].First());
            }
                    
            return RedirectToAction("Shop", "App");

        }

        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "App");
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

            if (!ModelState.IsValid || user == null || !result.Succeeded)
            {
                return BadRequest("Unable to authenticate.");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["Token:Issuer"],
                _config["Token:Audience"],
                claims,
                signingCredentials: creds,
                expires: DateTime.UtcNow.AddMinutes(20));

            return Created("", new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
    }
}
