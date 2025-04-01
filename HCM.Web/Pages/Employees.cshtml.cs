using HCM.Core.Helpers;
using HCM.Core.Models.Employee;
using HCM.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace HCM.Web.Pages
{
    [Authorize(Roles = "Manager,HR Admin")]
    public class EmployeesModel : PageModel
    {
        private readonly IEmployeesService _employeesService;

        public EmployeesModel(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        public List<EmployeeBasicInfo> Employees { get; set; }

        public async Task OnGet()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var id = Convert.ToInt32(identity!.FindFirst(CustomClaims.EmployeeId)!.Value);
            var role = identity!.FindFirst(ClaimTypes.Role)!.Value;

            if (role == "Manager")
            {
                var employees = await _employeesService.GetEmployeesByManagerIdAsync(id);
                Employees = employees.ToList();
            }
            else
            {
                var employees = await _employeesService.GetAllEmployeesAsync();
                Employees = employees.ToList();
            }

        }
    }
}
