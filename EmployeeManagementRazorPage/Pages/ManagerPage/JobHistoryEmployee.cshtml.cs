using EmployeeManagementBO.Models;
using EmployeeManagementService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.ManagerPage
{
    public class JobHistoryEmployeeModel : PageModel
    {
        private readonly IJobHistoryService _jobHistoryService;

        [BindProperty]
        public IList<JobHistory> JobHistories { get; set; } = default!;

        public JobHistoryEmployeeModel(IJobHistoryService jobHistoryService)
        {
            _jobHistoryService = jobHistoryService;
        }

        public async Task<IActionResult> OnGetAsync(int employeeId)
        {
            if (HttpContext.Session.GetString("Role") != "Manager") return Forbid();
            JobHistories = _jobHistoryService.GetAllJobHistoryIncludeJobAndDepartmentByEmployeeId(employeeId).ToList();
            return Page();
        }
    }
}
