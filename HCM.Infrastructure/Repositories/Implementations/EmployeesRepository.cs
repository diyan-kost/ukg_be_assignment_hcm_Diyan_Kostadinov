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

        public async Task<Employee?> GetByIdAsync(int id) 
        {
            var employee = await _dbContext.Employees
                .FirstOrDefaultAsync(e => e.Id == id);
            
            return employee;
        }
    }
}
