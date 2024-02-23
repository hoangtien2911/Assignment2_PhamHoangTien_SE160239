using EmployeeManagementBO.Models;

namespace EmployeeManagementService;

/// <summary>
/// Interface for managing department in the service layer.
/// </summary>
/// <author>TienPH</author>
public interface IDepartmentService
{
    /// <summary>
    /// Creates a new department in the database.
    /// </summary>
    /// <param name="department">The department object to be created.</param>
    /// <returns>True if the department is successfully created, otherwise false.</returns>
    bool Create(Department department);

    /// <summary>
    /// Retrieves all department from the database.
    /// </summary>
    /// <returns>An IEnumerable of all department.</returns>
    IEnumerable<Department> GetAll();

    /// <summary>
    /// Retrieves department by id from the database.
    /// </summary>
    /// /// <param name="departmentId">The department id.</param>
    /// <returns>A department.</returns>
    Department GetDepartmentById(int departmentId);

    /// <summary>
    /// Updates an existing department in the database.
    /// </summary>
    /// <param name="department">The department object to be updated.</param>
    /// <returns>True if the department is successfully updated, otherwise false.</returns>
    bool Update(Department department);

    /// <summary>
    /// Deletes a department from the database.
    /// </summary>
    /// <param name="department">The department object to be deleted.</param>
    /// <returns>True if the department is successfully deleted, otherwise false.</returns>
    bool Delete(Department department);

    /// <summary>
    /// Check existing department by department id in the database.
    /// </summary>
    /// <param name="departmentId">The department id to be checked.</param>
    /// <returns>True if the department id existed, otherwise false.</returns>
    bool IsExistingDepartment(int departmentId);
}
