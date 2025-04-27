using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MyApp.Core.Models;
using MyApp.Service.Interfaces;
using NLog;

namespace MyApp.Web.Pages.Departments
{
    /// <summary>
    /// Edit an existing department.
    /// </summary>
    public class EditModel : PageModel
    {
        private static readonly Logger Logger = LogManager.GetLogger("MyApp.Web.Pages.Departments.Edit");

        private readonly IDepartmentService _departmentService;

        public EditModel(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        /// <summary>
        /// Department model bound to form.
        /// </summary>
        [BindProperty]
        public Department Department { get; set; }

        /// <summary>
        /// Handles GET request to load existing department data.
        /// </summary>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Logger.Info("Fetching department details for ID: {0}", id);

                Department = await _departmentService.GetDepartmentByIdAsync(id);

                if (Department == null)
                {
                    Logger.Warn("Department with ID: {0} not found.", id);
                    return NotFound();
                }

                Logger.Info("Successfully loaded department for editing. ID: {0}", id);
                return Page();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while loading the department for editing. ID: {0}", id);
                TempData["ErrorMessage"] = "An unexpected error occurred while loading the department.";
                return RedirectToPage("/Error");
            }
        }

        /// <summary>
        /// Handles POST request to update department.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Logger.Warn("Validation failed while editing department. Errors: {0}",
                    string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)));
                return Page();
            }

            try
            {
                Logger.Info("Attempting to update department. ID: {0}", Department.Id);

                var existingDepartment = await _departmentService.GetDepartmentByIdAsync(Department.Id);

                if (existingDepartment == null)
                {
                    Logger.Warn("Department with ID: {0} not found during update.", Department.Id);
                    return NotFound();
                }

                // Only update editable fields
                existingDepartment.Name = Department.Name;
                existingDepartment.Description = Department.Description;

                // Preserve non-editable audit fields
                existingDepartment.UpdatedBy = "Admin";
                existingDepartment.UpdatedOnUtc = DateTime.UtcNow;
                existingDepartment.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                await _departmentService.UpdateDepartmentAsync(existingDepartment);

                Logger.Info("Successfully updated department. ID: {0}", Department.Id);
                TempData["SuccessMessage"] = "Department updated successfully.";

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error occurred while updating department. ID: {0}", Department.Id);
                ModelState.AddModelError(string.Empty, $"Error: {ex.InnerException?.Message ?? ex.Message}");
                return Page();
            }
        }
    }
}
