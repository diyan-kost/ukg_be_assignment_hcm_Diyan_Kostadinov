using FluentValidation;
using HCM.Core.Exceptions;
using HCM.Core.Mappers;
using HCM.Core.Models.Employee;
using HCM.Core.Validators;
using HCM.Infrastructure.Entities;
using HCM.Infrastructure.Repositories;

namespace HCM.Core.Services.Implementations
{
    public class EmployeesService : IEmployeesService
    {
        private readonly IEmployeesRepository _employeesRepository;
        private readonly ISalariesRepository _salariesRepository; 
        private readonly IUsersRepository _usersRepository;

        public EmployeesService(IEmployeesRepository employeesRepository, ISalariesRepository salariesRepository, IUsersRepository usersRepository)
        {
            _employeesRepository = employeesRepository;
            _salariesRepository = salariesRepository;
            _usersRepository = usersRepository;
        }

        public async Task<EmployeeDetails> GetEmployeeDetailsById(int id)
        {
            var employee = await _employeesRepository.GetByIdAsync(id);

            if (employee == null)
                throw new EntityNotFoundException("Employee not found");

            var salaries = await _salariesRepository.GetSalariesByEmployeeIdAsync(employee.Id);

            var salaryDetails = salaries.ToSalaryDetails();

            var employeeDetails = employee.ToEmployeeDetails(salaryDetails);

            return employeeDetails;
        }

        public async Task<IEnumerable<EmployeeBasicInfo>> GetEmployeesByManagerId(int managerId)
        {
            var employees = await _employeesRepository.GetByManagerIdAsync(managerId);

            var employeeBasicInfoList = employees.ToEmployeeBasicInfo();

            return employeeBasicInfoList;
        }

        public async Task<IEnumerable<EmployeeBasicInfo>> GetAllEmployees()
        {
            var employees = await _employeesRepository.GetAllAsync();

            var employeeBasicInfoList = employees.ToEmployeeBasicInfo();

            return employeeBasicInfoList;
        }

        public async Task UpdateEmployeeAsync(UpdateEmployeeModel model)
        {
            var validator = new UpdateEmployeeValidator();

            await validator.ValidateAndThrowAsync(model);

            var employee = await _employeesRepository.GetByIdAsync(model.Id, false);

            if (employee == null)
                throw new EntityNotFoundException("Employee not found");

            if (model.PhoneNumber != employee.PhoneNumber)
            {
                var isPhoneNumberTaken = await _employeesRepository.ExistsByPhoneNumber(model.PhoneNumber);
                if (isPhoneNumberTaken)
                    throw new InvalidInputDataException("Phone number is already taken!");
            }

            if (model.Email != employee.Email)
            {
                var isEmailTaken = await _employeesRepository.ExistsByEmail(model.Email);
                if (isEmailTaken)
                    throw new InvalidInputDataException("Email is already taken!");
            }
                
            if (model.NationalIdNumber != employee.NationalIdNumber)
            {
                var isNationalIdTaken = await _employeesRepository.ExistsByNationalIdNumber(model.NationalIdNumber);
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
        }

        public async Task AddNewEmployeeAsync(AddNewEmployeeModel model)
        {
            var validator = new AddNewEmployeeValidator();
            await validator.ValidateAndThrowAsync(model);

            var isPhoneNumberTaken = await _employeesRepository.ExistsByPhoneNumber(model.PhoneNumber);
            if (isPhoneNumberTaken)
                throw new InvalidInputDataException("Phone number is already taken!");

            var isEmailTaken = await _employeesRepository.ExistsByEmail(model.Email);
            if (isEmailTaken)
                throw new InvalidInputDataException("Email is already taken!");

            var isNationalIdTaken = await _employeesRepository.ExistsByNationalIdNumber(model.NationalIdNumber);
            if (isNationalIdTaken)
                throw new InvalidInputDataException("National ID number is already taken!");

            var newEmployee = model.ToEmployee();

            await _employeesRepository.AddNewEmployeeAsync(newEmployee);

            var salary = new Salary() { Amount = model.StartingSalary, EffectiveDate = model.SalaryEffectiveDate, EmployeeId = newEmployee.Id };

            await _salariesRepository.AddNewSalaryAsync(salary);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _employeesRepository.GetByIdAsync(id, false);
            if (employee == null)
                throw new EntityNotFoundException("Employee not found");           

            await _employeesRepository.DeleteAsync(employee);
        }
    }
}
