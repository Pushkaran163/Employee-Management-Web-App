using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyApp.Core.Models;
using MyApp.Service.Interfaces;
using NLog;

namespace MyApp.Web.Pages.Employees
{
    /// <summary>
    /// Handles adding a new employee.
    /// </summary>
    public class CreateModel : PageModel
    {
        private static readonly Logger Logger = LogManager.GetLogger("MyApp.Web.Pages.Employees.Create");

        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateModel"/> class.
        /// </summary>
        /// <param name="employeeService">Service for employee operations.</param>
        /// <param name="departmentService">Service for department operations.</param>
        public CreateModel(IEmployeeService employeeService, IDepartmentService departmentService)
        {
            _employeeService = employeeService;
            _departmentService = departmentService;
        }

        /// <summary>
        /// Gets or sets the employee to add.
        /// </summary>
        [BindProperty]
        public Employee Employee { get; set; }

        /// <summary>
        /// Gets or sets the department list for dropdown selection.
        /// </summary>
        public List<SelectListItem> Departments { get; set; }

        /// <summary>
        /// Handles the GET request to load department data.
        /// </summary>
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                Logger.Info("Loading department list for employee creation.");
                await LoadDepartmentsAsync();
                return Page();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while loading department data.");
                TempData["ErrorMessage"] = "An unexpected error occurred while preparing the employee creation form.";
                return RedirectToPage("/Error");
            }
        }

        /// <summary>
        /// Handles the POST request to create a new employee.
        /// </summary>
        public async Task<IActionResult> OnPostAsync()
        {
            foreach (var state in ModelState)
            {
                if (state.Value.Errors.Count > 0)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Logger.Warn("Validation Error - Field: {0}, Message: {1}", state.Key, error.ErrorMessage);
                    }
                }
            }

            if (!ModelState.IsValid)
            {
                Logger.Warn("Model validation failed while creating employee.");
                await LoadDepartmentsAsync();
                return Page();
            }

            try
            {
                if (Employee.ProfileImageFile != null)
                {
                    var fileName = Path.GetFileName(Employee.ProfileImageFile.FileName);
                    var filePath = Path.Combine("wwwroot/uploads", fileName);

                    Logger.Info("Uploading profile image for employee: {0}", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Employee.ProfileImageFile.CopyToAsync(stream);
                    }

                    Employee.ProfileImage = fileName;
                }

                Employee.CreatedBy = "Admin";
                Employee.UpdatedBy = "Admin";
                Employee.CreatedOnUtc = DateTime.UtcNow;
                Employee.UpdatedOnUtc = DateTime.UtcNow;
                Employee.IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

                await _employeeService.AddEmployeeAsync(Employee);

                TempData["SuccessMessage"] = "Employee added successfully.";
                Logger.Info("Employee {0} added successfully.", Employee.Name);

                return RedirectToPage("Index");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "An error occurred while creating a new employee.");
                TempData["ErrorMessage"] = "An unexpected error occurred while adding the employee.";
                return RedirectToPage("/Error");
            }
        }

        /// <summary>
        /// Loads the department dropdown list.
        /// </summary>
        private async Task LoadDepartmentsAsync()
        {
            var departments = await _departmentService.GetAllDepartmentsAsync();
            Departments = departments.Select(d => new SelectListItem
            {
                Value = d.Id.ToString(),
                Text = d.Name
            }).ToList();
        }
    }
}
