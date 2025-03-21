using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using progetto_settimanale_S18.Models;
using progetto_settimanale_S18.Services;
using progetto_settimanale_S18.ViewModels;

namespace progetto_settimanale_S18.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountService _accountService;

        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var authenticated = User.Identity.IsAuthenticated;
            if (authenticated)
            {
                TempData["Error"] = "You are already authenticated!";
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var result = await _accountService.LoginAsync(loginViewModel);
            if (!result)
            {
                TempData["Error"] = "Error while logging in!";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(registerViewModel);
            }

            var result = await _accountService.RegisterAsync(registerViewModel);
            if (!result) 
            {
                TempData["Error"] = "Error while registering a new account!";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var result = await _accountService.LogoutAsync();
            if (!result)
            {
                TempData["Error"] = "Error while logging out!";
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
