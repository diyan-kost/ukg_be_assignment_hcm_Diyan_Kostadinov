using HCM.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure.Repositories.Implementations
{
    public class SalariesRepository : ISalariesRepository
    {
        private readonly HCMContext _dbContext;

        public SalariesRepository(HCMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Salary>> GetSalariesByEmployeeIdAsync(int employeeId)
        {
            var salaries = _dbContext.Salaries
                .Where(s => s.EmployeeId == employeeId)
                .OrderByDescending(s => s.EffectiveDate);

            return salaries;
        }
    }
}
