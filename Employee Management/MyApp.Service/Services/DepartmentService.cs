using Microsoft.EntityFrameworkCore;
using MyApp.Core.Models;
using MyApp.Data.Context;
using MyApp.Service.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Service.Services
{
    /// <summary>
    /// Handles department-related business logic.
    /// </summary>
    public class DepartmentService : IDepartmentService
    {
        private static readonly Logger _nlogger = LogManager.GetCurrentClassLogger();
        private readonly AppDbContext _context;
        private readonly ILoggerService _logger;

        public DepartmentService(AppDbContext context, ILoggerService logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            _nlogger.Info("Fetching all departments");
            return await _context.Departments.ToListAsync();
        }

        public async Task<Department> GetDepartmentByIdAsync(int id)
        {
            _nlogger.Info($"Fetching department by Id: {id}");
            return await _context.Departments.FindAsync(id);
        }

        public async Task AddDepartmentAsync(Department department)
        {
            _nlogger.Info($"Adding new department: {department.Name}");
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();

            _logger.LogInfo("Department added", department.Name);
            _logger.LogAudit("Add", "Department", department.Id.ToString());
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            _nlogger.Info($"Updating department: {department.Name}");
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();

            _logger.LogInfo("Department updated", department.Name);
            _logger.LogAudit("Update", "Department", department.Id.ToString());
        }

        public async Task DeleteDepartmentAsync(int id)
        {
            _nlogger.Info($"Deleting department with Id: {id}");
            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                _nlogger.Warn($"Department with Id {id} not found for deletion");
                throw new ArgumentException("Department not found.", nameof(id));
            }

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();

            _logger.LogInfo("Department deleted", department.Name);
            _logger.LogAudit("Delete", "Department", id.ToString());
        }

        /// <summary>
        /// Retrieves a paginated and searchable list of departments.
        /// </summary>
        /// <param name="searchTerm">The search term for filtering by name.</param>
        /// <param name="currentPage">The current page number.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <returns>A tuple containing the list of departments and the total count.</returns>
        public async Task<(List<Department> Departments, int TotalCount)> GetDepartmentsAsync(string searchTerm, int currentPage, int pageSize)
        {
            _nlogger.Info($"Fetching departments with searchTerm: '{searchTerm}', page: {currentPage}, pageSize: {pageSize}");
            var query = _context.Departments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(d => d.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var departments = await query
                .OrderBy(d => d.Name)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (departments, totalCount);
        }
    }
}
