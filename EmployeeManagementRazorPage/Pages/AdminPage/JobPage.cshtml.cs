using EmployeeManagementBO.Models;
using EmployeeManagementService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.AdminPage
{
    public class JobPageModel : PageModel
    {
        private readonly IJobService _jobService;

        [BindProperty]
        public IList<Job> Jobs { get; set; } = default!;

        public JobPageModel(IJobService jobService)
        {
            _jobService = jobService;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Forbid();
            Jobs = _jobService.GetAll().ToList();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            return null;
        }
    }
}
