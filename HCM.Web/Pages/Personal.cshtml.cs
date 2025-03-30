using HCM.Core.Helpers;
using HCM.Core.Models.Employee;
using HCM.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace HCM.Web.Pages
{
    [Authorize(Roles = "Employee,Manager,HR Admin")]
    public class PersonalModel : PageModel
    {
        private readonly IEmployeesService _employeesService;
        public PersonalModel(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [BindProperty]
        public EmployeeDetails EmployeeDetails { get; set; }

        public async Task OnGet()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var id = Convert.ToInt32(identity!.FindFirst(CustomClaims.EmployeeId)!.Value);

            var employeeDetails = await _employeesService.GetEmployeeDetailsById(id);

            EmployeeDetails = employeeDetails;
            EmployeeDetails.Username = identity.FindFirst(ClaimTypes.Name)!.Value;
        }
    }
}
