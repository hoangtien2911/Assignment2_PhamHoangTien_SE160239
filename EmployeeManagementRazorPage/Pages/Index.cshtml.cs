using EmployeeManagementBO.Models;
using EmployeeManagementService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IAccountService _accountService;
        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var role = HttpContext.Session.GetString("Role");
            switch (role)
            {
                case "Manager":
                    return RedirectToPage("ManagerPage/ManagerHomePage");
                case "Admin":
                    return RedirectToPage("AdminPage/AdminHomePage");
                case "User":
                    return RedirectToPage("UserPage/UserProfilePage");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {            
            var account = _accountService.FindAccountByUsernameAndPassword(Username, Password);            
            if (account != null)
            {
                string role = account.Role;
                switch (role)
                {                      
                    case "Manager":
                        HttpContext.Session.SetString("Role", "Manager");
                        HttpContext.Session.SetString("Username", account.Username);
                        return RedirectToPage("ManagerPage/ManagerHomePage");
                    case "Admin":
                        HttpContext.Session.SetString("Role", "Admin");
                        HttpContext.Session.SetString("Username", account.Username);
                        return RedirectToPage("AdminPage/AdminHomePage");
                    default:
                        HttpContext.Session.SetString("Role", "User");
                        HttpContext.Session.SetString("Username", account.Username);
                        return RedirectToPage("UserPage/UserProfilePage");
                }
            }
            else
            {
                ViewData["ErrorMessage"] = "Invalid username or password.";
                return Page();
            }
        }
    }
}