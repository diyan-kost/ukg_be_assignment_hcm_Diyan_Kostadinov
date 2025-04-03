using HCM.Core.Models.User;
using HCM.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCM.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        //[HttpPost]
        public async Task<IActionResult> Login(LoginUserDto loginUserModel)
        {
            //await _usersService.LoginAsync(loginUserModel);

            return View("Index");
        }


    }
}
