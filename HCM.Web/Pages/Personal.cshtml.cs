using HCM.Core.Exceptions;
using HCM.Core.Helpers;
using HCM.Core.Models.Employee;
using HCM.Core.Models.Role;
using HCM.Core.Models.Salary;
using HCM.Core.Models.User;
using HCM.Core.Services;
using HCM.Infrastructure.Entities;
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
        private readonly ISalariesService _salariesService;
        private readonly IRolesService _rolesService;

        public PersonalModel(IEmployeesService employeesService, IUsersService usersService, ISalariesService salariesService, IRolesService rolesService)
        {
            _employeesService = employeesService;
            _usersService = usersService;
            _salariesService = salariesService;
            _rolesService = rolesService;
        }

        [BindProperty]
        public EmployeeDetails EmployeeDetails { get; set; }

        [BindProperty]
        public UpdateEmployeeModel UpdateEmployeeModel { get; set; }

        [BindProperty]
        public AddSalaryModel AddSalaryModel { get; set; }

        [BindProperty]
        public UpdateUser UpdateUser { get; set; }

        [BindProperty]
        public DeleteUser DeleteUser { get; set; }

        [BindProperty]
        public CreateUser CreateUser { get; set; }

        public IEnumerable<RoleInfo> Roles { get; set; }

        public async Task OnGet(int id)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var emplId = Convert.ToInt32(identity!.FindFirst(CustomClaims.EmployeeId)!.Value);
            var role = identity.FindFirst(ClaimTypes.Role).Value;

            var checkManagerId = false;
            if (emplId != id && role != "HR Admin")
            {
                if (role == "Employee")
                {
                    throw new PermissionDeniedException("Permission denied");
                }

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

        public async Task OnPostUpdateUser()
        {
            await _usersService.UpdateUserAsync(UpdateUser);

            await LoadEmployeeDetails(UpdateUser.EmployeeId);
        }

        public async Task OnPostDeleteUser()
        {
            await _usersService.DeleteUserAsync(DeleteUser);

            await LoadEmployeeDetails(DeleteUser.EmployeeId);
        }

        public async Task OnPostCreateUser()
        {
            await _usersService.CreateUserAsync(CreateUser);

            await LoadEmployeeDetails(CreateUser.EmployeeId);
        }

        private async Task LoadEmployeeDetails(int id, bool checkManagerId = false)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var loggedEmployeeId = Convert.ToInt32(identity!.FindFirst(CustomClaims.EmployeeId)!.Value);
            var loggedEmployeeRole = identity.FindFirst(ClaimTypes.Role)!.Value;

            // Reload data after update
            var employeeDetails = await _employeesService.GetEmployeeDetailsById(id);

            if (checkManagerId && loggedEmployeeId != employeeDetails.ManagerId)
            {
                throw new PermissionDeniedException("Permission denied");
            }

            EmployeeDetails = employeeDetails;
            EmployeeDetails.Username = identity.FindFirst(ClaimTypes.Name)!.Value;
            EmployeeDetails.Role = identity.FindFirst(ClaimTypes.Role)!.Value;
            if (loggedEmployeeId != id)
            {
                var username = await _usersService.GetUsernameByEmployeeIdAsync(id);
                EmployeeDetails.Username = username;
                EmployeeDetails.Role = null;
                if (loggedEmployeeRole == "HR Admin" && username != null)
                {
                    var role = await _usersService.GetUserRoleByUsernameAsync(EmployeeDetails.Username!);
                    EmployeeDetails.Role = role;
                }
            }

            if (loggedEmployeeRole == "HR Admin")
            {
                Roles = await _rolesService.GetRolesAsync();
            }
        }
    }
}
