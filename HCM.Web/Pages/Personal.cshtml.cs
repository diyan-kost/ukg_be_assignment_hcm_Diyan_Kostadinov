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
        private readonly IUsersService _usersService;

        public PersonalModel(IEmployeesService employeesService, IUsersService usersService)
        {
            _employeesService = employeesService;
            _usersService = usersService;
        }

        [BindProperty]
        public EmployeeDetails EmployeeDetails { get; set; }

        public async Task OnGet(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var emplId = Convert.ToInt32(identity!.FindFirst(CustomClaims.EmployeeId)!.Value);
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            if (role == "Employee" && emplId != id)
                throw new Exception("Permission denied");

            var employeeDetails = await _employeesService.GetEmployeeDetailsById(id);

            EmployeeDetails = employeeDetails;
            EmployeeDetails.Username = identity.FindFirst(ClaimTypes.Name)!.Value;
            if (emplId != id)
            {
                var username = await _usersService.GetUsernameByEmployeeIdAsync(id);
                EmployeeDetails.Username = username ?? "N/A";
            }
        }
    }
}
