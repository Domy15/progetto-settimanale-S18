using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using progetto_settimanale_S18.Models;
using progetto_settimanale_S18.ViewModels;
using Microsoft.AspNetCore.Authentication;

namespace progetto_settimanale_S18.Services
{
    public class AccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IHttpContextAccessor httpContextAccessor
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(LoginViewModel loginViewModel)
        {
            var user = await _userManager.FindByEmailAsync(loginViewModel.Email);

            if (user == null)
            {
                return false;
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, true, false);

            if (!signInResult.Succeeded)
            {
                return false;
            }

            var Roles = await _signInManager.UserManager.GetRolesAsync(user);

            var claims = new List<Claim>();

            foreach (var role in Roles)
            {
                var roleClaim = new Claim(ClaimTypes.Role, role);
                claims.Add(roleClaim);
            }

            claims.Add(new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"));

            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return true;
        }

        public async Task<bool> RegisterAsync(RegisterViewModel registerViewModel)
        {
            var newUser = new ApplicationUser()
            {
                Email = registerViewModel.Email,
                UserName = registerViewModel.Email,
                FirstName = registerViewModel.FirstName,
                LastName = registerViewModel.LastName,
                BirthDate = registerViewModel.BirthDate
            };

            var result = await _userManager.CreateAsync(newUser, registerViewModel.Password);

            if (!result.Succeeded)
            {
                return false;
            }

            var user = await _userManager.FindByEmailAsync(newUser.Email);

            if (user == null)
            {
                return false;
            }

            await _userManager.AddToRoleAsync(user, registerViewModel.Role);

            return true;
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
