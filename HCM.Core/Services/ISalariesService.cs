using HCM.Core.Models.Salary;

namespace HCM.Core.Services
{
    public interface ISalariesService
    {
        Task AddNewSalaryAsync(AddSalaryDto model);
    }
}
