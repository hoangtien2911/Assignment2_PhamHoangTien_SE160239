using EmployeeManagementBO.Models;
using EmployeeManagementService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.AdminPage
{
    public class AddUpdateDepartmentPageModel : PageModel
    {
        private readonly IDepartmentService _departmentService;

        [BindProperty]
        public int DepartmentId { get; set; }
        [BindProperty]
        public int IsUpdate { get; set; } = 0;
        [BindProperty]
        public Department Department { get; set; }

        public AddUpdateDepartmentPageModel(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }


        public IActionResult OnGet(int departmentId, int isUpdate)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Forbid();
            if (isUpdate == 1)
            {
                IsUpdate = 1;
                DepartmentId = departmentId;
                Department = _departmentService.GetDepartmentById(departmentId);
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Forbid();
            if (IsUpdate == 1)
            {
                _departmentService.Update(Department);
                return RedirectToPage("/AdminPage/DepartmentPage");
            }
            else
            {
                _departmentService.Create(Department);
                return RedirectToPage("/AdminPage/DepartmentPage");
            }
        }
    }
}
