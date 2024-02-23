using EmployeeManagementBO.Models;
using EmployeeManagementService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Principal;

namespace EmployeeManagementRazorPage.Pages.AdminPage
{
    public class JobHistoryPageModel : PageModel
    {
        private readonly IJobHistoryService _jobHistoryService;
        private readonly IAccountService _accountService;

        [BindProperty]
        public IList<JobHistory> JobHistories { get; set; } = default!;

        public JobHistoryPageModel(IJobHistoryService jobHistoryService, IAccountService accountService)
        {
            _jobHistoryService = jobHistoryService;
            _accountService = accountService;
        }

        public async Task<IActionResult> OnGetAsync(string username)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Forbid();
            var account = _accountService.FindAccountIncludeEmployeeByUsername(username);
            if (account != null && account.Role.ToLower() == "user")
            {
                if (account.Employee != null)
                {
                    JobHistories = _jobHistoryService.GetAllJobHistoryIncludeJobAndDepartmentByEmployeeId(account.Employee.EmployeeId).ToList();
                } else
                {
                    string message = $"{account.Username} is not yet an employee.";
                    return Content($"<script>alert('{message}'); window.location.href = '/AdminPage/AdminHomePage';</script>", "text/html");
                }
                
            }
            else
            {
                string message = "Manager role does not have job history.";
                return Content($"<script>alert('{message}'); window.location.href = '/AdminPage/AdminHomePage';</script>", "text/html");
            }

            return Page();
        }
    }
}
