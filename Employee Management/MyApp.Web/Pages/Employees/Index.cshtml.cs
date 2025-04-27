using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Core.Models;
using MyApp.Service.Interfaces;
using NLog;

namespace MyApp.Web.Pages.Employees
{
    /// <summary>
    /// Handles displaying and managing the list of employees.
    /// </summary>
    public class IndexModel : PageModel
    {
        private static readonly Logger Logger = LogManager.GetLogger("MyApp.Web.Pages.Employees.Index");

        private readonly IEmployeeService _employeeService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="employeeService">Service for employee operations.</param>
        public IndexModel(IEmployeeService employeeService)
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

        /// <summary>
        /// Handles the GET request to retrieve and display employees.
        /// </summary>
        /// <returns>The page with employee data.</returns>
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


        /// <summary>
        /// Handles the POST request to delete an employee.
        /// </summary>
        /// <param name="EmployeeId">The ID of the employee to delete.</param>
        /// <returns>Redirects back to the index page.</returns>
        public async Task<IActionResult> OnPostDeleteAsync(int EmployeeId)
        {
            if (EmployeeId == 0)
            {
                Logger.Warn("Delete attempt with invalid EmployeeId: 0");
                return NotFound();
            }

            try
            {
                Logger.Info("Attempting to delete employee with ID: {0}", EmployeeId);

                await _employeeService.DeleteEmployeeAsync(EmployeeId);

                TempData["SuccessMessage"] = "Employee deleted successfully.";

                Logger.Info("Employee with ID {0} deleted successfully.", EmployeeId);

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while deleting employee with ID: {0}", EmployeeId);
                TempData["ErrorMessage"] = "An unexpected error occurred while deleting the employee.";
                return RedirectToPage("/Error");
            }
        }
    }
}
