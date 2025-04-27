using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Core.Models;
using MyApp.Service.Interfaces;
using NLog;

namespace MyApp.Web.Pages.Departments
{
    /// <summary>
    /// Shows list of departments
    /// </summary>
    public class IndexModel : PageModel
    {
        private static readonly Logger Logger = LogManager.GetLogger("MyApp.Web.Pages.Departments.Index");

        private readonly IDepartmentService _departmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexModel"/> class.
        /// </summary>
        /// <param name="departmentService">Service for department operations.</param>
        public IndexModel(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Gets or sets the list of departments.
        /// </summary>
        public List<Department> Departments { get; set; } = new List<Department>();

        /// <summary>
        /// Search term used for filtering departments.
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
        /// ID of the department to delete.
        /// </summary>
        [BindProperty]
        public int DeleteId { get; set; }

        /// <summary>
        /// Handles GET request to load departments with optional search and pagination.
        /// </summary>
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Logger.Info("Fetching all departments without server-side paging.");

                var (departments, totalCount) = await _departmentService.GetDepartmentsAsync(SearchTerm, 1, int.MaxValue); 

                Departments = departments;
                TotalPages = 1;

                Logger.Info("Successfully fetched {0} departments. TotalRecords: {1}.", departments.Count, totalCount);

                return Page();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while fetching departments.");
                TempData["ErrorMessage"] = "An unexpected error occurred while loading departments.";
                return RedirectToPage("/Error");
            }
        }


        /// <summary>
        /// Handles POST request to delete a department.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (DeleteId > 0)
                {
                    Logger.Info("Attempting to delete Department with ID: {0}", DeleteId);

                    await _departmentService.DeleteDepartmentAsync(DeleteId);

                    Logger.Info("Successfully deleted Department with ID: {0}", DeleteId);
                    TempData["SuccessMessage"] = "Department deleted successfully.";
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred while deleting Department with ID: {0}", DeleteId);
                ModelState.AddModelError(string.Empty, $"Error: {ex.InnerException?.Message ?? ex.Message}");
            }

            return RedirectToPage();
        }
    }
}
