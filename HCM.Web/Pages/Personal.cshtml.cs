using HCM.Core.Helpers;
using HCM.Core.Models.Employee;
using HCM.Core.Models.Salary;
using HCM.Core.Services;
using HCM.Infrastructure.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;
using System.Security.Principal;

namespace HCM.Web.Pages
{
    [Authorize(Roles = "Employee,Manager,HR Admin")]
    public class PersonalModel : PageModel
    {
        private readonly IEmployeesService _employeesService;
        private readonly IUsersService _usersService;
        private readonly ISalariesService _salariesService;

        public PersonalModel(IEmployeesService employeesService, IUsersService usersService, ISalariesService salariesService)
        {
            _employeesService = employeesService;
            _usersService = usersService;
            _salariesService = salariesService;
        }

        [BindProperty]
        public EmployeeDetails EmployeeDetails { get; set; }

        [BindProperty]
        public UpdateEmployeeModel UpdateEmployeeModel { get; set; }

        [BindProperty]
        public AddSalaryModel AddSalaryModel { get; set; }

        public async Task OnGet(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var emplId = Convert.ToInt32(identity!.FindFirst(CustomClaims.EmployeeId)!.Value);
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            var checkManagerId = false;
            if (emplId != id && role != "HR Admin")
            {
                if (role == "Employee")
                    throw new Exception("Permission denied");

                checkManagerId = true;
            }

            await LoadEmployeeDetails(id, checkManagerId);
        }

        public async Task OnPostUpdateEmployeeAsync()
        {
            await _employeesService.UpdateEmployeeAsync(UpdateEmployeeModel);

            await LoadEmployeeDetails(UpdateEmployeeModel.Id);
        }

        public async Task OnPostAddSalary()
        {
            await _salariesService.AddNewSalary(AddSalaryModel);

            await LoadEmployeeDetails(AddSalaryModel.EmployeeId);
        }

        private async Task LoadEmployeeDetails(int id, bool checkManagerId = false)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var loggedEmployeeId = Convert.ToInt32(identity!.FindFirst(CustomClaims.EmployeeId)!.Value);

            // Reload data after update
            var employeeDetails = await _employeesService.GetEmployeeDetailsById(id);

            if (checkManagerId && loggedEmployeeId != employeeDetails.ManagerId)
            {
                throw new Exception("Permission denied");
            }

            EmployeeDetails = employeeDetails;
            EmployeeDetails.Username = identity.FindFirst(ClaimTypes.Name)!.Value;
            if (loggedEmployeeId != id)
            {
                var username = await _usersService.GetUsernameByEmployeeIdAsync(id);
                EmployeeDetails.Username = username ?? "N/A";
            }
        }
    }
}
