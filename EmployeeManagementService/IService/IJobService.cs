using EmployeeManagementBO.Models;

namespace EmployeeManagementService.IService;

/// <summary>
/// Interface for managing job in the service layer.
/// </summary>
/// <author>TienPH</author>
public interface IJobService
{
    /// <summary>
    /// Creates a new job in the database.
    /// </summary>
    /// <param name="job">The job object to be created.</param>
    /// <returns>True if the job is successfully created, otherwise false.</returns>
    bool Create(Job job);

    /// <summary>
    /// Retrieves all job from the database.
    /// </summary>
    /// <returns>An IEnumerable of all job.</returns>
    IEnumerable<Job> GetAll();

    /// <summary>
    /// Retrieves job by id from the database.
    /// </summary>
    /// /// <param name="jobId">The job id.</param>
    /// <returns>A job.</returns>
    Job GetJobById(int jobId);


    /// <summary>
    /// Updates an existing job in the database.
    /// </summary>
    /// <param name="job">The job object to be updated.</param>
    /// <returns>True if the job is successfully updated, otherwise false.</returns>
    bool Update(Job job);

    /// <summary>
    /// Deletes a job from the database.
    /// </summary>
    /// <param name="job">The job object to be deleted.</param>
    /// <returns>True if the job is successfully deleted, otherwise false.</returns>
    bool Delete(Job job);

    /// <summary>
    /// Check existing job by job id in the database.
    /// </summary>
    /// <param name="jobId">The job id to be checked.</param>
    /// <returns>True if the job id existed, otherwise false.</returns>
    bool IsExistingJob(int jobId);
}
