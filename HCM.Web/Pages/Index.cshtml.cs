using HCM.Core.Models.User;
using HCM.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HCM.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUsersService _usersService;

        [BindProperty]
        public LoginUserDto LoginUserModel { get; set; }

        public IndexModel(IUsersService usersService)
        {
            _usersService = usersService;
        }

        public void OnGet()
        {
            string? err = HttpContext.Session.GetString("ErrorMessage");
            if (!string.IsNullOrEmpty(err))
            {
                TempData["ErrorMessage"] = err!;
                HttpContext.Session.Remove("ErrorMessage");
            }
        }

        public async Task<IActionResult> OnPostLoginAsync() 
        {
            var employeeId = await _usersService.LoginAsync(LoginUserModel);

            return RedirectToPage("Personal", new { id = employeeId });
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await _usersService.LogoutAsync();

            return Redirect("Index");
        }
    }
}
