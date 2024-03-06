using EmployeeManagementBO.Models;
using EmployeeManagementRepository;
using EmployeeManagementRepository.IRepository;
using EmployeeManagementService.IService;

namespace EmployeeManagementService;

/// <summary>
/// Service class for managing department.
/// </summary>
/// <author>TienPH</author>
public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepo departmentRepo;

    public DepartmentService()
    {
        departmentRepo = new DepartmentRepo();
    }

    /// <summary>
    /// Creates a new department in the database.
    /// </summary>
    /// <param name="department">The department object to be created.</param>
    /// <returns>True if the department is successfully created, otherwise false.</returns>    
    public bool Create(Department department)
    {
        return departmentRepo.Create(department);
    }

    /// <summary>
    /// Retrieves all department from the database.
    /// </summary>
    /// <returns>An IEnumerable of all department.</returns>    
    public IEnumerable<Department> GetAll()
    {
        return departmentRepo.GetAll();
    }

    /// <summary>
    /// Retrieves department by id from the database.
    /// </summary>
    /// /// <param name="departmentId">The department id.</param>
    /// <returns>A department.</returns>
    public Department GetDepartmentById(int departmentId)
    {
        return departmentRepo.GetAll().First(d => d.DepartmentId.Equals(departmentId));
    }


    /// <summary>
    /// Updates an existing department in the database.
    /// </summary>
    /// <param name="department">The department object to be updated.</param>
    /// <returns>True if the department is successfully updated, otherwise false.</returns>
    public bool Update(Department department)
    {
        return departmentRepo.Update(department);
    }

    /// <summary>
    /// Deletes a department from the database.
    /// </summary>
    /// <param name="department">The department object to be deleted.</param>
    /// <returns>True if the department is successfully deleted, otherwise false.</returns>
    public bool Delete(Department department)
    {
        return departmentRepo.Delete(department);
    }

    /// <summary>
    /// Check existing department by department id in the database.
    /// </summary>
    /// <param name="departmentId">The department id to be checked.</param>
    /// <returns>True if the department id existed, otherwise false.</returns>
    public bool IsExistingDepartment(int departmentId)
    {
        return departmentRepo.GetAll().Any(d => d.DepartmentId.Equals(departmentId));
    }
}
