using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RetroMikeMiningTools.Common;
using RetroMikeMiningTools.DAO;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace RetroMikeMiningTools.Pages
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class LoginModel : PageModel
    {
        private readonly IConfiguration configuration;
        private static bool multiUserMode = false;
        private static string? username;

        public LoginModel(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        [BindProperty]
        public string UserName { get; set; }
        [BindProperty, DataType(DataType.Password)]
        public string Password { get; set; }
        public string Message { get; set; }

        public async Task<IActionResult> OnGet()
        {
            if (configuration != null)
            {
                var multiUserModeConfig = configuration.GetValue<string>(Constants.MULTI_USER_MODE);
                var fluxModeConfig = configuration.GetValue<string>(Constants.FLUX_MODE);
                if (!String.IsNullOrEmpty(multiUserModeConfig) && multiUserModeConfig == "true")
                {
                    return Redirect("https://retromike.net");
                }
                    if (!String.IsNullOrEmpty(multiUserModeConfig) && multiUserModeConfig == "true")
                {
                    username = User?.Identity?.Name;
                    multiUserMode = true;
                    ViewData["MultiUser"] = true;
                }
            }
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = UserDAO.GetUser(UserName);
            if (user != null)
            {
                if (UserName == user.UserName)
                {
                    var passwordHasher = new PasswordHasher<string>();
                    if (passwordHasher.VerifyHashedPassword(null, user.Password, Password) == PasswordVerificationResult.Success)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, UserName)
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                        return RedirectToPage("/Index");
                    }
                }
            }
            Message = "Invalid Credentials. Please Try Again or Register for an account";
            return Page();
        }
    }
}
