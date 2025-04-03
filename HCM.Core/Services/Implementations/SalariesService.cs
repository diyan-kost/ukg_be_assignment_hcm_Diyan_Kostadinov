using FluentValidation;
using HCM.Core.Common;
using HCM.Core.Models.Salary;
using HCM.Core.Validators;
using HCM.Infrastructure.Entities;
using HCM.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;

namespace HCM.Core.Services.Implementations
{
    public class SalariesService : ISalariesService
    {
        private readonly ISalariesRepository _salariesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SalariesService(ISalariesRepository salariesRepository, IHttpContextAccessor httpContextAccessor)
        {
            _salariesRepository = salariesRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task AddNewSalaryAsync(AddSalaryDto model)
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

            _httpContextAccessor.HttpContext.Session.SetString(MessageTypes.SUCCESS, "Salary created successfully");
        }
    }
}
