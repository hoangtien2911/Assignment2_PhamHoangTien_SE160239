using EmployeeManagementBO.Models;
using EmployeeManagementService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.ManagerPage
{
    public class JobPageModel : PageModel
    {
        private readonly IJobService _jobService;

        public JobPageModel(IJobService jobService)
        {
            _jobService = jobService;
        }
        public IList<Job> Jobs { get; set; } = default!;
        public IActionResult OnGet()
        {
            //if (HttpContext.Session.GetString("Role") != "Manager") return Forbid();
            Jobs = _jobService.GetAll().ToList();
            return Page();
        }
    }
}
