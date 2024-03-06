using EmployeeManagementBO.Models;
using EmployeeManagementService.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.AdminPage
{
    public class AddUpdateJobPageModel : PageModel
    {
        private readonly IJobService _jobService;

        [BindProperty]
        public int JobId { get; set; }
        [BindProperty]
        public int IsUpdate { get; set; } = 0;
        [BindProperty]
        public Job Job { get; set; }

        public AddUpdateJobPageModel(IJobService jobService) {
            _jobService = jobService;
        }


        public IActionResult OnGet(int jobId, int isUpdate)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Forbid();
            if (isUpdate == 1)
            {
                IsUpdate = 1;
                JobId = jobId;
                Job = _jobService.GetJobById(JobId);
            }        
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Forbid();
            if (IsUpdate == 1)
            {
                _jobService.Update(Job);
                return RedirectToPage("/AdminPage/JobPage");
            }
            else {
                _jobService.Create(Job);
                return RedirectToPage("/AdminPage/JobPage");
            }            
        }
    }
}
