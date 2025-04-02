using HCM.Core.Services;
using HCM.Core.Services.Implementations;
using HCM.Infrastructure.Entities;
using HCM.Infrastructure.Repositories;
using Moq;
using Xunit;

namespace HCM.Core.Tests.Services
{
    public class RolesServiceTests
    {
        private readonly Mock<IRolesRepository> _mockRolesRepository;
        private readonly IRolesService _rolesService;

        public RolesServiceTests()
        {
            _mockRolesRepository = new();

            _rolesService = new RolesService(_mockRolesRepository.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnRoles()
        {
            //Arrange
            var mockRoles = new List<Role>()
            {
                { new Role() { Id = 1, Name = "Test", Description = "Test Role"} },
                { new Role() { Id = 1, Name = "Admin", Description = "Admin Test Role"} },
            };

            var mockRolesCount = mockRoles.Count;

            _mockRolesRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(mockRoles);

            // Act
            var roles = await _rolesService.GetRolesAsync();

            // Assert
            Assert.NotEmpty(roles);
            Assert.Equal(roles.Count(), mockRolesCount);
        }
    }
}
