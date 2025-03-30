using HCM.Core.Helpers;
using HCM.Core.Models.User;
using HCM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace HCM.Core.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersService(IUsersRepository usersRepository, IHttpContextAccessor httpContextAccessor)
        {
            _usersRepository = usersRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task LoginAsync(LoginUserModel loginUserModel)
        {
            var user = await _usersRepository.GetByUsernameAsync(loginUserModel.Username);

            if (user is null)
                throw new Exception("Username or password is incorrect"); // Not found exception

            var inputPasswordHash = LoginHelper.ComputeSHA256Hash(loginUserModel.Password);

            if (inputPasswordHash != user.Password_Hash)
                throw new Exception("Username or password is incorrect"); // Bad request exception

            var userClaims = LoginHelper.GenerateClaims(user.Id, loginUserModel.Username, user.Role.Name);

            var context = _httpContextAccessor.HttpContext;

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userClaims);
        }

        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }
    }
}
