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

        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {            
            var account = _accountService.FindAccountByUsernameAndPassword(Username, Password);            
            if (account != null)
            {
                HttpContext.Session.SetString("Role", "User");
                HttpContext.Session.SetString("Username", account.Username);
                return RedirectToPage("/Error");
            }
            else
            {
                ViewData["ErrorMessage"] = "Invalid username or password.";
                return Page();
            }
        }
    }
}