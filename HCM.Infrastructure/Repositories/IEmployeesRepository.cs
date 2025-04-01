using HCM.Infrastructure.Entities;

namespace HCM.Infrastructure.Repositories
{
    public interface IEmployeesRepository
    {
        Task<Employee?> GetByIdAsync(int id, bool asNoTracking = true);

        Task<IEnumerable<Employee>> GetByManagerIdAsync(int managerId);

        Task<IEnumerable<Employee>> GetAllAsync();

        Task<bool> ExistsByPhoneNumberAsync(string phoneNumber);

        Task<bool> ExistsByEmailAsync(string email);

        Task<bool> ExistsByNationalIdNumberAsync(string nationalIdNumber);

        Task<Employee> AddNewEmployeeAsync(Employee employee);

        Task SaveTrackingChangesAsync();

        Task DeleteAsync(Employee employee);
    }
}
