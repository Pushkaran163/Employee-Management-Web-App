using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyApp.Core.Models;
using MyApp.Service.Interfaces;
using NLog;

namespace MyApp.Web.Pages.Employees
{
    /// <summary>
    /// Handles editing an employee's information.
    /// </summary>
    public class EditModel : PageModel
    {
        private static readonly Logger Logger = LogManager.GetLogger("MyApp.Web.Pages.Employees.Edit");

        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="EditModel"/> class.
        /// </summary>
        /// <param name="employeeService">Service for employee operations.</param>
        /// <param name="departmentService">Service for department operations.</param>
        public EditModel(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        /// <summary>
        /// Gets or sets the employee to edit.
        /// </summary>
        [BindProperty]
        public Employee Employee { get; set; }

        /// <summary>
        /// Gets or sets the department list for dropdown selection.
        /// </summary>
        public SelectList DepartmentList { get; set; }

        /// <summary>
        /// Handles the GET request to load employee data for editing.
        /// </summary>
        /// <param name="id">The employee ID.</param>
        /// <returns>The edit page or not found if employee doesn't exist.</returns>
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Logger.Info("Fetching employee with ID: {0} for editing.", id);

                Employee = await _employeeService.GetEmployeeByIdAsync(id);

                if (Employee == null)
                {
                    Logger.Warn("Employee with ID {0} not found.", id);
                    return NotFound();
                }

                await LoadDepartmentListAsync();
                Logger.Info("Successfully loaded employee and department data for editing.");

                return Page();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while fetching employee with ID: {0}", id);
                TempData["ErrorMessage"] = "An unexpected error occurred while loading employee data.";
                return RedirectToPage("/Error");
            }
        }

        /// <summary>
        /// Handles the POST request to update employee data.
        /// </summary>
        /// <returns>Redirects to the index page if successful; reloads page if validation fails.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                Logger.Warn("Validation failed while updating employee with ID: {0}.", Employee?.Id);

                foreach (var error in ModelState)
                {
                    foreach (var subError in error.Value.Errors)
                    {
                        Logger.Warn("Validation Error - Field: {0}, Message: {1}", error.Key, subError.ErrorMessage);
                    }
                }

                await LoadDepartmentListAsync();
                return Page();
            }

            try
            {
                Logger.Info("Updating employee with ID: {0}.", Employee.Id);

                // Preserve existing image if no new image uploaded
                if (Employee.ProfileImageFile == null)
                {
                    var existingEmployee = await _employeeService.GetEmployeeByIdAsync(Employee.Id);

                    if (existingEmployee == null)
                    {
                        Logger.Warn("Existing employee with ID {0} not found during update.", Employee.Id);

                        ModelState.AddModelError(string.Empty, "Unable to update employee.");
                        await LoadDepartmentListAsync();
                        return Page();
                    }

                    Employee.ProfileImage = existingEmployee.ProfileImage;
                }

                var result = await _employeeService.UpdateEmployeeAsync(Employee, Request.HttpContext);

                if (!result)
                {
                    Logger.Warn("Failed to update employee with ID: {0}.", Employee.Id);

                    ModelState.AddModelError(string.Empty, "Unable to update employee.");
                    await LoadDepartmentListAsync();
                    return Page();
                }

                TempData["SuccessMessage"] = "Employee updated successfully.";
                Logger.Info("Employee with ID {0} updated successfully.", Employee.Id);

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while updating employee with ID: {0}.", Employee.Id);
                TempData["ErrorMessage"] = "An unexpected error occurred while updating the employee.";
                return RedirectToPage("/Error");
            }
        }

        /// <summary>
        /// Loads the department dropdown list.
        /// </summary>
        private async Task LoadDepartmentListAsync()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            DepartmentList = new SelectList(departments, "Id", "Name");
        }
    }
}
