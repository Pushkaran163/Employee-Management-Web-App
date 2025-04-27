using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using MyApp.Core.Models;
using MyApp.Data.Context;
using MyApp.Service.Helpers;
using MyApp.Service.Interfaces;
using NLog; // <-- Added for NLog
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.Service.Services
{
    /// <summary>
    /// Handles employee-related business logic.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILoggerService _logger;
        private static readonly Logger _nlogger = LogManager.GetCurrentClassLogger(); // <-- NLog Logger

        public EmployeeService(AppDbContext dbContext, ILoggerService logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            try
            {
                _nlogger.Info("Fetching all employees.");
                return await (from e in _dbContext.Employees
                              join d in _dbContext.Departments on e.DepartmentId equals d.Id
                              select new Employee
                              {
                                  Id = e.Id,
                                  Name = e.Name,
                                  Email = e.Email,
                                  Phone = e.Phone,
                                  Status = e.Status,
                                  ProfileImage = e.ProfileImage,
                                  CreatedBy = e.CreatedBy,
                                  UpdatedBy = e.UpdatedBy,
                                  CreatedOnUtc = e.CreatedOnUtc,
                                  UpdatedOnUtc = e.UpdatedOnUtc,
                                  IpAddress = e.IpAddress,
                                  DepartmentId = e.DepartmentId,
                                  DepartmentName = d.Name
                              }).ToListAsync();
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, "Error occurred while fetching all employees.");
                throw;
            }
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            try
            {
                _nlogger.Info($"Fetching employee with ID: {id}");
                return await _dbContext.Employees.FindAsync(id);
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, $"Error occurred while fetching employee with ID: {id}");
                throw;
            }
        }

        public async Task AddEmployeeAsync(Employee employee)
        {
            try
            {
                await _dbContext.Employees.AddAsync(employee);
                await _dbContext.SaveChangesAsync();

                _logger.LogInfo("Employee added", employee.Name);
                _logger.LogAudit("Add", "Employee", employee.Id.ToString());

                _nlogger.Info($"Employee added successfully. Name: {employee.Name}");
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, "Error occurred while adding employee.");
                throw;
            }
        }

        public async Task<bool> UpdateEmployeeAsync(Employee updatedEmployee, HttpContext httpContext)
        {
            try
            {
                var existing = await _dbContext.Employees.FindAsync(updatedEmployee.Id);
                if (existing == null)
                {
                    _nlogger.Warn($"Employee with ID: {updatedEmployee.Id} not found for update.");
                    return false;
                }

                existing.Name = updatedEmployee.Name;
                existing.Email = updatedEmployee.Email;
                existing.Phone = updatedEmployee.Phone;
                existing.Status = updatedEmployee.Status;
                existing.DepartmentId = updatedEmployee.DepartmentId;

                // Handle profile image
                if (updatedEmployee.ProfileImageFile != null)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(updatedEmployee.ProfileImageFile.FileName)}";
                    var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadPath); // Ensure directory exists
                    var fullPath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await updatedEmployee.ProfileImageFile.CopyToAsync(stream);
                    }

                    existing.ProfileImage = fileName;
                }

                // Audit Info
                existing.UpdatedBy = httpContext.User.Identity?.Name ?? "System";
                existing.UpdatedOnUtc = DateTime.UtcNow;
                existing.IpAddress = httpContext.Connection.RemoteIpAddress?.ToString();

                await _dbContext.SaveChangesAsync();

                _logger.LogInfo("Employee updated", existing.Name);
                _logger.LogAudit("Update", "Employee", existing.Id.ToString());

                _nlogger.Info($"Employee updated successfully. ID: {existing.Id}");

                return true;
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, $"Error occurred while updating employee with ID: {updatedEmployee.Id}");
                throw;
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            try
            {
                var employee = await _dbContext.Employees.FindAsync(id);
                if (employee != null)
                {
                    _dbContext.Employees.Remove(employee);
                    await _dbContext.SaveChangesAsync();

                    _logger.LogInfo("Employee deleted", employee.Name);
                    _logger.LogAudit("Delete", "Employee", id.ToString());

                    _nlogger.Info($"Employee deleted successfully. ID: {id}");
                }
                else
                {
                    _nlogger.Warn($"Employee with ID: {id} not found for deletion.");
                }
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, $"Error occurred while deleting employee with ID: {id}");
                throw;
            }
        }

        public async Task<(List<Employee> Employees, int TotalCount)> GetEmployeesAsync(string searchTerm, int page, int pageSize)
        {
            try
            {
                _nlogger.Info($"Fetching employees with searchTerm='{searchTerm}', page={page}, pageSize={pageSize}.");

                var query = from e in _dbContext.Employees
                            join d in _dbContext.Departments on e.DepartmentId equals d.Id
                            select new Employee
                            {
                                Id = e.Id,
                                Name = e.Name,
                                Email = e.Email,
                                Phone = e.Phone,
                                Status = e.Status,
                                ProfileImage = e.ProfileImage,
                                CreatedBy = e.CreatedBy,
                                UpdatedBy = e.UpdatedBy,
                                CreatedOnUtc = e.CreatedOnUtc,
                                UpdatedOnUtc = e.UpdatedOnUtc,
                                IpAddress = e.IpAddress,
                                DepartmentId = e.DepartmentId,
                                DepartmentName = d.Name
                            };

                // Apply search
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    query = query.Where(e => e.Name.Contains(searchTerm) ||
                                             e.Email.Contains(searchTerm) ||
                                             e.Phone.Contains(searchTerm));
                }

                var totalCount = await query.CountAsync(); // Total before pagination

                var employees = await query
                    .OrderBy(e => e.Name) // You can change sorting logic
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                _nlogger.Info($"Fetched {employees.Count} employees out of total {totalCount} matching records.");

                return (employees, totalCount);
            }
            catch (Exception ex)
            {
                _nlogger.Error(ex, "Error occurred while fetching paged employees.");
                throw;
            }
        }
    }
}
