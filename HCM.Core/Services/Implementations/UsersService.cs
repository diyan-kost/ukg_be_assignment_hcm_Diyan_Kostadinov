using FluentValidation;
using HCM.Core.Common;
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

        public async Task<int> LoginAsync(LoginUserDto loginUserModel)
        {
            var validator = new LoginUserModelValidator();
            await validator.ValidateAndThrowAsync(loginUserModel);

            var user = await _usersRepository.GetByUsernameAsync(loginUserModel.Username);

            if (user is null)
                throw new EntityNotFoundException("Username or password is incorrect");

            var inputPasswordHash = LoginHelper.ComputeSHA256Hash(loginUserModel.Password);

            if (inputPasswordHash != user.Password_Hash)
                throw new InvalidInputDataException("Username or password is incorrect");

            var userClaims = LoginHelper.GenerateClaims(user.Id, loginUserModel.Username, user.Role.Name, user.EmployeeId);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userClaims);

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

        public async Task CreateUserAsync(CreateUserDto model)
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

            _httpContextAccessor.HttpContext.Session.SetString(MessageTypes.SUCCESS, "User created successfully");
        }

        public async Task UpdateUserAsync(UpdateUserDto model)
        {
            var validator = new UpdateUserValidator();
            await validator.ValidateAndThrowAsync(model);

            var user = await _usersRepository.GetByUsernameAsync(model.Username);

            if (user == null)
                throw new EntityNotFoundException("User not found");

            if (model.Role != null)
            {
                var role = await _rolesRepository.GetByIdAsync(model.Role.Value);

                if (role == null)
                    throw new EntityNotFoundException("Role not found");

                user.RoleId = role.Id;
            }

            if (model.Password != null)
            {
                var newPasswordHash = LoginHelper.ComputeSHA256Hash(model.Password);
                user.Password_Hash = newPasswordHash;
            }
            
            if (model.Role != null || model.Password != null)
            {
                await _usersRepository.SaveTrackingChangesAsync();

                _httpContextAccessor.HttpContext.Session.SetString(MessageTypes.SUCCESS, "User updated successfully");
            }
        }

        public async Task DeleteUserAsync(string username)
        {
            var user = await _usersRepository.GetByUsernameAsync(username);

            if (user == null)
                throw new EntityNotFoundException("User not found");

            await _usersRepository.DeleteAsync(user);

            _httpContextAccessor.HttpContext.Session.SetString(MessageTypes.SUCCESS, "User deleted successfully");
        }

        public async Task<string> GetUserRoleByUsernameAsync(string username)
        {
            var user = await _usersRepository.GetByUsernameAsync(username);

            if (user == null)
                throw new EntityNotFoundException("User not found");

            return user.Role.Name;
        }
    }
}
