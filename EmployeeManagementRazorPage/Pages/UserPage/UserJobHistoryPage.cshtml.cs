using EmployeeManagementBO.Models;
using EmployeeManagementService.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.UserPage
{
    public class UserJobHistoryPageModel : PageModel
    {
        private readonly IEmployeeService _employeeService;
        private readonly IJobHistoryService _jobHistoryService;

        [BindProperty]
        public IList<JobHistory> JobHistories { get; set; } = default!;


        public UserJobHistoryPageModel(IEmployeeService employeeService, IJobHistoryService jobHistoryService)
        {
            _employeeService = employeeService;
            _jobHistoryService = jobHistoryService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Role") != "User") return Forbid();
            var username = HttpContext.Session.GetString("Username");            
            if(username != null)
            {
                var employee = _employeeService.GetEmployeeByUserName(username);
                JobHistories = _jobHistoryService.GetAllJobHistoryIncludeJobAndDepartmentByEmployeeId(employee.EmployeeId).ToList();
                return Page();
            }
            return Page();
        }
    }
}
