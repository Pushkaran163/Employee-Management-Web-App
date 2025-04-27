using System;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Core.Models
{
    /// <summary>
    /// Represents a department in the company.
    /// </summary>
    public class Department
    {
        /// <summary>Primary key</summary>
        public int Id { get; set; }

        /// <summary>Name of the department</summary>
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        /// <summary>Short description</summary>
        public string? Description { get; set; }

        /// <summary>User who created the department</summary>
        public string? CreatedBy { get; set; }

        /// <summary>User who last updated the department</summary>
        public string? UpdatedBy { get; set; }

        /// <summary>UTC creation timestamp</summary>
        public DateTime CreatedOnUtc { get; set; }

        /// <summary>UTC update timestamp</summary>
        public DateTime UpdatedOnUtc { get; set; }

        /// <summary>IP address of the creator/updater</summary>
        public string? IpAddress { get; set; }
    }
}
