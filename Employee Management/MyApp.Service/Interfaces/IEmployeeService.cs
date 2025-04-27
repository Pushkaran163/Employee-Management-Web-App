using Microsoft.AspNetCore.Http;
using MyApp.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyApp.Service.Interfaces
{
    /// <summary>
    /// Employee service interface
    /// </summary>
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task AddEmployeeAsync(Employee employee);
        Task<bool> UpdateEmployeeAsync(Employee employee, HttpContext httpContext); // ← FIXED
        //Task<List<Employee>> GetEmployeesAsync(string searchTerm, string sortBy);

        //Task<List<Employee>> SearchEmployeesAsync(string searchTerm);


        Task<(List<Employee> Employees, int TotalCount)> GetEmployeesAsync(string searchTerm, int page, int pageSize);


        Task DeleteEmployeeAsync(int id);
    }
}

