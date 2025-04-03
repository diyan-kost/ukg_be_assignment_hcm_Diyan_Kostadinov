using FluentValidation;
using HCM.Core.Exceptions;
using HCM.Core.Models.Employee;
using HCM.Core.Models.User;
using HCM.Core.Services;
using HCM.Core.Services.Implementations;
using HCM.Infrastructure.Entities;
using HCM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace HCM.Core.Tests.Services
{
    public class EmployeeServiceTests
    {
        private readonly Mock<IEmployeesRepository> _mockEmployeesRepository;
        private readonly Mock<ISalariesRepository> _mockSalariesRepository;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

        private readonly IEmployeesService _employeesService;

        public EmployeeServiceTests()
        {
            _mockEmployeesRepository = new Mock<IEmployeesRepository>();
            _mockSalariesRepository = new Mock<ISalariesRepository>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            _employeesService = new EmployeesService(_mockEmployeesRepository.Object, _mockSalariesRepository.Object, _mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task GetEmployeeDetailsByIdAsync_WhenEmployeeNotFound_ShouldThrowException()
        {
            // Act
            var action = async () => await _employeesService.GetEmployeeDetailsByIdAsync(1);

            // Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(action);
        }

        [Fact]
        public async Task GetEmployeeDetailsByIdAsync_WhenEmployeeFound_ShouldReturnData()
        {
            //Arrange
            var mockEmployee = CreateMockEmployee(1);

            var mockSalary = new Salary() { Id = 2, Amount = 1000, EffectiveDate = DateTime.Today.AddDays(1), Employee = mockEmployee, EmployeeId = mockEmployee.Id, Note = "Starting Salary" };

            var mockSalaryList = new List<Salary>() { mockSalary };

            _mockEmployeesRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(mockEmployee);
            _mockSalariesRepository.Setup(r => r.GetSalariesByEmployeeIdAsync(It.IsAny<int>())).ReturnsAsync(mockSalaryList);

            // Act
            var details = await _employeesService.GetEmployeeDetailsByIdAsync(1);

            // Assert
            Assert.NotNull(details);
            Assert.Equal(mockSalaryList.Count, details.Salaries.Count);
        }

        [Fact]
        public async Task GetEmployeesByManagerIdAsync_WhenNoEmployeesFound_ShouldReturnEmptyCollection()
        {
            // Act
            var result = await _employeesService.GetEmployeesByManagerIdAsync(1);

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetEmployeesByManagerIdAsync_WhenEmployeesFound_ShouldReturnCollectionWithElements()
        {
            // Arrange
            var mockEmployeeManager = CreateMockEmployee(1);

            var mockEmployee = CreateMockEmployee(1, mockEmployeeManager);

            var mockList = new List<Employee>() { mockEmployee };

            _mockEmployeesRepository.Setup(r => r.GetByManagerIdAsync(It.IsAny<int>())).ReturnsAsync(mockList);

            // Act
            var result = await _employeesService.GetEmployeesByManagerIdAsync(mockEmployee.ManagerId.Value);

            // Assert
            Assert.Equal(mockList.Count, result.Count());
        }

        [Fact]
        public async Task GetAllEmployeesAsync_WhenNoEmployeesFound_ShouldReturnEmptyCollection()
        {
            // Act
            var result = await _employeesService.GetAllEmployeesAsync();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllEmployeesAsync_WhenHasEmployees_ShouldReturnCollectionWithElements()
        {
            // Arrange
            var mockEmployee1 = CreateMockEmployee(1);
            var mockEmployee2 = CreateMockEmployee(2);


            var mockListEmployees = new List<Employee>() { mockEmployee1, mockEmployee2 };

            for (var i = 0; i < 10; i++)
            {
                if (i < 6)
                {
                    mockListEmployees.Add(CreateMockEmployee(i + 3, mockEmployee1));
                }
                else
                {
                    mockListEmployees.Add(CreateMockEmployee(i + 3, mockEmployee2));
                }
            }

            _mockEmployeesRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(mockListEmployees);

            // Act
            var result = await _employeesService.GetAllEmployeesAsync();

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal(mockListEmployees.Count, result.Count());

        }

        [Theory]
        [MemberData(nameof(MockDataTestHelper.InvalidNames), MemberType = typeof(MockDataTestHelper))]
        public async Task UpdateEmployeeAsync_WhenFirstNameIsInvalid_ShouldThrowException(string invalidName)
        {
            // Arrange
            var updateModel = CreateUpdateModel(1);
            updateModel.FirstName = invalidName;

            // Act
            var action = async () => await _employeesService.UpdateEmployeeAsync(updateModel);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);

        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenPhoneNumberIsInvalid_ShouldThrowException()
        {
            // Arrange
            var updateModel = CreateUpdateModel(1);
            updateModel.PhoneNumber = "test";

            // Act
            var action = async () => await _employeesService.UpdateEmployeeAsync(updateModel);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenEmailIsInvalid_ShouldThrowException()
        {
            // Arrange
            var updateModel = CreateUpdateModel(1);
            updateModel.Email = "test";

            // Act
            var action = async () => await _employeesService.UpdateEmployeeAsync(updateModel);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Theory]
        [MemberData(nameof(MockDataTestHelper.InvalidAddresses), MemberType = typeof(MockDataTestHelper))]
        public async Task UpdateEmployeeAsync_WhenAddressIsInvalid_ShouldThrowException(string invalidAddress)
        {
            // Arrange
            var updateModel = CreateUpdateModel(1);
            updateModel.Address = invalidAddress;

            // Act
            var action = async () => await _employeesService.UpdateEmployeeAsync(updateModel);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Theory]
        [MemberData(nameof(MockDataTestHelper.InvalidNationalIds), MemberType = typeof(MockDataTestHelper))]
        public async Task UpdateEmployeeAsync_WhenNationalIdIsInvalid_ShouldThrowException(string invalidNationalId)
        {
            // Arrange
            var updateModel = CreateUpdateModel(1);
            updateModel.NationalIdNumber = invalidNationalId;

            // Act
            var action = async () => await _employeesService.UpdateEmployeeAsync(updateModel);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenPhoneNumberIsTaken_ShouldThrowException()
        {
            // Arrange
            var updateModel = CreateUpdateModel(1);
            var mockUser = CreateMockEmployee(1);


            _mockEmployeesRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(mockUser);
            _mockEmployeesRepository.Setup(r => r.ExistsByPhoneNumberAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var action = async () => await _employeesService.UpdateEmployeeAsync(updateModel);

            // Assert
            await Assert.ThrowsAsync<InvalidInputDataException>(action);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenEmailIsTaken_ShouldThrowException()
        {
            // Arrange
            var updateModel = CreateUpdateModel(1);
            var mockUser = CreateMockEmployee(1);


            _mockEmployeesRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(mockUser);
            _mockEmployeesRepository.Setup(r => r.ExistsByPhoneNumberAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockEmployeesRepository.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var action = async () => await _employeesService.UpdateEmployeeAsync(updateModel);

            // Assert
            await Assert.ThrowsAsync<InvalidInputDataException>(action);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenNationalIdNumberIsTaken_ShouldThrowException()
        {
            // Arrange
            var updateModel = CreateUpdateModel(1);
            var mockUser = CreateMockEmployee(1);


            _mockEmployeesRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(mockUser);
            _mockEmployeesRepository.Setup(r => r.ExistsByPhoneNumberAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockEmployeesRepository.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockEmployeesRepository.Setup(r => r.ExistsByNationalIdNumberAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var action = async () => await _employeesService.UpdateEmployeeAsync(updateModel);

            // Assert
            await Assert.ThrowsAsync<InvalidInputDataException>(action);
        }

        [Fact]
        public async Task UpdateEmployeeAsync_WhenDataIsCorrect_ShouldCallRepository()
        {
            // Arrange
            var updateModel = CreateUpdateModel(1);
            var mockUser = CreateMockEmployee(1);


            _mockEmployeesRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(mockUser);
            _mockEmployeesRepository.Setup(r => r.ExistsByPhoneNumberAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockEmployeesRepository.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockEmployeesRepository.Setup(r => r.ExistsByNationalIdNumberAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            await _employeesService.UpdateEmployeeAsync(updateModel);

            // Assert
            _mockEmployeesRepository.Verify(r => r.SaveTrackingChangesAsync(), Times.Once);
        }

        [Theory]
        [MemberData(nameof(MockDataTestHelper.InvalidNames), MemberType = typeof(MockDataTestHelper))]
        public async Task AddNewEmployeeAsync_WhenNameIsInvalid_ShouldThrowException(string invalidName)
        {
            // Arrange
            var addModel = CreateAddNewEmployeeModel();
            addModel.FirstName = invalidName;

            // Act
            var action = async () => await _employeesService.AddNewEmployeeAsync(addModel);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Fact]
        public async Task AddNewEmployeeAsync_WhenPhoneNumberIsInvalid_ShouldThrowException()
        {
            // Arrange
            var addModel = CreateAddNewEmployeeModel();
            addModel.PhoneNumber = "test";

            // Act
            var action = async () => await _employeesService.AddNewEmployeeAsync(addModel);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Fact]
        public async Task AddNewEmployeeAsync_WhenEmailIsInvalid_ShouldThrowException()
        {
            // Arrange
            var addModel = CreateAddNewEmployeeModel();
            addModel.Email = "test";

            // Act
            var action = async () => await _employeesService.AddNewEmployeeAsync(addModel);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Theory]
        [MemberData(nameof(MockDataTestHelper.InvalidAddresses), MemberType = typeof(MockDataTestHelper))]
        public async Task AddNewEmployeeAsync_WhenAddressIsInvalid_ShouldThrowException(string invalidAddress)
        {
            // Arrange
            var addModel = CreateAddNewEmployeeModel();
            addModel.Address = invalidAddress;

            // Act
            var action = async () => await _employeesService.AddNewEmployeeAsync(addModel);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Theory]
        [MemberData(nameof(MockDataTestHelper.InvalidNationalIds), MemberType = typeof(MockDataTestHelper))]
        public async Task AddNewEmployeeAsync_WhenNationalIdIsInvalid_ShouldThrowException(string invalidNationalId)
        {
            // Arrange
            var addModel = CreateAddNewEmployeeModel();
            addModel.NationalIdNumber = invalidNationalId;

            // Act
            var action = async () => await _employeesService.AddNewEmployeeAsync(addModel);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Fact]
        public async Task AddNewEmployeeAsync_WhenPhoneNumberIsTaken_ShouldThrowException()
        {
            // Arrange
            var addModel = CreateAddNewEmployeeModel();

            _mockEmployeesRepository.Setup(r => r.ExistsByPhoneNumberAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var action = async () => await _employeesService.AddNewEmployeeAsync(addModel);

            // Assert
            await Assert.ThrowsAsync<InvalidInputDataException>(action);
        }

        [Fact]
        public async Task AddNewEmployeeAsync_WhenEmailIsTaken_ShouldThrowException()
        {
            // Arrange
            var addModel = CreateAddNewEmployeeModel();

            _mockEmployeesRepository.Setup(r => r.ExistsByPhoneNumberAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockEmployeesRepository.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var action = async () => await _employeesService.AddNewEmployeeAsync(addModel);

            // Assert
            await Assert.ThrowsAsync<InvalidInputDataException>(action);
        }

        [Fact]
        public async Task AddNewEmployeeAsync_WhenNationalIdNumberIsTaken_ShouldThrowException()
        {
            // Arrange
            var addModel = CreateAddNewEmployeeModel();

            _mockEmployeesRepository.Setup(r => r.ExistsByPhoneNumberAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockEmployeesRepository.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockEmployeesRepository.Setup(r => r.ExistsByNationalIdNumberAsync(It.IsAny<string>())).ReturnsAsync(true);

            // Act
            var action = async () => await _employeesService.AddNewEmployeeAsync(addModel);

            // Assert
            await Assert.ThrowsAsync<InvalidInputDataException>(action);
        }

        [Fact]
        public async Task AddNewEmployeeAsync_WhenDataIsCorrect_ShouldCallRepository()
        {
            // Arrange
            var addModel = CreateAddNewEmployeeModel();

            _mockEmployeesRepository.Setup(r => r.ExistsByPhoneNumberAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockEmployeesRepository.Setup(r => r.ExistsByEmailAsync(It.IsAny<string>())).ReturnsAsync(false);
            _mockEmployeesRepository.Setup(r => r.ExistsByNationalIdNumberAsync(It.IsAny<string>())).ReturnsAsync(false);

            // Act
            await _employeesService.AddNewEmployeeAsync(addModel);

            // Assert
            _mockEmployeesRepository.Verify(r => r.AddNewEmployeeAsync(It.IsAny<Employee>()), Times.Once);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_WhenEmployeeNotFound_ShouldThrowException()
        {
            // Act
            var action = async() => await _employeesService.DeleteEmployeeAsync(1);

            // Assert
            await Assert.ThrowsAsync<EntityNotFoundException>(action);
        }

        [Fact]
        public async Task DeleteEmployeeAsync_WhenDataIsCorrect_ShouldCallRepository()
        {
            // Arrange
            var mockEmployee = CreateMockEmployee(1);

            _mockEmployeesRepository.Setup(r => r.GetByIdAsync(It.IsAny<int>(), It.IsAny<bool>())).ReturnsAsync(mockEmployee);

            // Act
            await _employeesService.DeleteEmployeeAsync(mockEmployee.Id);

            // Assert
            _mockEmployeesRepository.Verify(r => r.DeleteAsync(It.IsAny<Employee>()), Times.Once);
        }

        private AddNewEmployeeDto CreateAddNewEmployeeModel(int? managerId = null)
        {
            var model = new AddNewEmployeeDto()
            {
                FirstName = "John",
                MiddleName = "Johnson",
                LastName = "Doe",
                Email = $"john.doe@test.com",
                PhoneNumber = $"1456654165",
                Address = "Lorem Ipsum",
                NationalIdNumber = $"2537815740",
                DateOfBirth = DateTime.Today.AddYears(-30),
                HiredAt = DateTime.Today.AddDays(1),
                ManagerId = managerId,
                IsEuCitizen = false,
                Gender = "Male",
                StartingSalary = 1000,
                SalaryEffectiveDate = DateTime.Today.AddDays(1)
            };

            return model;
        }

        private UpdateEmployeeDto CreateUpdateModel(int id, int? managerId = null)
        {
            var model = new UpdateEmployeeDto()
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = $"john.doe@test.com",
                PhoneNumber = $"1456654165",
                Address = "Lorem Ipsum",
                NationalIdNumber = $"2537815740",
                ManagerId = managerId,
            };

            return model;
        }

        private Employee CreateMockEmployee(int id, Employee? manager = null)
        {
            var mockEmployee = new Employee()
            {
                Id = id,
                FirstName = "John",
                LastName = "Doe",
                Email = $"john.doe{id}@test.com",
                PhoneNumber = $"1456654165{id}",
                CurrentAddress = "Lorem Ipsum",
                IsEuCitizen = false,
                NationalIdNumber = $"2537815740{id}",
                DateOfBirth = DateTime.Today.AddYears(-30),
                HiredAt = DateTime.Today.AddDays(1),
                Gender = "Male",
                Manager = manager,
                ManagerId = manager?.Id
            };

            return mockEmployee;
        }
    }
}
