using HCM.Infrastructure.Entities;

namespace HCM.Infrastructure.Repositories
{
    public interface IEmployeesRepository
    {
        Task<Employee?> GetByIdAsync(int id, bool asNoTracking = true);

        Task<IEnumerable<Employee>> GetByManagerIdAsync(int managerId);

        Task<IEnumerable<Employee>> GetAllAsync();

        Task<bool> ExistsByPhoneNumber(string phoneNumber);

        Task<bool> ExistsByEmail(string email);

        Task<bool> ExistsByNationalIdNumber(string nationalIdNumber);

        Task<Employee> AddNewEmployeeAsync(Employee employee);

        Task SaveTrackingChangesAsync();
    }
}
