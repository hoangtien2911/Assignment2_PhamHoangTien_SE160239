using EmployeeManagementBO.Models;
using EmployeeManagementService.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace EmployeeManagementRazorPage.Pages.UserPage
{
    public class UserProfilePageModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;

        public UserProfilePageModel(IAccountService accountService, IAddressService addressService)
        {
            _accountService = accountService;
            _addressService = addressService;
        }

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string FullName { get; set; }
        [BindProperty]
        public string PhoneNumber { get; set; }
        [BindProperty]
        public string? Street { get; set; }
        [BindProperty]
        public string? Ward { get; set; }
        [BindProperty]
        public string? City { get; set; }
        [BindProperty]
        public string? Province { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("Role") != "User") return Forbid();
            string? username = HttpContext.Session.GetString("Username");            
            if (username == null)
            {
                return NotFound();
            }
            else
            {
                var account = _accountService.FindAccountByUsername(username);
                if (account == null)
                {
                    return NotFound();
                }
                else
                {
                    Username = account.Username;
                    Email = account.Email;
                    FullName = account.FullName;
                    PhoneNumber = account.PhoneNumber;
                    var address = account.Address;
                    if (address != null)
                    {
                        Street = address.Street;
                        Ward = address.Ward;
                        City = address.City;
                        Province = address.Province;
                    }
                    return Page();
                }
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("Role") != "User") return Forbid();
            var username = HttpContext.Session.GetString("Username");            
            if (username == null || !username.Equals(Username)) return Forbid();
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var account = _accountService.FindAccountByUsername(username);
            if (account != null)
            {
                var accountUpdate = ValidateAccountInformation(account);
                if (accountUpdate == null)
                {
                    return Page();
                }
                else
                {
                    var address = accountUpdate.Address;
                    if (address != null && account.AddressId == null)
                    {
                        account.AddressId = _addressService.Create(address)?.AddressId;
                    }
                    else if (address != null && account.AddressId != null)
                    {
                        _addressService.Update(address);
                    }
                    _accountService.Update(account);
                }
            }
            return RedirectToPage();
        }

        private Account? ValidateAccountInformation(Account account)
        {
            //Validate email
            var email = Email.Trim();
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
            account.Email = email;

            //Validate fullname
            var fullName = FullName.Trim();
            if (string.IsNullOrEmpty(fullName))
            {
                ViewData["ErrorMessageFullname"] = "Please input fullname!";
                return null;
            }
            account.FullName = fullName;

            //Validate phone
            var phone = PhoneNumber;
            if (!Regex.IsMatch(phone, @"^[0-9\s\+]+$"))
            {
                ViewData["ErrorMessagePhoneNumber"] = "Please enter valid phone number!";
                return null;
            }

            account.PhoneNumber = phone;

            if (Street != null || Ward != null || City != null || Province != null)
            {
                if (account.Address == null)
                {
                    var address = new Address();
                    address.Street = Street ?? "";
                    address.Ward = Ward ?? "";
                    address.City = City ?? "";
                    address.Province = Province ?? "";
                    account.Address = address;
                }
                else
                {
                    account.Address.Street = Street ?? "";
                    account.Address.Ward = Ward ?? "";
                    account.Address.City = City ?? "";
                    account.Address.Province = Province ?? "";
                }
            }
            return account;
        }
    }
}
