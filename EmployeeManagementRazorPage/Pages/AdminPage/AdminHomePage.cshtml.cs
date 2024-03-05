using EmployeeManagementBO.Models;
using EmployeeManagementService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementRazorPage.Pages.AdminPage
{
    public class AdminHomePageModel : PageModel
    {
        private readonly IAccountService _accountService;

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
        public IList<Account> Accounts { get; set; } = default!;

        public AdminHomePageModel(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public IActionResult OnGet(string cbSearch, string txtSearch, int pageNumber = 1)
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Forbid();
            TotalPage = (int)Math.Ceiling(_accountService.CountTotalAccount() / (double)PageSize);
            PageNumber = pageNumber;
            if (!string.IsNullOrEmpty(txtSearch))
            {
                CbSearch = cbSearch;
                TxtSearch = txtSearch;
                if (cbSearch == "Name")
                {
                    Accounts = _accountService.FindAccountIncludeAddressByFullnamePageNumPageSize(TxtSearch, pageNumber, PageSize).ToList();
                }
                else if (cbSearch == "Email")
                {
                    Accounts = _accountService.FindAccountIncludeAddressByEmailPageNumPageSize(TxtSearch, pageNumber, PageSize).ToList();
                }
            }
            else
            {
                Accounts = _accountService.GetAllIncludeAddressWithRoleNotAdminByPageNumPageSize(pageNumber, PageSize).ToList();
            }
            return Page();
        }
    }
}
