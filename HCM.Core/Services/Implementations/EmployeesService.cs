using FluentValidation;
using HCM.Core.Common;
using HCM.Core.Exceptions;
using HCM.Core.Mappers;
using HCM.Core.Models.Employee;
using HCM.Core.Validators;
using HCM.Infrastructure.Entities;
using HCM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;

namespace HCM.Core.Services.Implementations
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly ISalariesRepository _salariesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeesService(IEmployeesRepository employeesRepository, ISalariesRepository salariesRepository, IHttpContextAccessor httpContextAccessor)
        {
            _employeesRepository = employeesRepository;
            _salariesRepository = salariesRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<EmployeeDetailsDto> GetEmployeeDetailsByIdAsync(int id)
        {
            var employee = await _employeesRepository.GetByIdAsync(id);

            if (employee == null)
                throw new EntityNotFoundException("Employee not found");

            var salaries = await _salariesRepository.GetSalariesByEmployeeIdAsync(employee.Id);

            var salaryDetails = salaries.ToSalaryDetails();

            var employeeDetails = employee.ToEmployeeDetails(salaryDetails);

            return employeeDetails;
        }

        public async Task<IEnumerable<EmployeeInfoDto>> GetEmployeesByManagerIdAsync(int managerId)
        {
            var employees = await _employeesRepository.GetByManagerIdAsync(managerId);

            var employeeBasicInfoList = employees.ToEmployeeBasicInfo();

            return employeeBasicInfoList;
        }

        public async Task<IEnumerable<EmployeeInfoDto>> GetAllEmployeesAsync()
        {
            var employees = await _employeesRepository.GetAllAsync();

            var employeeBasicInfoList = employees.ToEmployeeBasicInfo();

            return employeeBasicInfoList;
        }

        public async Task UpdateEmployeeAsync(UpdateEmployeeDto model)
        {
            var validator = new UpdateEmployeeValidator();

            await validator.ValidateAndThrowAsync(model);

            var employee = await _employeesRepository.GetByIdAsync(model.Id, false);

            if (employee == null)
                throw new EntityNotFoundException("Employee not found");

            if (model.PhoneNumber != employee.PhoneNumber)
            {
                var isPhoneNumberTaken = await _employeesRepository.ExistsByPhoneNumberAsync(model.PhoneNumber);
                if (isPhoneNumberTaken)
                    throw new InvalidInputDataException("Phone number is already taken!");
            }

            if (model.Email != employee.Email)
            {
                var isEmailTaken = await _employeesRepository.ExistsByEmailAsync(model.Email);
                if (isEmailTaken)
                    throw new InvalidInputDataException("Email is already taken!");
            }
                
            if (model.NationalIdNumber != employee.NationalIdNumber)
            {
                var isNationalIdTaken = await _employeesRepository.ExistsByNationalIdNumberAsync(model.NationalIdNumber);
                if (isNationalIdTaken)
                    throw new InvalidInputDataException("National ID number is already taken!");
            }

            employee.FirstName = model.FirstName;
            employee.MiddleName = model.MiddleName;
            employee.LastName = model.LastName;
            employee.ManagerId = model.ManagerId;
            employee.Email = model.Email;
            employee.PhoneNumber = model.PhoneNumber;
            employee.CurrentAddress = model.Address;
            employee.NationalIdNumber = model.NationalIdNumber;

            await _employeesRepository.SaveTrackingChangesAsync();

            _httpContextAccessor.HttpContext.Session.SetString(MessageTypes.SUCCESS, "Employee updated successfully");
        }

        public async Task AddNewEmployeeAsync(AddNewEmployeeDto model)
        {
            var validator = new AddNewEmployeeValidator();
            await validator.ValidateAndThrowAsync(model);

            var isPhoneNumberTaken = await _employeesRepository.ExistsByPhoneNumberAsync(model.PhoneNumber);
            if (isPhoneNumberTaken)
                throw new InvalidInputDataException("Phone number is already taken!");

            var isEmailTaken = await _employeesRepository.ExistsByEmailAsync(model.Email);
            if (isEmailTaken)
                throw new InvalidInputDataException("Email is already taken!");

            var isNationalIdTaken = await _employeesRepository.ExistsByNationalIdNumberAsync(model.NationalIdNumber);
            if (isNationalIdTaken)
                throw new InvalidInputDataException("National ID number is already taken!");

            var newEmployee = model.ToEmployee();

            await _employeesRepository.AddNewEmployeeAsync(newEmployee);

            var salary = new Salary() { Amount = model.StartingSalary, EffectiveDate = model.SalaryEffectiveDate, EmployeeId = newEmployee.Id };

            await _salariesRepository.AddNewSalaryAsync(salary);

            _httpContextAccessor.HttpContext.Session.SetString(MessageTypes.SUCCESS, "Employee created successfully");
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _employeesRepository.GetByIdAsync(id, false);

            if (employee == null)
                throw new EntityNotFoundException("Employee not found");           

            await _employeesRepository.DeleteAsync(employee);

            _httpContextAccessor.HttpContext.Session.SetString(MessageTypes.SUCCESS, "Employee deleted successfully");
        }
    }
}
