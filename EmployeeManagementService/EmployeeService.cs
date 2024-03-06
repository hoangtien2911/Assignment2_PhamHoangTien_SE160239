using EmployeeManagementBO.Models;
using EmployeeManagementRepository;
using EmployeeManagementRepository.IRepository;
using EmployeeManagementService.IService;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementService;

/// <summary>
/// Service class for managing employee.
/// </summary>
/// <author>TienPH</author>
public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepo employeeRepo;

    public EmployeeService()
    {
        employeeRepo = new EmployeeRepo();
    }

    /// <summary>
    /// Creates a new employee in the database.
    /// </summary>
    /// <param name="employee">The employee object to be created.</param>
    /// <returns>True if the employee is successfully created, otherwise false.</returns>    
    public bool Create(Employee employee)
    {
        return employeeRepo.Create(employee);
    }

    /// <summary>
    /// Retrieves all employee from the database.
    /// </summary>
    /// <returns>An IEnumerable of all employee.</returns>    
    public IEnumerable<Employee> GetAll()
    {
        return employeeRepo.GetAll();
    }

    /// <summary>
    /// Count total employees from the database.
    /// </summary>
    /// <returns>Total employees.</returns>
    public int CountTotalEmployee()
    {
        return employeeRepo.GetAll().Count();
    }

    /// <summary>
    /// Get employee by username from the database.
    /// </summary>
    /// <param name="username">The username of the account.</param>
    /// <returns>An employee, otherwise null</returns>    
    public Employee? GetEmployeeByUserName(string username)
    {
        return employeeRepo.GetAll().Where(e => e.Username.Equals(username)).FirstOrDefault();
    }

    /// <summary>
    /// Retrieves all employee include account address department job and history from the database.
    /// </summary>
    /// <param name="pageNum">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>An IEnumerable of all employee.</returns>
    public IEnumerable<Employee> GetAllIncludeAccountAddressDepartmentJobAndHistory(int pageNum, int pageSize)
    {
        return employeeRepo.GetAllInclude().Include(e => e.Account).Include(e => e.Account.Address).Include(e => e.Department).Include(e => e.Job).Include(e => e.JobHistories)
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    /// <summary>
    /// Retrieves employee by username include account address department job and history from the database.
    /// </summary>
    /// <returns>An employee.</returns>
    public Employee GetEmployeeByUsernameIncludeAccountAddressDepartmentJobAndHistory(string username)
    {
        return employeeRepo.GetAllInclude().Include(e => e.Account).Include(e => e.Account.Address).Include(e => e.Department).Include(e => e.Job).Include(e => e.JobHistories).First(e => e.Username.Equals(username));
    }

    /// <summary>
    /// Retrieves all employee by email include account department job from the database.
    /// </summary>
    /// <param name="email">The email to search for in employee records.</param>
    /// <param name="pageNum">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>An IEnumerable of all employee.</returns>
    public IEnumerable<Employee> GetAllByEmailIncludeAccountDepartmentJob(string email, int pageNum, int pageSize)
    {
        return employeeRepo.GetAllInclude().Include(e => e.Account).Include(e => e.Department).Include(e => e.Job)            
            .Where(e => e.Account.Email.Contains(email))
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    /// <summary>
    /// Retrieves all employee by name include account department job from the database.
    /// </summary>
    /// <param name="name">The name to search for in account records.</param>
    /// <param name="pageNum">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>An IEnumerable of all employee.</returns>
    public IEnumerable<Employee> GetAllByNameIncludeAccountDepartmentJob(string name, int pageNum, int pageSize)
    {
        return employeeRepo.GetAllInclude().Include(e => e.Account).Include(e => e.Department).Include(e => e.Job)
            .Where(e => e.Account.FullName.Contains(name))
            .Skip((pageNum - 1) * pageSize)
            .Take(pageSize)
            .ToList();
    }

    /// <summary>
    /// Get employee by username include job and department information from the database.
    /// </summary>
    /// <param name="username">The username of the account.</param>
    /// <returns>An employee.</returns>    
    public Employee? GetEmployeeByUserNameIncludeJobAndDepartment(string username)
    {
        return employeeRepo.GetAllInclude().Include(e => e.Job).Include(e => e.Department).Where(e => e.Username.Equals(username)).FirstOrDefault();
    }

    /// <summary>
    /// Get employee by username include account job and department information from the database.
    /// </summary>
    /// <param name="username">The username of the account.</param>
    /// <returns>An employee.</returns>    
    public Employee GetEmployeeByUserNameIncludeAccountJobAndDepartment(string username)
    {
        return employeeRepo.GetAllInclude().Include(e => e.Account).Include(e => e.Job).Include(e => e.Department).Where(e => e.Username.Equals(username)).First();
    }

    /// <summary>
    /// Updates an existing employee in the database.
    /// </summary>
    /// <param name="employee">The employee object to be updated.</param>
    /// <returns>True if the employee is successfully updated, otherwise false.</returns>    
    public bool Update(Employee employee)
    {
        return employeeRepo.Update(employee);
    }

    /// <summary>
    /// Deletes a employee from the database.
    /// </summary>
    /// <param name="employee">The employee object to be deleted.</param>
    /// <returns>True if the employee is successfully deleted, otherwise false.</returns>
    public bool Delete(Employee employee)
    {
        return employeeRepo.Delete(employee);
    }
}
