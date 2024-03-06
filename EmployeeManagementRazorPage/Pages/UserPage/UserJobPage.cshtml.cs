using EmployeeManagementBO.Models;
using EmployeeManagementService.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.UserPage
{
    public class UserJobPageModel : PageModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IJobHistoryService _jobHistoryService;

        [BindProperty]
        public Employee Employee { get; set; } = default!;


        public UserJobPageModel(IEmployeeService employeeService, IJobHistoryService jobHistoryService)
        {
            _employeeService = employeeService;
            _jobHistoryService = jobHistoryService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Role") != "User") return Forbid();
            var username = HttpContext.Session.GetString("Username");            
            if (username != null)
            {
                Employee = _employeeService.GetEmployeeByUserNameIncludeJobAndDepartment(username);                
                return Page();
            }
            return Page();
        }
    }
}
