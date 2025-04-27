using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Core.Models;
using MyApp.Service.Interfaces;
using NLog;

namespace MyApp.Web.Pages.Departments
{
    /// <summary>
    /// Create a new department
    /// </summary>
    public class CreateModel : PageModel
    {
        private static readonly Logger Logger = LogManager.GetLogger("MyApp.Web.Pages.Departments.Create");

        private readonly IDepartmentService _departmentService;

        public CreateModel(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        [BindProperty]
        public Department Department { get; set; }

        /// <summary>
        /// Handles GET request to display the create page.
        /// </summary>
        public void OnGet()
        {
            Logger.Info("Accessed Department Create Page.");
        }

        /// <summary>
        /// Handles POST request to create a new department.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Logger.Warn("Validation failed while creating department. Errors: {0}",
                    string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return Page();
            }

            try
            {
                Logger.Info("Attempting to create a new department. Name: {0}", Department.Name);

                Department.CreatedBy = "Admin";
                Department.UpdatedBy = "Admin";
                Department.CreatedOnUtc = DateTime.UtcNow;
                Department.UpdatedOnUtc = DateTime.UtcNow;
                Department.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                await _departmentService.AddDepartmentAsync(Department);

                Logger.Info("Department created successfully. Name: {0}", Department.Name);

                TempData["SuccessMessage"] = "Department Added Successfully.";
                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                Logger.Error(ex, "An error occurred while creating department. Error: {0}", errorMessage);

                ModelState.AddModelError(string.Empty, $"Error: {errorMessage}");
                return Page();
            }
        }
    }
}
