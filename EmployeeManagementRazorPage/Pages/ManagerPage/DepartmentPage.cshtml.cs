using EmployeeManagementBO.Models;
using EmployeeManagementService.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.ManagerPage
{
    public class DepartmentPageModel : PageModel
    {
        private readonly IDepartmentService _departmentService;

        [BindProperty]
        public IList<Department> Departments { get; set; } = default!;

        public DepartmentPageModel(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }
        
        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("Role") != "Manager") return Forbid();
            Departments = _departmentService.GetAll().ToList();
            return Page();
        }
    }
}
