using FluentValidation;
using HCM.Core.Exceptions;
using HCM.Core.Helpers;
using HCM.Core.Mappers;
using HCM.Core.Models.User;
using HCM.Core.Validators;
using HCM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace HCM.Core.Services.Implementations
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersService(IUsersRepository usersRepository, IRolesRepository rolesRepository, IHttpContextAccessor httpContextAccessor)
        {
            _usersRepository = usersRepository;
            _rolesRepository = rolesRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> LoginAsync(LoginUserModel loginUserModel)
        {
            var user = await _usersRepository.GetByUsernameAsync(loginUserModel.Username);

            if (user is null)
                throw new EntityNotFoundException("Username or password is incorrect"); // Not found exception

            var inputPasswordHash = LoginHelper.ComputeSHA256Hash(loginUserModel.Password);

            if (inputPasswordHash != user.Password_Hash)
                throw new InvalidInputDataException("Username or password is incorrect"); // Bad request exception

            var userClaims = LoginHelper.GenerateClaims(user.Id, loginUserModel.Username, user.Role.Name, user.EmployeeId);

            var context = _httpContextAccessor.HttpContext;

            await context.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userClaims);

            return user.EmployeeId;
        }

        public async Task LogoutAsync()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async Task<string?> GetUsernameByEmployeeIdAsync(int employeeId)
        {
            var username = await _usersRepository.GetByEmployeeIdAsync(employeeId);

            return username?.Username;
        }

        public async Task CreateUserAsync(CreateUser model)
        {
            var validator = new CreateUserValidator();

            await validator.ValidateAndThrowAsync(model);

            var user = await _usersRepository.GetByUsernameAsync(model.Username);

            if (user != null)
                throw new InvalidInputDataException("Username is already taken");

            var role = await _rolesRepository.GetByNameAsync(model.Role);
            if (role == null)
                throw new EntityNotFoundException("Role not found");

            var newUser = model.ToUser(role.Id);

            _ = await _usersRepository.CreateUserAsync(newUser);
        }

        public async Task UpdateUserAsync(UpdateUser model)
        {
            var user = await _usersRepository.GetByUsernameAsync(model.Username);

            if (user == null)
                throw new EntityNotFoundException("User not found");

            int roleId = user.RoleId;
            if (user.Role != null)
            {
                var role = await _rolesRepository.GetByNameAsync(model.Role!);

                if (role == null)
                    throw new EntityNotFoundException("Role not found");

                roleId = role.Id;
            }

            if (model.Password != null)
            {
                var newPasswordHash = LoginHelper.ComputeSHA256Hash(model.Password);
                user.Password_Hash = newPasswordHash;
            }
            user.RoleId = roleId;

            await _usersRepository.SaveTrackingChangesAsync();
        }

        public async Task DeleteUserAsync(DeleteUser model)
        {
            var user = await _usersRepository.GetByUsernameAsync(model.Username);

            if (user == null)
                throw new EntityNotFoundException("User not found");

            await _usersRepository.DeleteAsync(user);
        }
    }
}
