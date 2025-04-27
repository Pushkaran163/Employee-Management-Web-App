using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Core.Models;
using MyApp.Service.Interfaces;
using NLog;

namespace MyApp.Web.Pages.Employees
{
    /// <summary>
    /// Handles displaying the employee data in a view-only mode.
    /// </summary>
    public class DataModel : PageModel
    {
        private static readonly Logger Logger = LogManager.GetLogger("MyApp.Web.Pages.Employees.Data");

        private readonly IEmployeeService _employeeService;

        public DataModel(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        /// <summary>
        /// Gets or sets the list of employees.
        /// </summary>
        public List<Employee> Employees { get; set; } = new List<Employee>();

        /// <summary>
        /// Search term used for filtering employees.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; } = string.Empty;

        /// <summary>
        /// Current page number for pagination.
        /// </summary>
        [BindProperty(SupportsGet = true)]
        public int CurrentPage { get; set; } = 1;

        /// <summary>
        /// Total number of pages available.
        /// </summary>
        public int TotalPages { get; set; }

        private const int PageSize = 10;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Logger.Info("Fetching all employees without server-side paging.");

                var (employees, totalCount) = await _employeeService.GetEmployeesAsync(SearchTerm, 1, int.MaxValue);

                Employees = employees;
                TotalPages = 1;

                Logger.Info("Successfully fetched {0} employees.", employees.Count);

                return Page();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while fetching employees.");
                TempData["ErrorMessage"] = "An unexpected error occurred while loading employees.";
                return RedirectToPage("/Error");
            }
        }
    }
}
