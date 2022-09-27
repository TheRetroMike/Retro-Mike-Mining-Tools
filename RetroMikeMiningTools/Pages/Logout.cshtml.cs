using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RetroMikeMiningTools.Pages
{
    [ResponseCache(NoStore = true, Duration = 0)]
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            SignOut().Wait();
            return RedirectToPage("/Index");
        }

        public async Task SignOut()
        {
            await HttpContext.SignOutAsync("Cookies");
            HttpContext.Response.Cookies.Delete(".AspNetCore.Cookies");
        }
    }
}
