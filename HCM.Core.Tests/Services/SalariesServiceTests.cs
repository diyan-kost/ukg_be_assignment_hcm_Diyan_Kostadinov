﻿using FluentValidation;
using HCM.Core.Models.Salary;
using HCM.Core.Services;
using HCM.Core.Services.Implementations;
using HCM.Infrastructure.Entities;
using HCM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace HCM.Core.Tests.Services
{
    public class SalariesServiceTests
    {
        private readonly ISalariesService _salariesService;
        private readonly Mock<ISalariesRepository> _mockSalariesRepository;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

        public SalariesServiceTests()
        {
            _mockSalariesRepository = new Mock<ISalariesRepository>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var mockSession = new Mock<ISession>();
            _mockHttpContextAccessor.Setup(c => c.HttpContext.Session).Returns(mockSession.Object);

            _salariesService = new SalariesService(_mockSalariesRepository.Object, _mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task AddNewSalaryAsync_WhenDataIsValid_ShouldCallRepository()
        {
            // Arrange
            var newSalary = new AddSalaryDto() { Amount = 1000, EffectiveDate = DateTime.Today.AddDays(1), Note="Test", EmployeeId = 1 };

            // Act
            await _salariesService.AddNewSalaryAsync(newSalary);

            // Assert
            _mockSalariesRepository.Verify(r => r.AddNewSalaryAsync(It.IsAny<Salary>()), Times.Once());
        }

        [Fact]
        public async Task AddNewSalaryAsync_WhenEffectiveDateIsInvalid_ShouldThrowException()
        {
            // Arrange
            var newSalary = new AddSalaryDto() { Amount = 1000, EffectiveDate = DateTime.Today.AddDays(-7), Note = "Test", EmployeeId = 1 };

            // Act
            var action = async () => await _salariesService.AddNewSalaryAsync(newSalary);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }

        [Fact]
        public async Task AddNewSalaryAsync_WhenAmountIsInvalid_ShouldThrowException()
        {
            // Arrange
            var newSalary = new AddSalaryDto() { Amount = 0, EffectiveDate = DateTime.Today.AddDays(1), Note = "Test", EmployeeId = 1 };

            // Act
            var action = async () => await _salariesService.AddNewSalaryAsync(newSalary);

            // Assert
            await Assert.ThrowsAsync<ValidationException>(action);
        }
    }
}
