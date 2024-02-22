using EmployeeManagementBO.Models;
using EmployeeManagementService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Principal;

namespace EmployeeManagementRazorPage.Pages.ManagerPage
{
    public class AddEmployeePageModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IJobService _jobService;
        private readonly IJobHistoryService _jobHistoryService;
        private readonly IDepartmentService _departmentService;
        private readonly IEmployeeService _employeeService;
        
        [BindProperty]      
        public Employee Employee { get; set; }

        public bool IsNoAccount { get; set; } = false;

        public AddEmployeePageModel(IAccountService accountService, IJobService jobService, IJobHistoryService jobHistoryService, IDepartmentService departmentService, IEmployeeService employeeService) { 
            _accountService = accountService;
            _jobService = jobService;
            _jobHistoryService = jobHistoryService;
            _departmentService = departmentService;
            _employeeService = employeeService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Role") != "Manager") return Forbid();
            var accounts = _accountService.FindAccountsWithNullEmployee().ToList();
            if (accounts.Count == 0)
            {
                IsNoAccount = true;
            }
            ViewData["Username"] = new SelectList(_accountService.FindAccountsWithNullEmployee(), "Username", "Email");
            ViewData["DepartmentId"] = new SelectList(_departmentService.GetAll(), "DepartmentId", "DepartmentName");
            ViewData["JobId"] = new SelectList(_jobService.GetAll(), "JobId", "JobTitle");
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("Role") != "Manager") return Forbid();
            if (Employee != null)
            {
                Employee newEmployee = new Employee
                {
                    HiredDate = Employee.HiredDate,
                    Username = Employee.Username,
                    DepartmentId = Employee.DepartmentId,
                    JobId = Employee.JobId
                };
                _employeeService.Create(newEmployee);
                JobHistory jobHistory = new JobHistory();
                jobHistory.JobId = Employee.JobId;
                jobHistory.DepartmentId = Employee.DepartmentId;
                jobHistory.StartedDate = Employee.HiredDate;
                jobHistory.EmployeeId = newEmployee.EmployeeId;
                _jobHistoryService.Create(jobHistory);
            }
            return RedirectToPage("/ManagerPage/ManagerHomePage");
        }
    }
}
