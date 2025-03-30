using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HCM.Web.Controllers
{
    [Authorize(Roles = "Employee")]
    [Authorize(Roles = "Manager")]
    [Authorize(Roles = "HRAdmin")]
    public class EmployeesController : Controller
    {
        public async Task<IActionResult> LoadEmployeeDetails()
        {
            return View();
        }
    }
}
