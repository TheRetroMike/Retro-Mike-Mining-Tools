using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetroMikeMiningTools.DAO;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RetroMikeMiningTools.Pages
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class RegisterModel : PageModel
    {
        private readonly IConfiguration configuration;
        public RegisterModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [BindProperty]
        public string UserName { get; set; }
        [BindProperty, DataType(DataType.Password)]
        public string Password { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnPost()
        {
            var user = UserDAO.GetUser(UserName);
            if (String.IsNullOrEmpty(UserName))
            {
                Message = "Username required";
            }
            else if (String.IsNullOrEmpty(Password))
            {
                Message = "Password required";
            }
            else if (user != null)
            {
                Message = "Username already in use. Please try a different one or login";
            }
            else
            {
                var passwordHasher = new PasswordHasher<string>();
                var hashedPassword = passwordHasher.HashPassword(UserName, Password);
                UserDAO.AddUser(UserName, hashedPassword);
                Common.Logger.Push("Account Registered: " + UserName);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, UserName)
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                return RedirectToPage("/Index");
            }
            return Page();
        }
    }
}
