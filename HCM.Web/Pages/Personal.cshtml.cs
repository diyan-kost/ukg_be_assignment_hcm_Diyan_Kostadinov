using HCM.Core.Models.Employee;
using HCM.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HCM.Web.Pages
{
    [Authorize(Roles = "Employee,Manager,HRAdmin")]
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
            var userId = Convert.ToInt32(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

            var employeeDetails = await _employeesService.GetEmployeeDetailsByUserId(userId);

            Console.WriteLine("test");
        }
    }
}
