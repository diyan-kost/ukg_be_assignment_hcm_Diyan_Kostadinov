using FluentValidation;
using HCM.Core.Models.Salary;
using HCM.Core.Validators;
using HCM.Infrastructure.Entities;
using HCM.Infrastructure.Repositories;

namespace HCM.Core.Services.Implementations
{
    public class SalariesService : ISalariesService
    {
        private readonly ISalariesRepository _salariesRepository;

        public SalariesService(ISalariesRepository salariesRepository)
        {
            _salariesRepository = salariesRepository;
        }

        public async Task AddNewSalary(AddSalaryModel model)
        {
            var validator = new SalaryValidator();

            await validator.ValidateAndThrowAsync(model);

            var salary = new Salary()
            {
                Amount = model.Amount,
                EffectiveDate = model.EffectiveDate,
                Note = model.Note,
                EmployeeId = model.EmployeeId,
            };

            _ = await _salariesRepository.AddNewSalaryAsync(salary);
        }
    }
}
