using EmployeeManagementBO.Models;
using EmployeeManagementService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.ManagerPage
{
    public class ManagerHomePageModel : PageModel
    {
        private readonly IEmployeeService _employeeService;

        [BindProperty]
        public string CbSearch { get; set; }

        [BindProperty]
        public string TxtSearch { get; set; }
        public IList<Employee> Employees { get; set; } = default!;

        public ManagerHomePageModel(IEmployeeService employeeService)
        {            
            _employeeService = employeeService;
        }

        public IActionResult OnGet(string cbSearch, string txtSearch)
        {
            if (HttpContext.Session.GetString("Role") != "Manager") return Forbid();
            if (!string.IsNullOrEmpty(txtSearch))
            {
                CbSearch = cbSearch;
                TxtSearch = txtSearch;
                if (cbSearch == "Name")
                {
                    Employees = _employeeService.GetAllByNameIncludeAccountDepartmentJob(txtSearch.Trim()).ToList();
                }
                else if (cbSearch == "Email")
                {
                    Employees = _employeeService.GetAllByEmailIncludeAccountDepartmentJob(txtSearch.Trim()).ToList();
                }
            } else
            {
                Employees = _employeeService.GetAllIncludeAccountAddressDepartmentJobAndHistory().ToList();
            }
            
            return Page();
        }
    }
}
