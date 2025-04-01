using HCM.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure.Repositories.Implementations
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly HCMContext _dbContext;

        public EmployeesRepository(HCMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var employees = _dbContext.Employees
                .Include(e => e.Manager)
                .AsNoTracking();

            return employees;
        }

        public async Task<Employee?> GetByIdAsync(int id, bool asNoTracking = true) 
        {
            Employee? employee = null;

            if (asNoTracking)
            {
                employee = await _dbContext.Employees
                    .Include(e => e.Manager)
                    .Include(e => e.Users)
                    .Include(e => e.Salaries)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == id);
            }

            else
            {
                employee = await _dbContext.Employees
                    .Include(e => e.Manager)
                    .Include(e => e.Users)
                    .Include(e => e.Salaries)
                    .FirstOrDefaultAsync(e => e.Id == id);
            }

            return employee;
        }

        public async Task<IEnumerable<Employee>> GetByManagerIdAsync(int managerId)
        {
            var employees = _dbContext.Employees
                .AsNoTracking()
                .Where(e => e.ManagerId == managerId);

            return employees;
        }

        public async Task<bool> ExistsByPhoneNumberAsync(string phoneNumber)
        {
            return await _dbContext.Employees
                .AnyAsync(e => e.PhoneNumber == phoneNumber);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _dbContext.Employees
                .AnyAsync(e => e.Email == email);
        }

        public async Task<bool> ExistsByNationalIdNumberAsync(string nationalIdNumber)
        {
            return await _dbContext.Employees
                .AnyAsync(e => e.NationalIdNumber == nationalIdNumber);
        }

        public async Task<Employee> AddNewEmployeeAsync(Employee employee)
        {
            await _dbContext.Employees.AddAsync(employee);

            await _dbContext.SaveChangesAsync();

            return employee;
        }

        public async Task DeleteAsync(Employee employee)
        {
            _dbContext.Salaries.RemoveRange(employee.Salaries);

            _dbContext.Users.RemoveRange(employee.Users);

            _dbContext.Employees.Remove(employee);

            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveTrackingChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
