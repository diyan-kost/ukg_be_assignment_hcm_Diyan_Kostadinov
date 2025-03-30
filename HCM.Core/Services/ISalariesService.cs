using HCM.Core.Models.Salary;

namespace HCM.Core.Services
{
    public interface ISalariesService
    {
        Task AddNewSalary(AddSalaryModel model);
    }
}
