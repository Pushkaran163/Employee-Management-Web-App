using MyApp.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Service.Interfaces
{
    /// <summary>
    /// Department service interface
    /// </summary>
    public interface IDepartmentService
    {
        Task<List<Department>> GetAllDepartmentsAsync();
        Task<Department> GetDepartmentByIdAsync(int id);

        //Task<List<Department>> SearchDepartmentsAsync(string searchTerm);

        Task<(List<Department> Departments, int TotalCount)> GetDepartmentsAsync(string searchTerm, int currentPage, int pageSize);


        Task AddDepartmentAsync(Department department);
        Task UpdateDepartmentAsync(Department department);
        Task DeleteDepartmentAsync(int id);
    }
}
