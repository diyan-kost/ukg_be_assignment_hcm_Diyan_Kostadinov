using FluentValidation;
using HCM.Core.Exceptions;
using HCM.Core.Helpers;
using HCM.Core.Models.User;
using HCM.Core.Services;
using HCM.Core.Services.Implementations;
using HCM.Infrastructure.Entities;
using HCM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace HCM.Core.Tests.Services
{
    public class UsersServiceTests
    {
        private readonly Mock<IUsersRepository> _mockUsersRepository;
        private readonly Mock<IRolesRepository> _mockRolesRepository;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

        private readonly IUsersService _usersService;

        public UsersServiceTests()
        {
            _mockUsersRepository = new Mock<IUsersRepository>();
            _mockRolesRepository = new Mock<IRolesRepository>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var mockSession = new Mock<ISession>();
            _mockHttpContextAccessor.Setup(c => c.HttpContext.Session).Returns(mockSession.Object);

            _usersService = new UsersService(_mockUsersRepository.Object, _mockRolesRepository.Object, _mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task LoginAsync_WhenDataIsValid_ShouldLogReturnEmployeeId()
        {
            // Arrange
            var loginModel = new LoginUserDto()
            {
                Username = "test",
                Password = "Test1"
            };

            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(_ => _.SignInAsync(It.IsAny<HttpContext>(), It.IsAny<string>(), It.IsAny<ClaimsPrincipal>(), It.IsAny<AuthenticationProperties>()))
                .Returns(Task.FromResult((object)null));

            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(_ => _.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);

            _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(new DefaultHttpContext() { RequestServices = serviceProviderMock.Object });

            var mockUser = new User() { Id = 1, Username = loginModel.Username, Password_Hash = LoginHelper.ComputeSHA256Hash(loginModel.Password), 
                EmployeeId = 1, RoleId = 1, Role = new Role() { Id = 1, Name = "Test" } };

            _mockUsersRepository.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(mockUser);

            // Act
            var employeeId = await _usersService.LoginAsync(loginModel);

            // Assert
            Assert.Equal(mockUser.EmployeeId, employeeId);
        }

        [Fact]
        public async Task LoginAsync_WhenUserNotFound_ShouldThrowException()
        {
            // Arrange
            var loginModel = new LoginUserDto()
            {
                Username = "test",
                Password = "Test1"
            };

            // Act
            var action = async () => await _usersService.LoginAsync(loginModel);

            // Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(action);
        }

        [Theory]
        [MemberData(nameof(MockDataTestHelper.InvalidUsernames), MemberType = typeof(MockDataTestHelper))]
        public async Task LoginAsync_WhenUsernameIsInvalid_ShouldThrowException(string username)
        {
            // Arrange
            var loginModel = new LoginUserDto()
            {
                Username = username,
                Password = "test"
            };

            // Act
            var action = async () => await _usersService.LoginAsync(loginModel);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordIsNotSet_ShouldThrowException()
        {
            // Arrange
            var loginModel = new LoginUserDto()
            {
                Username = "test",
                Password = ""
            };

            // Act
            var action = async () => await _usersService.LoginAsync(loginModel);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Fact]
        public async Task LoginAsync_PasswordIsIncorrect_ShouldThrowException()
        {
            // Arrange
            var loginModel = new LoginUserDto()
            {
                Username = "test",
                Password = "Test1"
            };

            var mockUser = new User() { Id = 1, Username = loginModel.Username, Password_Hash = LoginHelper.ComputeSHA256Hash("g2sdf1f"), EmployeeId = 1 };

            _mockUsersRepository.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(mockUser);

            // Act
            var action = async () => await _usersService.LoginAsync(loginModel);

            // Assert
            await Assert.ThrowsAsync<InvalidInputDataException>(action);
        }

        [Fact]
        public async Task GetUsernameByEmployeeIdAsync_WhenUsernameIsFound_ShouldReturnUsername()
        {
            //Arrange
            var mockUser = new User() { Id = 1, Username = "test", EmployeeId = 123 };

            _mockUsersRepository.Setup(r => r.GetByEmployeeIdAsync(It.IsAny<int>())).ReturnsAsync(mockUser);

            // Act
            var foundUsername = await _usersService.GetUsernameByEmployeeIdAsync(mockUser.EmployeeId);

            // Assert
            Assert.Equal(mockUser.Username, foundUsername);
        }

        [Fact]
        public async Task GetUsernameByEmployeeIdAsync_WhenUsernameIsNotFound_ShouldReturnNull()
        {
            // Act
            var foundUsername = await _usersService.GetUsernameByEmployeeIdAsync(1);

            // Assert
            Assert.Null(foundUsername);
        }

        [Fact]
        public async Task CreateUserAsync_WhenDataIsCorrect_ShouldCallRepository()
        {
            // Arrange
            var input = new CreateUserDto()
            {
                Username = "test",
                Password = "Test1",
                Role = "test"
            };

            var mockRole = new Role() { Id = 13, Name = input.Role };

            _mockRolesRepository.Setup(r => r.GetByNameAsync(It.IsAny<string>())).ReturnsAsync(mockRole);

            // Act
            await _usersService.CreateUserAsync(input);

            // Assert
            _mockUsersRepository.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(MockDataTestHelper.InvalidUsernames), MemberType = typeof(MockDataTestHelper))]
        public async Task CreateUserAsync_WhenUsernameIsInvalid_ShouldThrowException(string username)
        {
            // Arrange
            var input = new CreateUserDto()
            {
                Username = username,
                Password = "test",
                //...
            };

            // Act
            var action = async() => await _usersService.CreateUserAsync(input);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Fact]
        public async Task CreateUserAsync_WhenUsernameIsTaken_ShouldThrowException()
        {
            // Arrange
            var input = new CreateUserDto()
            {
                Username = "test",
                Password = "Test123",
                Role = "test"
                //...
            };

            var mockUser = new User() { Id = 1, Username = input.Username, Password_Hash = LoginHelper.ComputeSHA256Hash(input.Password), EmployeeId = 123, RoleId = 13, Role = new Role() { Id = 13, Name = "test" } };

            _mockUsersRepository.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(mockUser);

            // Act
            var action = async () => await _usersService.CreateUserAsync(input);

            // Assert
            await Assert.ThrowsAsync<InvalidInputDataException>(action);
        }

        [Theory]
        [MemberData(nameof(MockDataTestHelper.InvalidPasswords), MemberType = typeof(MockDataTestHelper))]
        public async Task CreateUserAsync_WhenPasswordIsInvalid_ShouldThrowException(string password)
        {
            // Arrange
            var input = new CreateUserDto()
            {
                Username = "test",
                Password = password,
                //...
            };

            // Act
            var action = async () => await _usersService.CreateUserAsync(input);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Fact]
        public async Task CreateUserAsync_WhenRoleNotSet_ShouldThrowException()
        {
            // Arrange
            var input = new CreateUserDto()
            {
                Username = "test",
                Password = "Test1",
                //...
            };

            // Act
            var action = async () => await _usersService.CreateUserAsync(input);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Fact]
        public async Task CreateUserAsync_WhenRoleIsNotFound_ShouldThrowException()
        {
            // Arrange
            var input = new CreateUserDto()
            {
                Username = "test",
                Password = "Test1",
                Role = "test"
            };

            // Act
            var action = async () => await _usersService.CreateUserAsync(input);

            // Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(action);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenPasswordIsNotValid_ShouldThrowException()
        {
            // Arrange
            var input = new UpdateUserDto()
            {
                Username = "test",
                Password = "te",
                //...
            };

            // Act
            var action = async () => await _usersService.UpdateUserAsync(input);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserNotFound_ShouldThrowException()
        {
            // Arrange
            var input = new UpdateUserDto()
            {
                Username = "test",
                Password = "Test123",
                //...
            };

            // Act
            var action = async () => await _usersService.UpdateUserAsync(input);

            // Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(action);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenRoleIsSetAndNotFound_ShouldThrowException()
        {
            // Arrange
            var input = new UpdateUserDto()
            {
                Username = "test",
                Password = "Test123",
                Role = 1
                //...
            };

            var mockUser = new User() { Id = 1, Username = input.Username };

            _mockUsersRepository.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(mockUser);

            // Act
            var action = async () => await _usersService.UpdateUserAsync(input);

            // Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(action);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenRoleIsSet_ShouldCallRepository()
        {
            // Arrange
            var input = new UpdateUserDto()
            {
                Username = "test",
                Role = 2,
                EmployeeId = 99
                //...
            };

            var mockUser = new User() { Id = 1, Username = input.Username, Password_Hash = LoginHelper.ComputeSHA256Hash("1"), 
                Role = new Role() { Id = 1, Name = "Test" }, RoleId = 1, EmployeeId = input.EmployeeId };

            var mockRole = new Role() { Id = input.Role.Value, Name = "New Role" };

            _mockUsersRepository.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(mockUser);
            _mockRolesRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(mockRole);

            // Act
            await _usersService.UpdateUserAsync(input);

            // Assert
            _mockUsersRepository.Verify(r => r.SaveTrackingChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenPasswordIsSet_ShouldCallRepository()
        {
            // Arrange
            var input = new UpdateUserDto()
            {
                Username = "test",
                Password = "Test123",
                //...
            };

            var mockUser = new User()
            {
                Id = 1,
                Username = input.Username,
                Password_Hash = LoginHelper.ComputeSHA256Hash("1"),
                Role = new Role() { Id = 1, Name = "Test" },
                RoleId = 1,
                EmployeeId = input.EmployeeId
            };

            _mockUsersRepository.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(mockUser);

            // Act
            await _usersService.UpdateUserAsync(input);

            // Assert
            _mockUsersRepository.Verify(r => r.SaveTrackingChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteUserAsync_WhenUserNotFound_ShouldThrowException()
        {
            // Act
            var action = async() => await _usersService.DeleteUserAsync("test");

            // Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(action);
        }

        [Fact]
        public async Task DeleteUserAsync_WhenUserFound_ShouldCallDelete()
        {
            // Arrange
            var mockUser = new User() { Id = 1, Username = "test", Password_Hash = LoginHelper.ComputeSHA256Hash("!") };

            _mockUsersRepository.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(mockUser);

            // Act
            await _usersService.DeleteUserAsync(mockUser.Username);

            // Assert
            _mockUsersRepository.Verify(r => r.DeleteAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task GetUserRoleByUsernameAsync_WhenUserNotFound_ShouldThrowException()
        {
            // Act
            var action = async () => await _usersService.GetUserRoleByUsernameAsync("test");

            // Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(action);
        }

        [Fact]
        public async Task GetUserRoleByUsernameAsync_WhenUserFound_ShouldReturnRoleName()
        {
            // Arrange
            var mockRole = new Role() { Id = 3, Name = "Test Role" };
            var mockUser = new User() { Id = 1, Username = "test", Role = mockRole, RoleId = mockRole.Id };

            _mockUsersRepository.Setup(r => r.GetByUsernameAsync(It.IsAny<string>())).ReturnsAsync(mockUser);

            // Act
            var result = await _usersService.GetUserRoleByUsernameAsync(mockUser.Username);

            // Assert
            Assert.Equal(mockRole.Name, result);
        }
    }
}
