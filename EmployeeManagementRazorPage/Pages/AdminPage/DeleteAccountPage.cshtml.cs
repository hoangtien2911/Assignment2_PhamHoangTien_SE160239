using EmployeeManagementService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.AdminPage
{
    public class DeleteAccountPageModel : PageModel
    {
        private readonly IAccountService _accountService;
        public DeleteAccountPageModel(IAccountService accountService) {
            _accountService = accountService;
        }
        public IActionResult OnGet(string username)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Forbid();
            var account  = _accountService.FindAccountByUsername(username);
            account.DeleteFlag = 1;
            _accountService.Update(account);
            return RedirectToPage("/AdminPage/AdminHomePage");
        }
    }
}
