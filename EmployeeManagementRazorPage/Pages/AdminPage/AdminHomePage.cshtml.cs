using EmployeeManagementBO.Models;
using EmployeeManagementService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.AdminPage
{
    public class AdminHomePageModel : PageModel
    {
        private readonly IAccountService _accountService;

        [BindProperty]
        public string CbSearch { get; set; } = default!;

        [BindProperty]
        public string TxtSearch { get; set; } = default!;

        [BindProperty]
        public IList<Account> Accounts { get; set; } = default!;

        public AdminHomePageModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult OnGet(string cbSearch, string txtSearch)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Forbid();
            if (!string.IsNullOrEmpty(txtSearch))
            {
                CbSearch = cbSearch;
                TxtSearch = txtSearch;
                if (cbSearch == "Name")
                {
                    Accounts = _accountService.FindAccountIncludeAddressByFullname(TxtSearch).ToList();
                }
                else if (cbSearch == "Email")
                {
                    Accounts = _accountService.FindAccountIncludeAddressByEmail(TxtSearch).ToList();
                }
            }
            else
            {
                Accounts = _accountService.GetAllIncludeAddressWithRoleNotAdmin().ToList();
            }

            return Page();
        }
    }
}
