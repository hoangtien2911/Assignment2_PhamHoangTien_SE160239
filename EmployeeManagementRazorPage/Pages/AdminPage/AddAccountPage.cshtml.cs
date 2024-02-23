using EmployeeManagementBO.Models;
using EmployeeManagementService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Security.Principal;
using System.Text.RegularExpressions;

namespace EmployeeManagementRazorPage.Pages.AdminPage
{
    public class AddAccountPageModel : PageModel
    {
        private readonly IAccountService _accountService;
        [BindProperty]
        public Account Account { get; set; }

        public AddAccountPageModel(IAccountService accountService) {
            _accountService = accountService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("Role") != "Admin") return Forbid();
            var account = ValidateAccountInformation(Account);
            if (account == null)
            {
                return Page();
            }
            bool isCreated = _accountService.Create(account);
            if (isCreated)
            {
                return RedirectToPage("/AdminPage/AdminHomePage");
            }
            else
            {
                ViewData["ErrorMessageUsername"] = "Duplicate username. Please check again!";
                return Page();
            }            
        }

        private Account? ValidateAccountInformation(Account account)
        {
            //Validate password
            var password = account.Password.Trim();
            Regex regex = new Regex(@"^(?=.*[A-Z])(?=.*\W).{8,}$"); // At least one uppercase letter, one special character, and minimum 8 characters
            if (!regex.IsMatch(password))
            {
                ViewData["ErrorMessagePassword"] = "Please input password with min length 8 characters, containing at least one uppercase letter and one special character!";                
                return null;
            }
            //Validate email
            var email = account.Email.Trim();
            if (string.IsNullOrEmpty(email.Trim()))
            {
                ViewData["ErrorMessageEmail"] = "Please enter email!";
                return null;
            }
            else
            {
                if (!new EmailAddressAttribute().IsValid(email))
                {
                    ViewData["ErrorMessageEmail"] = "Please enter valid email!";
                    return null;
                }
            }
            // Check Existed email
            if (!account.Email.Equals(email))
            {
                if (_accountService.IsExistedEmail(email))
                {
                    ViewData["ErrorMessageEmail"] = "Email already exist. Please input another email!";
                    return null;
                }
            }            

            //Validate fullname
            var fullName = account.FullName.Trim();
            if (string.IsNullOrEmpty(fullName))
            {
                ViewData["ErrorMessageFullname"] = "Please input fullname!";
                return null;
            }            

            //Validate phone            
            if (!Regex.IsMatch(account.PhoneNumber, @"^[0-9\s\+]+$"))
            {
                ViewData["ErrorMessagePhoneNumber"] = "Please enter valid phone number!";
                return null;
            }            
            return account;
        }
    }
}
