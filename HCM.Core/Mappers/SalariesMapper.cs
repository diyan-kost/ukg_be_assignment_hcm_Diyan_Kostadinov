using HCM.Core.Models.Salary;
using HCM.Infrastructure.Entities;

namespace HCM.Core.Mappers
{
    public static class SalariesMapper
    {
        public static List<SalaryDto> ToSalaryDetails(this IEnumerable<Salary> salaries)
        {
            var salaryDetails = salaries
                .Select(s => new SalaryDto { Amount = s.Amount, EffectiveDate = s.EffectiveDate, Note = s.Note })
                .ToList();

            return salaryDetails;
        }
    }
}
