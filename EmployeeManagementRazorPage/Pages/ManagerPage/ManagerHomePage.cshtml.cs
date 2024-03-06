using EmployeeManagementBO.Models;
using EmployeeManagementService.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.ManagerPage
{
    public class ManagerHomePageModel : PageModel
    {
        private readonly IEmployeeService _employeeService;

        private const int PageSize = 5;

        [BindProperty]
        public int TotalPage { get; set; } = 1;

        [BindProperty]
        public int PageNumber { get; set; } = 1;

        [BindProperty]
        public string CbSearch { get; set; } = default!;

        [BindProperty]
        public string TxtSearch { get; set; } = default!;

        [BindProperty]
        public IList<Employee> Employees { get; set; } = default!;

        public ManagerHomePageModel(IEmployeeService employeeService)
        {            
            _employeeService = employeeService;
        }

        public IActionResult OnGet(string cbSearch, string txtSearch, int pageNumber = 1)
        {
            if (HttpContext.Session.GetString("Role") != "Manager") return Forbid();
            TotalPage = (int)Math.Ceiling(_employeeService.CountTotalEmployee() / (double)PageSize);
            PageNumber = pageNumber;
            if (!string.IsNullOrEmpty(txtSearch))
            {                
                CbSearch = cbSearch;
                TxtSearch = txtSearch;
                if (cbSearch == "Name")
                {
                    Employees = _employeeService.GetAllByNameIncludeAccountDepartmentJob(txtSearch.Trim(), pageNumber, PageSize).ToList();
                }
                else if (cbSearch == "Email")
                {
                    Employees = _employeeService.GetAllByEmailIncludeAccountDepartmentJob(txtSearch.Trim(), pageNumber, PageSize).ToList();
                }                
            } else
            {
                Employees = _employeeService.GetAllIncludeAccountAddressDepartmentJobAndHistory(pageNumber, PageSize).ToList();
            }            
            return Page();
        }
    }
}
