using EmployeeManagementBO.Models;
using EmployeeManagementService.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace EmployeeManagementRazorPage.Pages.ManagerPage
{
    public class UpdateInformationEmployeeModel : PageModel
    {
        private readonly IAccountService _accountService;
        private readonly IAddressService _addressService;
        private readonly IJobService _jobService;
        private readonly IDepartmentService _departmentService;
        private readonly IJobHistoryService _jobHistoryService;
        private readonly IEmployeeService _employeeService;

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string FullName { get; set; }
        [BindProperty]
        public string PhoneNumber { get; set; }
        [BindProperty]
        public int DepartmentId { get; set; }
        [BindProperty]
        public int JobId { get; set; }
        [BindProperty]
        public string? Street { get; set; }
        [BindProperty]
        public string? Ward { get; set; }
        [BindProperty]
        public string? City { get; set; }
        [BindProperty]
        public string? Province { get; set; }

        public UpdateInformationEmployeeModel(IAccountService accountService, IAddressService addressService, IJobService jobService, IDepartmentService departmentService, IJobHistoryService jobHistoryService, IEmployeeService employeeService)
        {
            _accountService = accountService;
            _addressService = addressService;
            _jobService = jobService;
            _departmentService = departmentService;
            _jobHistoryService = jobHistoryService;
            _employeeService = employeeService;
        }


        public async Task<IActionResult> OnGetAsync(string employeeUsername)
        {
            if (HttpContext.Session.GetString("Role") != "Manager") return Forbid();
            var account = _accountService.FindAccountByUsername(employeeUsername);
            if (account == null)
            {
                return NotFound();
            }
            else if (account.Role.Equals("Admin") || account.Role.Equals("Manager")) return Forbid();
            else
            {
                ViewData["DepartmentId"] = new SelectList(_departmentService.GetAll(), "DepartmentId", "DepartmentName");
                ViewData["JobId"] = new SelectList(_jobService.GetAll(), "JobId", "JobTitle");
                var employee = _employeeService.GetEmployeeByUserName(employeeUsername);
                if (employee != null)
                {
                    DepartmentId = employee.DepartmentId;
                    JobId = employee.JobId;
                }                
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.Session.GetString("Role") != "Manager") return Forbid();
            Employee? employee = ValidateEmployeeInformation(_employeeService.GetEmployeeByUsernameIncludeAccountAddressDepartmentJobAndHistory(Username));
            if (!_departmentService.IsExistingDepartment(DepartmentId))
            {
                ViewData["ErrorMessageDepartment"] = "Not exist this department!";
                return Page();
            }
            if (!_jobService.IsExistingJob(JobId))
            {
                ViewData["ErrorMessageJob"] = "Not exist this job!";
                return Page();
            }
            if (employee != null)
            {
                if (employee.Account != null)
                {
                    if (employee.Account.Address != null && employee.Account.AddressId != null)
                    {
                        _addressService.Update(employee.Account.Address);
                    }
                    else if (employee.Account.Address != null && employee.Account.AddressId == null)
                    {
                        employee.Account.AddressId = _addressService.Create(employee.Account.Address)?.AddressId;
                    }
                    _accountService.Update(employee.Account);
                }
                if (!employee.JobId.Equals(JobId) || !employee.DepartmentId.Equals(DepartmentId))
                {
                    //Update end date of old job
                    JobHistory lastJobHistory = employee.JobHistories.OrderByDescending(jh => jh.JobHistoryId)
                                    .First();
                    lastJobHistory.EndedDate = DateTime.Now;
                    _jobHistoryService.Update(lastJobHistory);

                    //Add new job history
                    JobHistory newJobHistory = new JobHistory();
                    newJobHistory.StartedDate = DateTime.Now;
                    newJobHistory.JobId = JobId;
                    newJobHistory.DepartmentId = DepartmentId;
                    newJobHistory.EmployeeId = employee.EmployeeId;
                    //Update employee data
                    _jobHistoryService.Create(newJobHistory);
                    var employeeUpdate = _employeeService.GetEmployeeByUserName(employee.Username);
                    employeeUpdate.JobId = JobId;
                    employeeUpdate.DepartmentId = DepartmentId;
                    _employeeService.Update(employeeUpdate);
                }
            } else
            {
                return Page();
            }
            return RedirectToPage("/ManagerPage/ManagerHomePage");
        }

        private Employee? ValidateEmployeeInformation(Employee employee)
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
            if (!employee.Account.Email.Equals(email))
            {
                if (_accountService.IsExistedEmail(email))
                {
                    ViewData["ErrorMessageEmail"] = "Email already exist. Please input another email!";
                    return null;
                }
            }
            employee.Account.Email = email;

            //Validate fullname
            var fullName = FullName.Trim();
            if (string.IsNullOrEmpty(fullName))
            {
                ViewData["ErrorMessageFullname"] = "Please input fullname!";
                return null;
            }
            employee.Account.FullName = fullName;

            //Validate phone
            var phone = PhoneNumber;
            if (!Regex.IsMatch(phone, @"^[0-9\s\+]+$"))
            {
                ViewData["ErrorMessagePhoneNumber"] = "Please enter valid phone number!";
                return null;
            }

            employee.Account.PhoneNumber = phone;

            if (Street != null || Ward != null || City != null || Province != null)
            {
                if (employee.Account.Address == null)
                {
                    var address = new Address();
                    address.Street = Street ?? "";
                    address.Ward = Ward ?? "";
                    address.City = City ?? "";
                    address.Province = Province ?? "";
                    employee.Account.Address = address;
                }
                else
                {
                    employee.Account.Address.Street = Street ?? "";
                    employee.Account.Address.Ward = Ward ?? "";
                    employee.Account.Address.City = City ?? "";
                    employee.Account.Address.Province = Province ?? "";
                }
            }
            return employee;
        }
    }
}
