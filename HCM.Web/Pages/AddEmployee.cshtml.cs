using HCM.Core.Models.Employee;
using HCM.Core.Services;
using HCM.Web.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HCM.Web.Pages
{
    [Authorize(Roles = LoginRoles.HR_ADMIN)]
    public class AddEmployeeModel : PageModel
    {
        private readonly IEmployeesService _employeesService;

        public AddEmployeeModel(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [BindProperty]
        public AddNewEmployeeDto AddEmployee { get; set; }

        public List<EmployeeInfoDto> EmployeeList { get; set; }

        public async Task OnGet()
        {
            await LoadEmployees();
        }

        public async Task OnPostAsync()
        {
            await _employeesService.AddNewEmployeeAsync(AddEmployee);

            await LoadEmployees();
        }

        private async Task LoadEmployees()
        {
            EmployeeList = (await _employeesService.GetAllEmployeesAsync()).ToList();
        }
    }
}
